
using System.ComponentModel.DataAnnotations.Schema;

namespace Bank.Model
{
    [Table("T2Parametro")]
    public class Parametro
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }

    }
}
