using ApiProjeto.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace ApiProjeto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        private readonly string MySqlConnection = @"Server=20.226.185.146;Port=3306;Database=bd_projeto;Uid=root;Pwd=1234;";

        [HttpGet]
        public IActionResult RetornaClientes()
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"SELECT * FROM cliente";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                var reader = comando.ExecuteReader();

                List<Cliente> listaCliente = new List<Cliente>();

                while (reader.Read())
                {
                    Cliente item = new Cliente();
                    item.id = int.Parse(reader["id"].ToString());
                    item.nome_cliente = reader["nome_cliente"].ToString();
                    item.cpf = reader["cpf"].ToString();
                    item.endereco = reader["endereco"].ToString();
                    item.telefone = reader["telefone"].ToString();
                    listaCliente.Add(item);
                }

                return Ok(listaCliente);
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
        public ActionResult NovoCliente([FromBody] Cliente cliente)
        {
            MySqlConnection conexao = new MySqlConnection(MySqlConnection);

            try
            {
                conexao.Open();

                string sql = @"INSERT INTO cliente(nome_cliente, cpf, endereco, telefone) 
                                 VALUES(@nome, @cpf, @endereco, @telefone)";

                MySqlCommand comando = new MySqlCommand(sql, conexao);

                comando.Parameters.AddWithValue("@nome", cliente.nome_cliente);
                comando.Parameters.AddWithValue("@cpf", cliente.cpf);
                comando.Parameters.AddWithValue("@endereco", cliente.endereco);
                comando.Parameters.AddWithValue("@telefone", cliente.telefone);

                int linhasAfetadas = comando.ExecuteNonQuery();

                if (linhasAfetadas == 0)
                {
                    return UnprocessableEntity("Não foi posssível inserir o cliente!");
                }

                return Created("", cliente.nome_cliente);

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
