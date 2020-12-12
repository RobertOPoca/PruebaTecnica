using System;
using System.Collections.Generic;

#nullable disable

namespace PruebaTecnica.Models
{
    public partial class Area
    {
        public Area()
        {
            Empleados = new HashSet<Empleado>();
        }

        public int IdArea { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }
    }
}
