using System.Collections.Generic;
using System;

namespace ExtranetQz.Areas.Calculadora.Models
{
    public class DetalleVentaResponse
    {
        public int totalRegistros { get; set; }
        public List<Resultado> resultados { get; set; }
    }

    public class Resultado
    {
        public string codcliente { get; set; }
        public string nombrecliente { get; set; }
        public string nombrecomercial { get; set; }
        public string nrofactura { get; set; }
        public string cuf { get; set; }
        public DateTime? fechafactura { get; set; }
        public string codanterior { get; set; }
        public string descproducto { get; set; }
        public string cantidad { get; set; }
        public string precio { get; set; }
        public string subtotal { get; set; }
        public string vencimiento { get; set; }
        public string centro { get; set; }
        public string lote { get; set; }
        public string oficinaventa { get; set; }
    }
}

