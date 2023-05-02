using System.Text;
using System.Reflection;

namespace MineFramework.Models
{
    public class Form
    {
        static void Main(string[] args)
        {
            var input = args[0];
            var types = GetTypes(input);
            GenerateForms(types);
            GenerateDatabaseAccess(types);
        }

        static List<Type> GetTypes(string input)
        {
            var assembly = System.Reflection.Assembly.LoadFrom(input);
            return assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .ToList();
        }



        public static string GenerateForms(List<Type> types)
        {
            StringBuilder sb = new StringBuilder();
            var fieldHtmlTemplate =
                @"
                        <label for=""VAR_NAME"">SHOW_NAME:</label>
                        VAR_INPUT_TYPE
                        <br><br>";

            var htmlFields =
                new Dictionary<Type, string>()
                {
                    {
                        typeof (Frase),
                        fieldHtmlTemplate
                            .Replace("VAR_INPUT_TYPE",
                            "<input type=\"text\" id=\"VAR_NAME\" name=\"VAR_NAME\">")
                    },
                    {
                        typeof (Texto),
                        fieldHtmlTemplate
                            .Replace("VAR_INPUT_TYPE",
                            "<textarea id=\"VAR_NAME\" name=\"VAR_NAME\" rows=\"4\" cols=\"50\"></textarea>")
                    },
                    {
                        typeof (Data),
                        fieldHtmlTemplate
                            .Replace("VAR_INPUT_TYPE",
                            "<input type=\"date\" id=\"VAR_NAME\" name=\"VAR_NAME\">")
                    }
                };
            foreach (Type type in types)
            {
                PropertyInfo[] properties = type.GetProperties();

                // sb.AppendFormat("<form method =""POST"">");
                sb.AppendLine();

                foreach (PropertyInfo property in properties)
                {
                    sb.AppendFormat("<label>{0}</label>", property.Name);
                    sb.AppendFormat("<input type='text' name='{0}' />", property.Name);
                    sb.AppendLine();
                }

                sb.AppendFormat("<input type='submit' value='Submit' />");
                sb.AppendLine();
                // sb.AppendFormat("</form>");
                sb.AppendLine();
            }
             

            return sb.ToString();
        }

        public static void GenerateDatabaseAccess(List<Type> types)
        {
            var dbAccessTemplate =
                @"
            using System;
            using System.Collections.Generic;
            using System.Data.SQLite;
            using System.IO;
            using System.Linq;
            using System.Threading.Tasks;
            using Microsoft.AspNetCore.Http;
            using Microsoft.AspNetCore.Mvc;

            namespace FormsDatabaseAccess
            {
                public class VAR_NAMEController : Controller
                {
                    [HttpGet]
                    public IActionResult Index()
                    {
                        return View();
                    }

                    [HttpPost]
                    public IActionResult ProcessForm(IFormCollection form)
                    {
                        using (var conn = new SQLiteConnection(""Data Source=formularios.db;Version=3,New=True;Compress=True;""))
                        {
                            conn.Open();

                            var command = conn.CreateCommand();
                            command.CommandText = ""CREATE TABLE IF NOT EXISTS VAR_NAME (id INTEGER PRIMARY KEY AUTOINCREMENT, VAR_FIELDS)"";
                            command.ExecuteNonQuery();

                            command.CommandText = ""INSERT INTO VAR_NAME (VAR_NAMES) VALUES (VAR_VALUES)"";
                            VAR_COMMAND_PARAMS
                            command.ExecuteNonQuery();
                        }

                        return Ok(""dados salvos com sucesso!"");
                    }
                }
            }";

            var fieldSqlTypes =
                new Dictionary<Type, string>()
                {
                    { typeof (Frase), "TEXT" },
                    { typeof (Texto), "TEXT" },
                    { typeof (Data), "TEXT" }
                };

            foreach (var type in types)
            {
                var fields =
                    type
                        .GetFields()
                        .Where(field =>
                            field.FieldType.IsSubclassOf(typeof(Campo)))
                        .ToList();

                var tableName = type.Name.ToLower();
                var tableFields = "";
                var tableParams = "";

                foreach (var field in fields)
                {
                    var fieldName = field.Name;
                    var sqlType = fieldSqlTypes[field.FieldType];

                    tableFields += $"{fieldName} {sqlType},";
                    tableParams += $"@{fieldName},";
                }

                tableFields = tableFields.TrimEnd(',');
                tableParams = tableParams.TrimEnd(',');

                var dbAccess =
                    dbAccessTemplate
                        .Replace("VAR_NAME", type.Name)
                        .Replace("VAR_FIELDS", tableFields)
                        .Replace("VAR_COMMAND_PARAMS", tableParams);

                System.IO.File.WriteAllText($"./Views/Home/Form.cshtml", dbAccess);

            }
        }
    }

    public abstract class Campo
    {
        public string? Verboso { get; set; }
    }

    public class Frase : Campo { }

    public class Texto : Campo { }

    public class Data : Campo { }
}
