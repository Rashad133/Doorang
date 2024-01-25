using System.ComponentModel.DataAnnotations;

namespace Doorang.Areas.Admin.ViewModels
{
    public class CreateCategoryVM
    {
        [Required]
        public string Name { get; set; }

    }
}
