using Microsoft.AspNetCore.Mvc;

namespace ASPNETITSTEP.Models.Home.Models
{
    public class HomeModelsFormModel
    {
        [FromForm(Name = "user-login")]
        public String UserLogin { get; set; } = null!;

        [FromForm(Name = "user-password")]
        public String UserPassword { get; set; } = null!;

        [FromForm(Name = "user-agree")]
        public bool UserAgree { get; set; }

        [FromForm(Name = "user-gender")]
        public String UserGender { get; set; } = null!;

        [FromForm(Name = "user-birthdate")]
        public DateTime UserBirthdate { get; set; }

        [FromForm(Name = "user-color")]
        public string UserColor { get; set; } = null!;

        [FromForm(Name = "user-age")]
        public int UserAge { get; set; }
    }
}

