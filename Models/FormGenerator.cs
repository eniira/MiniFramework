using System.Globalization;

namespace MineFramework.Models
{
    public class Form
    {
        static void Main(string[] args)
        {
            var input = args[0];
            var types = GetTypes(input);
            GenerateForms (types);
            GenerateDatabaseAccess (types);
        }

        static List<Type> GetTypes(string input)
        {
            var assembly = System.Reflection.Assembly.LoadFrom(input);
            return assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract)
                .ToList();
        }


        public static void GenerateForms(List<Type> types)
        {
            var formHtmlTemplate =
                @"
            <html>
                <body>
                    <form method=""POST"">
                        VAR_FIELDS
                        <input type=""submit"" value=""Enviar"">
                    </form>
                </body>
            </html>
            ";
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
            foreach (var type in types)
            {
                var fields =
                    type
                        .GetFields()
                        .Where(field =>
                            field.FieldType.IsSubclassOf(typeof (Campo)))
                        .ToList();
                var formFields = "";

                foreach (var field in fields)
                {
                    var inputType = htmlFields[field.FieldType];
                    var inputName = field.Name;
                    var showName =
                        ((Campo) field.GetValue(null)).Verboso
                            ?? inputName.Replace("_", " ");

                    formFields +=
                        inputType
                            .Replace("VAR_NAME", inputName)
                            .Replace("SHOW_NAME", showName);
                }
                var formHtml =
                    formHtmlTemplate.Replace("VAR_FIELDS", formFields);
                System
                    .IO
                    .File
                    .WriteAllText($"./Views/Home/Cadastrar.cshtml",
                    formHtml);
            }
        }

        static void GenerateDatabaseAccess(List<Type> types)
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
                    [HttpGet(""/"")]
                    public IActionResult Index()
                    {
                        return View();
                    }

                    [HttpPost(""/processar_formulario"")]
                    public IActionResult ProcessForm(IFormCollection form)
                    {
                        using (var conn = new SQLiteConnection(""Data Source=formularios.db;Version=3;""))
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
                            field.FieldType.IsSubclassOf(typeof (Campo)))
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

                System.IO.File.WriteAllText($"{tableName}_db.cs", dbAccess);
            }
        }
    }

    public abstract class Campo
    {
        public string Verboso { get; set; }
    }

    public class Frase : Campo { }

    public class Texto : Campo { }

    public class Data : Campo { }
}
