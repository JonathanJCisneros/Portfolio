using System.ComponentModel.DataAnnotations;

namespace PortfolioV2.Web.Models
{
    public abstract class BaseModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}
