using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AUTOS_AJH.Models
{
    public class Venta
    {
        public string Id { get; set; }
        public string CodigoAuto { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal PrecioVenta { get; set; }
        public decimal DescuentoAplicado { get; set; }
        public decimal PrecioFinal { get; set; }
        public string Vendedor { get; set; }
        public string Cliente { get; set; }

        public Venta()
        {
            Id = Guid.NewGuid().ToString();
            FechaVenta = DateTime.Now;
        }
    }
}