using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using MiniFramework.Models;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Text;
using System.Threading.Tasks;

namespace MiniFramework.Controllers;

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

        /*[HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {
            Usuario.Salvar(usuario);
            return RedirectToAction("Usuarios");
        }*/

        [HttpPost]
        public IActionResult Cadastrar(Usuario usuario)
        {   
            using (var conn = new SQLiteConnection("Data Source=database.db;Version=3;New=True;Compress=True;"))
            {
                conn.Open();
                using(var command = conn.CreateCommand())
                {   
                    //string criar = "CREATE DATABASE IF NOT EXISTS db";
                    //command.CommandText = criar;
                    //command.ExecuteNonQuery();
                    string tabela = "CREATE TABLE IF NOT EXISTS Test (id INTEGER PRIMARY KEY AUTOINCREMENT,nome VARCHAR(30),email VARCHAR(100))";
                    //command.CommandText = ""CREATE TABLE IF NOT EXISTS VAR_NAME (id INTEGER PRIMARY KEY AUTOINCREMENT, VAR_FIELDS)"";
                    command.CommandText = tabela;
                    command.ExecuteNonQuery();
                    //command.CommandText = ""INSERT INTO VAR_NAME (VAR_NAMES) VALUES (VAR_VALUES)"";
                    //VAR_COMMAND_PARAMS
                    command.CommandText = @"INSERT INTO Test (nome,email) VALUES (@getNome, @getEmail)";
                    command.Parameters.AddWithValue("getNome", usuario.Nome);
                    command.Parameters.AddWithValue("getEmail", usuario.Email);
                    command.ExecuteNonQuery();
                }
            }

            return Ok("Dados salvos com sucesso!");
        }

        public IActionResult Usuarios()
        {
            return View(Usuario.Listagem);
        }
}
