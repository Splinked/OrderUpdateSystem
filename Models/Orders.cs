using System.ComponentModel.DataAnnotations;

namespace OrderUpdateSystem.Models
{
    public class Orders
    {
        public int Id { get; set; }
        [DataType(DataType.Date)]
        public DateTime OrderUpdateDate { get; set; }
        public string? Note { get; set; }
        public string? Status { get; set; }
        public int OperatorId { get; set; }

    }
}
