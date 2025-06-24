using System;
using System.Collections.Generic;

namespace ExtranetQz.Areas.Kardex.Models
{
    public class KardexResponse
    {
        public int totalRegistros { get; set; }
        public List<Movimiento> resultados { get; set; }
    }

    public class Movimiento
    {
        public int? clase_movimiento { get; set; }
        public int? codigo_material { get; set; }
        public string descripcion { get; set; }
        public int? cantidad { get; set; }
        public string lote { get; set; }
        public DateTime? fecha_entrada { get; set; }
        public decimal? costo { get; set; }
        public decimal? importe { get; set; }
    }

}
