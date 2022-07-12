using eTickets.Data.Base;
using System.ComponentModel.DataAnnotations;

namespace E_Commerce.eTickets.WebSite.Models
{
    public class Cinema: IEntityBase
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Logo")]
        [Required(ErrorMessage = "Cinema Logo Is Required")]
        public string Logo { get; set; }
        [StringLength(50,MinimumLength =3,ErrorMessage = "Name Between 3 And 50")]
        [Display(Name = "Name")]
        [Required(ErrorMessage = "Cinema Name Is Required")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Cinema Description Is Required")]
        public string Description { get; set; }
    }
}
