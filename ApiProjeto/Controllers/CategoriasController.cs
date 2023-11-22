using ApiProjeto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly string MySqlConnection = @"Server=20.226.185.146;Port=3306;Database=bd_projeto;Uid=root;Pwd=1234;";

        [HttpGet]
        public IActionResult RetornaCategorias()
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"SELECT * FROM categoria";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                var reader = comando.ExecuteReader();

                List<Categoria> listaCategoria = new List<Categoria>();

                while (reader.Read())
                {
                    Categoria item = new Categoria();
                    item.id = int.Parse(reader["id"].ToString());
                    item.nome_categoria = reader["nome_categoria"].ToString();
                    item.descricao_categoria = reader["descricao_categoria"].ToString();
                    listaCategoria.Add(item);
                }

                return Ok(listaCategoria);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }

        [HttpPost]
        public ActionResult NovaCategoria([FromBody] Categoria categoria)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"INSERT INTO categoria(nome_categoria, descricao_categoria) VALUES(@nome, @descricao)";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", categoria.nome_categoria);
                comando.Parameters.AddWithValue("@descricao", categoria.descricao_categoria);

                int linhasAfetadas = comando.ExecuteNonQuery();

                if(linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posssível inserir a categoria!");
                }

                return Created("", categoria.nome_categoria);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            finally
            {
                conexao.Close();
            }
        }
    }
}
