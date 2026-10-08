using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace ASPNETITSTEP.Models.Admin
{
    public class AdminAddGroupFormModel
    {
        [FromForm(Name = "group-parent")]
        public Guid? ParentId { get; set; } = null!;
        [Required(ErrorMessage = "Назва є обов'язковою")]
        [StringLength(100, MinimumLength = 2,
        ErrorMessage = "Назва повинна містити від 2 до 100 символів")]
        [RegularExpression(
        @"^[\p{L}\p{N} ]+$",
        ErrorMessage = "Назва не повинна містити спеціальних символів"
        )]
        [FromForm(Name = "group-name")]
        public String Name { get; set; } = null!;
        [StringLength(
        1000,
        MinimumLength = 5,
        ErrorMessage = "Опис повинен містити від 5 до 1000 символів"
        )]
        [FromForm(Name = "group-description")]

        public String Description { get; set; } = null!;
        [Required(ErrorMessage = "Slug є обов'язковим")]
        [RegularExpression(
         @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
        ErrorMessage = "Slug має бути URL-коректним: тільки малі латинські літери, цифри та дефіси"
        )]
        [FromForm(Name = "group-slug")]
        public String Slug { get; set; } = null!;

        [FromForm(Name = "group-img")]
        public IFormFile Image { get; set; } = null!;

        [FromForm(Name = "group-hidden")]
        public int IsHidden { get; set; } = 0;

        [FromForm(Name = "group-order")]
        public int Order { get; set; }
    }
}