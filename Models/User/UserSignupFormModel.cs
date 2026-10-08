using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
namespace ASPNETITSTEP.Models.User
{
    public class UserSignupFormModel
    {
        [JsonPropertyName("name")]
        public String FullName { get; set; } = null!;

        [JsonPropertyName("login")]
        public String Login { get; set; } = null!;

        [JsonPropertyName("email")]
        public String Email { get; set; } = null!;
        [Phone]
        [RegularExpression(@"^0\d{9}$", ErrorMessage = "Телефон має складатися з 10 цифр і починатися з 0")]
        [JsonPropertyName("phone")]
        public String? Phone { get; set; }
        [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$",
        ErrorMessage = "Пароль має містити мінімум 8 символів, велику і малу літери, цифру та спеціальний символ"
        )]
        [JsonPropertyName("password")]
        public String Password { get; set; } = null!;

        [JsonPropertyName("repeat")]
        public String Repeat { get; set; } = null!;

        [JsonPropertyName("isAgree")]
        public bool IsAgree { get; set; }

    }
}