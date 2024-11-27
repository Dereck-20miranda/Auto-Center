using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ClasesFerreteria;
using System.Xml.Linq;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace APIFerreteria71.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : Controller
    {
        [Route("[action]")]
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Producto>> ProductoTr(Producto pr)
        {

            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];

            XDocument xmlParam = Shared.DBXmlMethods.GetXml(pr);

            DataSet dsResult = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosDB.consultaProducto, cadenaConexion, pr.Transaccion, xmlParam.ToString());

            List<Producto> listProducts = new List<Producto>();
            if(dsResult.Tables.Count> 0)
            {
                try
                {
                    foreach(DataRow row in dsResult.Tables[0].Rows) 
                    {
                        Producto objResponse = new Producto
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Nombre = row["nombre"].ToString(),
                            Precio_Compra = Convert.ToDouble(row["precio_compra"]),
                            Precio_Venta = Convert.ToDouble(row["precio_venta"]),
                            Categoria_Producto = new Categoria_Producto()
                            {
                                Id= Convert.ToInt32(row["id_cp"]),
                                Nombre_Categoria = row["nombre_categoria"].ToString()
                            }


                        };
                        listProducts.Add(objResponse);
                    }
                }
                catch(Exception ex) 
                {
                    Console.Write(ex.ToString());
                }
            }


            return Ok(listProducts);
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<MensajeResultado>> SetProductoTr(Producto pr)
        {
            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];

            XDocument xmlParam = Shared.DBXmlMethods.GetXml(pr);

            DataSet dsResult = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosDB.SetProducto, cadenaConexion, pr.Transaccion, xmlParam.ToString());
            List<MensajeResultado> listMensajeResultado = new List<MensajeResultado>();
            if (dsResult.Tables.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        MensajeResultado objResponse = new MensajeResultado
                        {
                            Respuesta = row["respuesta"].ToString(),
                            Leyenda = row["leyenda"].ToString()
                        };
                       
                        listMensajeResultado.Add(objResponse);
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            return Ok(listMensajeResultado);
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult<Producto>> GetProductoTr_Id(int id, string transaccion)
        {
            Producto producto_tmp = new Producto();
            producto_tmp.Id = id;
            producto_tmp.Transaccion = transaccion;


            var cadenaConexion = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build()
                .GetSection("ConnectionStrings")["Conexion"];

            XDocument xmlParam = Shared.DBXmlMethods.GetXml(producto_tmp);

            DataSet dsResult = await Shared.DBXmlMethods.EjecutaBase(Shared.ProcedimientosDB.consultaProducto, cadenaConexion, producto_tmp.Transaccion, xmlParam.ToString());

            List<Producto> listProducts = new List<Producto>();
            if (dsResult.Tables.Count > 0)
            {
                try
                {
                    foreach (DataRow row in dsResult.Tables[0].Rows)
                    {
                        Producto objResponse = new Producto
                        {
                            Id = Convert.ToInt32(row["id"]),
                            Nombre = row["nombre"].ToString(),
                            Precio_Compra = Convert.ToDouble(row["precio_compra"]),
                            Precio_Venta = Convert.ToDouble(row["precio_venta"]),
                            Categoria_Producto = new Categoria_Producto()
                            {
                                Id = Convert.ToInt32(row["id_cp"]),
                                Nombre_Categoria = row["nombre_categoria"].ToString()
                            }


                        };
                        listProducts.Add(objResponse);
                    }
                }
                catch (Exception ex)
                {
                    Console.Write(ex.ToString());
                }
            }


            return Ok(listProducts);

        }
        
    }
}
