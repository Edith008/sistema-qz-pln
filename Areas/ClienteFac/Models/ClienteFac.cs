using System;
using System.Collections.Generic;

namespace ExtranetQz.Areas.ClienteFac.Models
{
    public class ClienteFacResponse
    {
        public Archivo archivo { get; set; }
        public int facturasTotales { get; set; }
        public List<Factura> facturas { get; set; }
    }

    public class Archivo
    {
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
    }

    public class Factura
    {
        public int docEntry { get; set; }
        public int docNum { get; set; }
        public int u_LB_NumeroFactura { get; set; }
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public string pymntGroup { get; set; }
        public decimal docTotal { get; set; }
        public decimal paidToDate { get; set; }
        public decimal balance { get; set; }
        public DateTime taxDate { get; set; }
        public DateTime docDueDate { get; set; }
        public string idVendedor { get; set; }
    }

    public class ClienteFacturaDetalleResponse
    {
        public ArchivoPDF archivo { get; set; }
    }

    public class ArchivoPDF
    {
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
    }
}
