using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClasesFerreteria;
using System.Data;
using System.Xml.Linq;
using Newtonsoft.Json;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace APIFerreteria71.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : Controller
    {
        private readonly IConfiguration Configuration;

        public UsuarioController(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<Usuario>> GetLogin([FromBody] Usuario usuarios)
        {
            try
            {
                var cadenaConexion = new ConfigurationBuilder()
               .AddJsonFile("appsettings.json")
               .Build()
               .GetSection("ConnectionStrings")["Conexion"];

                XDocument xmlParam = Shared.DBXmlMethods.GetXml(usuarios);

                DataSet dsResult = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosDB.getUsuario, cadenaConexion, usuarios.Transaccion, xmlParam.ToString());
                List<Usuario> listData = new List<Usuario>();
                if (dsResult.Tables.Count > 0)
                {
                    try
                    {
                        if (dsResult.Tables[0].Rows.Count > 0)
                        {
                            Usuario usertem = new Usuario();
                            usertem.Id = Convert.ToInt32(dsResult.Tables[0].Rows[0]["Id"]);
                            usertem.Cedula = dsResult.Tables[0].Rows[0]["Cedula"].ToString();
                            return Ok(JsonConvert.SerializeObject(CrearToken(usertem)));

                            
                        }
                        else
                        {
                            MensajeResultado objresponse = new MensajeResultado();
                            objresponse.Leyenda = "Error en las credenciales de acceso";
                            objresponse.Respuesta = "Error";
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write("Error "+ ex.Message);
                    }
                }
                return Ok();
            }
            catch(Exception ex)
            {
                Console.Write("error: " + ex.Message);
                return StatusCode(500);
            }
        }
        private string CrearToken(Usuario usuario)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Name, usuario.Cedula),
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(Configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            var tokenhandler = new JwtSecurityTokenHandler();
            var token = tokenhandler.CreateToken(tokenDescriptor);
            return tokenhandler.WriteToken(token);
        }

    }
}
