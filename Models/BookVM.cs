using System.ComponentModel.DataAnnotations.Schema;

namespace Book.WebAPI.Models
{
    [Table("tblBooks")]
    public class BookVM
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public string? Author { get; set; }
        public string? Title { get; set; }

    }
}
