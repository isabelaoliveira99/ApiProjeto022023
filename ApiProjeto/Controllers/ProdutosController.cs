using ApiProjeto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using System.Security.Cryptography;

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
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                
                conexao.Open();

                string sql = @"SELECT * FROM produto";

                MySqlCommand comando = new MySqlCommand(sql, conexao);
                var reader = comando.ExecuteReader();

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
            finally
            {
                conexao.Close();
            }

        }

        [HttpPost]
        public IActionResult NovoProduto([FromBody] Produto produto)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"INSERT INTO produto(nome_produto, descricao_produto, valor, id_categoria) VALUES(@nome, @descricao, @valor, @idCategoria)";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", produto.nome_produto);
                comando.Parameters.AddWithValue("@descricao", produto.descricao_produto);
                comando.Parameters.AddWithValue("@valor", produto.valor);
                comando.Parameters.AddWithValue("@idCategoria", produto.id_categoria);

                int linhasAfetadas = comando.ExecuteNonQuery();

                if(linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posssível inserir o produto!");
                }

                return Created("", produto.nome_produto);
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
        public IActionResult AlteraProduto(int id, [FromBody] Produto produto)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                if (id != produto.id)
                {
                    return StatusCode(StatusCodes.Status417ExpectationFailed, "Id da URL e do Body não correspondem");
                }

                conexao.Open();

                string sqlConsulta = @"SELECT * FROM produto WHERE id = @id";

                MySqlCommand produtoExistente = new MySqlCommand(sqlConsulta, conexao);

                produtoExistente.Parameters.AddWithValue("@id", id);

                var readerproduto = produtoExistente.ExecuteReader();

                if (!readerproduto.Read())
                {
                    return NotFound($"Produto de ID {id} não encontrado");
                }

                readerproduto.Close();

                string updateProduto = @"UPDATE produto SET nome_produto = @nome, descricao_produto = @descricao, valor = @valor, id_categoria = @idCategoria WHERE id = @id";

                MySqlCommand updateComando = new MySqlCommand(updateProduto, conexao);

                updateComando.Parameters.AddWithValue("@nome", produto.nome_produto);
                updateComando.Parameters.AddWithValue("@descricao", produto.descricao_produto);
                updateComando.Parameters.AddWithValue("@valor", produto.valor);
                updateComando.Parameters.AddWithValue("@idCategoria", produto.valor);
                updateComando.Parameters.AddWithValue("@id", produto.id);

                int linhasAfetadas=  updateComando.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posível realizar a alteração do produto!");
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
    }
}
