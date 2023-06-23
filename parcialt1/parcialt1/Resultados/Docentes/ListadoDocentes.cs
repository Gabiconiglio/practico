using parcialt1.Models;

namespace parcialt1.Resultados.Docentes
{
    public class ListadoDocentes : ResultadoBase
    {
        public List<ItemDocente> ListDocentes { get; set; } = new List<ItemDocente>();
    }

    public class ItemDocente
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public int Edad { get; set; }
        public string Email { get; set; }
        public string Nivel { get; set; }

       
    }
}
