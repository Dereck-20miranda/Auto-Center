using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClasesFerreteria
{
    public class Mecanico
    {
        public int cedula { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
        public int telefono { get; set; }
        public int edad { get; set; }
        public string direccion { get; set; }
        public string cargo { get; set; }
    }
}
