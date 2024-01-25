using System.ComponentModel.DataAnnotations;

namespace Doorang.Areas.Admin.ViewModels
{
    public class UpdateCategoryVM
    {
        [Required]
        public string Name { get; set; }
    }
}
