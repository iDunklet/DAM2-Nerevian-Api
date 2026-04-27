namespace NerevianApi.Models.Documents
{
    public class Presupuesto
    {
        public string Id { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string Tipo { get; set; }
        public string Expira { get; set; } 
        public string Precio { get; set; }
        public string Incoterm { get; set; }
        public string Detalle { get; set; } 
        public string Estado { get; set; }
    }
}
