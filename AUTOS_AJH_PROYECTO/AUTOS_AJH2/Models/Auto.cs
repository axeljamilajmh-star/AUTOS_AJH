using System;
using System.Collections.Generic;

namespace AUTOS_AJH.Models
{
    public class Auto
    {
        public string Codigo { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Ano { get; set; }
        public string Color { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public decimal Cilindraje { get; set; }
        public string Transmision { get; set; }
        public string Combustible { get; set; }
        public int Puertas { get; set; }
        public int Pasajeros { get; set; }
        public string Descripcion { get; set; }
        public bool Disponible { get; set; } = true;
        public List<string> Fotos { get; set; } = new List<string>();

        public bool EsValido()
        {
            return !string.IsNullOrEmpty(Codigo) &&
                   !string.IsNullOrEmpty(Marca) &&
                   !string.IsNullOrEmpty(Modelo) &&
                   Ano > 1900 &&
                   Precio > 0;
        }

        public override string ToString()
        {
            return $"{Marca} {Modelo} {Ano} - ${Precio:N2}";
        }
    }
}