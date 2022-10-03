using System.ComponentModel.DataAnnotations;

namespace Bazar360.Areas.Admin.ViewModels
{
    public class RoleUserVm
    {
        [Required]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string RoleId { get; set; }
    }
}
