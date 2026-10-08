using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
namespace ASPNETITSTEP.Models.Admin
{
    public class AdminAddProductFormModel
    {
        [FromForm(Name = "product-group")]
        public Guid GroupId { get; set; }

        [FromForm(Name = "product-id")]
        public Guid? ProductId { get; set; }   // якщо додається версія наявного товару
        [Required(ErrorMessage = "Назва товару є обов'язковою")]
        [StringLength(
    100,
    MinimumLength = 2,
    ErrorMessage = "Назва товару повинна містити від 2 до 100 символів"
)]
        [RegularExpression(
    @"^[\p{L}\p{N} ]+$",
    ErrorMessage = "Назва товару не повинна містити спеціальних символів"
)]
        [FromForm(Name = "product-name")]
        public String Name { get; set; } = null!;
        [StringLength(
    1000,
    MinimumLength = 5,
    ErrorMessage = "Опис товару повинен містити від 5 до 1000 символів"
)]
        [FromForm(Name = "product-description")]
        public String? Description { get; set; } = null!;
        [RegularExpression(
    @"^[a-z0-9]+(?:-[a-z0-9]+)*$",
    ErrorMessage = "Slug має бути URL-коректним: тільки малі латинські літери, цифри та дефіси"
)]
        [FromForm(Name = "product-slug")]
        public String? Slug { get; set; } = null!;

        [FromForm(Name = "product-img")]
        public IFormFile? Image { get; set; } = null!;

        [FromForm(Name = "product-hidden")]
        public int IsHidden { get; set; } = 0;

        [FromForm(Name = "product-order")]
        public int Order { get; set; }
        [Range(-1, int.MaxValue,
    ErrorMessage = "Кількість має бути цілим позитивним числом або -1")]
        [RegularExpression(
    @"^-1|[1-9]\d*$",
    ErrorMessage = "Кількість має бути цілим позитивним числом або -1"
)]
        [FromForm(Name = "product-stock")]
        public int Stock { get; set; }
        [Range(0.01, double.MaxValue,
        ErrorMessage = "Ціна повинна бути більше за 0.01")]
        [FromForm(Name = "product-price")]
        public double Price { get; set; }

    }
}