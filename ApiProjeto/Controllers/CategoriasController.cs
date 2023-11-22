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

        [HttpPut]
        [Route("{id:int}")]
        public ActionResult AlteraCategoria(int id, [FromBody] Categoria categoria)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {

                if(id != categoria.id)
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, "Id da URL e do Body não correspondem");
                }

                conexao.Open();

                string sqlConsulta = @"SELECT * FROM categoria WHERE id = @id";

                MySqlCommand categoriaExistente = new MySqlCommand(sqlConsulta, conexao);

                categoriaExistente.Parameters.AddWithValue("@id", id);

                var readerCategoria = categoriaExistente.ExecuteReader();

                if (!readerCategoria.Read())
                {
                    return NotFound($"Categoria de ID {id} não encontrado");
                }

                readerCategoria.Close();

                string updateCategoria = @"UPDATE categoria SET nome_categoria = @nome, descricao_categoria = @descricao WHERE id = @id";

                MySqlCommand updateComando = new MySqlCommand(updateCategoria, conexao);

                updateComando.Parameters.AddWithValue("@id", categoria.id);
                updateComando.Parameters.AddWithValue("@nome", categoria.nome_categoria);
                updateComando.Parameters.AddWithValue("@descricao", categoria.descricao_categoria);

                int linhasAfetadas = updateComando.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posível realizar a alteração da categoria!");
                }

                return Ok();
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

        [HttpDelete]
        [Route("{id:int}")]
        public ActionResult DeletaCategoria(int id)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"SELECT * FROM categoria WHERE id = @id";

                MySqlCommand comandoBusca = new MySqlCommand(sql, conexao);

                comandoBusca.Parameters.AddWithValue("@id", id);

                var reader = comandoBusca.ExecuteReader();

                if (!reader.Read())
                {
                    return NotFound($"Categoria de ID {id} não encontrada");
                }

                reader.Close();

                string deletarCategoria = @"DELETE FROM categoria WHERE id = @id";

                MySqlCommand comandoDeleta = new MySqlCommand(deletarCategoria, conexao);

                comandoDeleta.Parameters.AddWithValue("@id", id);

                int linhasAfetadas = comandoDeleta.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posível realizar a exclusão da categoria!");
                }

                return Ok($"Categoria de ID {id} excluída.");
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
