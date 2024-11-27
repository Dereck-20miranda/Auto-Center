using System.ComponentModel.DataAnnotations;

namespace ClasesFerreteria
{
    public class Mantenimiento
    {
        public string matricula { get; set; }
        public string modelo { get; set; }
        public string carga { get; set; }
        public string tipo_man { get; set; }
        public string estado { get; set; }

    }
}