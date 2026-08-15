using Microsoft.AspNetCore.Mvc;

namespace ASPNETITSTEP.Models.Home.Models
{
    public class HomeModelsFormModel
    {
        [FromForm(Name = "user-login")]
        public String UserLogin { get; set; } = null!;

        [FromForm(Name = "user-password")]
        public String UserPassword { get; set; } = null!;
    }
}

