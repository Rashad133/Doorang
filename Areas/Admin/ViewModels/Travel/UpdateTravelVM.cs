using Doorang.Models;
using System.ComponentModel.DataAnnotations;

namespace Doorang.Areas.Admin.ViewModels
{
    public class UpdateTravelVM
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        public IFormFile? Photo { get; set; }
        public string Image { get; set; }

        public int CategoryId { get; set; }
        public List<Category>? Categories { get; set; }
    }
}
