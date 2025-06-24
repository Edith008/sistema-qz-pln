using System.Collections.Generic;

namespace ExtranetQz.Areas.Vendedor.Models
{
    public class ClienteVendedor
    {
        public string cardCode { get; set; }
        public string cardName { get; set; }
        public string cardFName { get; set; }
        public string street { get; set; }
        public string address2 { get; set; }
        public string licTradNum { get; set; }
        public string phone1 { get; set; }
        public decimal creditLine { get; set; }
        public decimal debtLine { get; set; }
        public decimal balance { get; set; }
        public string u_BIREGION { get; set; }
        public int diasCredito { get; set; }
        public string u_CanalCliente { get; set; }
        public string e_Mail { get; set; }
        public string zonaVenta { get; set; }
        public string zonaTransporte { get; set; }
        public string tipoCredito { get; set; }
        public string ofcVta { get; set; }
    }

    public class ClientesVendedorResponse
    {
        public Archivo archivo { get; set; }  // Aquí reutilizas la clase Archivo
        public int totalRegistros { get; set; }
        public List<ClienteVendedor> resultados { get; set; }
    }
}
