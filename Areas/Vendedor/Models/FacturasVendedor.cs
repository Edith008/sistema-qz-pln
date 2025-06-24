using System;
using System.Collections.Generic;

namespace ExtranetQz.Areas.Vendedor.Models
{
    public class FacturaVendedor
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
    }

    public class FacturasVendedorResponse
    {
        public Archivo archivo { get; set; }  // Reutilizas tu clase Archivo existente
        public int totalRegistros { get; set; }
        public List<FacturaVendedor> resultados { get; set; }
    }

    public class VClienteFacturaDetalleResponse
    {
        public ArchivoPDF archivo { get; set; }
    }

    public class ArchivoPDF
    {
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
    }
}
