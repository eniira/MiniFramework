using Microsoft.AspNetCore.Mvc;
using MiniFramework.Models;
using System.Data.SQLite;


namespace MiniFramework.Controllers;

public class Pessoa {
    public int Id { get; set; }
    public string? Nome { get; set; }
    public DateTime DataNascimento { get; set; }
}

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Cadastrar(int? id)
    {
        if (id.HasValue && Usuario.Listagem.Any(u => u.IdUsuario == id))
        {
            var usuario = Usuario.Listagem.Single(u => u.IdUsuario == id);
            return View(usuario);
        }
        return View();
    }

  
    public IActionResult Form() {
        List<Type> types = new List<Type>();
        types.Add(typeof(Pessoa));
        string formHtml = MineFramework.Models.Form.GenerateForms(types);
        ViewBag.FormHtml = formHtml;
        return View("Cadastrar");
    }
    
    

    [HttpPost]  
    public IActionResult Cadastrar(Usuario usuario)
    {
       
        Usuario.Salvar(usuario);
        using (var conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;"))
        {
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                string tabela = "CREATE TABLE IF NOT EXISTS Test (id INTEGER PRIMARY KEY AUTOINCREMENT,nome VARCHAR(30),email VARCHAR(100))";
                command.CommandText = tabela;
                command.ExecuteNonQuery();

                string sql = "INSERT INTO Test (nome, email) VALUES (@nome, @email)";
                command.CommandText = sql;
                // command.Parameters.AddWithValue("id", usuario.IdUsuario);
                 command.Parameters.AddWithValue("@nome", usuario.Nome);
                 command.Parameters.AddWithValue("@email", usuario.Email); 
                command.ExecuteNonQuery();
                Console.WriteLine(usuario.Nome);
            }
        }

        return Ok(usuario.Nome);
    }
    public IActionResult Usuarios()
    {
        return View(Usuario.Listagem);
    }
}
