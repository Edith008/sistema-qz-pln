using System.Collections.Generic;

namespace ExtranetQz.Areas.Vendedor.Models
{
    public class ProductosVendedorResponse
    {
        public Archivo archivo { get; set; }
        public int totalRegistros { get; set; }
        public List<ProductoVendedor> resultados { get; set; }
    }

    public class Archivo
    {
        public string rutaArchivo { get; set; }
        public string nombreArchivo { get; set; }
    }

    public class ProductoVendedor
    {
        public string itemCode { get; set; }
        public string itemName { get; set; }
        public string frgnName { get; set; }
        public int numInSale { get; set; }
        public string vatLiable { get; set; }
        public string salUnitMsr { get; set; }
        public string itmsGrpCod { get; set; }
        public string undNegocio { get; set; }
        public int bonus { get; set; }
        public string status { get; set; }
    }
}
