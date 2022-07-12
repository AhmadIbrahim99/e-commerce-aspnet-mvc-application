using eTickets.data.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace E_Commerce.eTickets.WebSite.Models
{
    public class NewMovieVM
    {
        public int Id { get; set; }

        [Display(Name = "Movie Name")]
        [Required(ErrorMessage = "Movie Name is required")]
        public string Name { get; set; }
        [Display(Name = "Description")]
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Display(Name = "Price")]
        [Required(ErrorMessage = "Price is required")]
        public double Price { get; set; }
        [Display(Name = "Image")]
        [Required(ErrorMessage = "Image is required")]
        public string ImageURL { get; set; }
        [Display(Name = "StartDate")]
        [Required(ErrorMessage = "StartDate is required")]
        public DateTime StartDate { get; set; }
        [Display(Name = "EndDate")]
        [Required(ErrorMessage = "EndDate is required")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Select Category")]
        [Required(ErrorMessage = "Movie Category is required")]
        public MovieCategory MovieCategory { get; set; }
        [Display(Name = "Select Actor(s)")]
        [Required(ErrorMessage = "Movie Actor(s) is required")]
        public List<int> ActorsIds { get; set; }
        [Display(Name = "Select Cinema")]
        [Required(ErrorMessage = "Cinema is required")]
        public int CinemaId { get; set; }
        [Display(Name = "Select Producer")]
        [Required(ErrorMessage = "Producer is required")]
        public int ProducerId { get; set; }



    }
}
