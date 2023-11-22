using ApiProjeto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace ApiProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    
    public class ProdutosController : ControllerBase
    {
        private readonly string MySqlConnection = @"Server=20.226.185.146;Port=3306;Database=bd_projeto;Uid=root;Pwd=1234;";

        [HttpGet]
        public IActionResult RetornaProdutos()
        {
            try
            {
                MySqlConnection conn = new MySqlConnection(MySqlConnection);

                conn.Open();

                string sql = @"SELECT * FROM PRODUTO";

                MySqlCommand cmd = new MySqlCommand(sql, conn);
                var reader = cmd.ExecuteReader();

                List<Produto> lista = new List<Produto>();

                while (reader.Read())
                {
                    Produto item = new Produto();
                    item.id = int.Parse(reader["id"].ToString());
                    item.nome_produto = reader["nome_produto"].ToString();
                    item.descricao_produto = reader["descricao_produto"].ToString();
                    item.valor = decimal.Parse(reader["valor"].ToString());
                    item.id_categoria = int.Parse(reader["id_categoria"].ToString());
                    lista.Add(item);
                }

              
                return Ok(lista);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
