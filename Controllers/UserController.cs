using ASPNETITSTEP.Data;
using ASPNETITSTEP.Data.Entities;
using ASPNETITSTEP.Services.Kdf;
using Microsoft.AspNetCore.Mvc;
using System.Text;
namespace ASPNETITSTEP.Controllers
{
    public class UserController(DataContext dataContext, IKdfService kdfService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;
        public IActionResult BasicAuth()
        {
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if(authHeader == String.Empty)
            {
                return Unauthorized("Missing Authorization header");
            }
            String scheme = "Basic ";
            if(!authHeader.StartsWith(scheme))
            {
                return Unauthorized("Authorization scheme is not Basic");
            }
            String credentials = authHeader[scheme.Length..];
            byte[] rawData;
            try
            {
                rawData = Convert.FromBase64String(credentials);
            }
            catch
            {
                return Unauthorized(
                    "Authorization credentials must be valid Base64::section 4");
            }
            String userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(rawData);
            }
            catch
            {
                return Unauthorized(
                    "User-pass must be valid UTF8 string");
            }
            String[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                return Unauthorized(
                    "User-pass must be concatenated by ':'");
            }
            String login = parts[0];
            String password = parts[1];
            if (_dataContext
                .UserAccesses
                .FirstOrDefault(ua => ua.Login == login)
                is UserAccess userAccess)
            {
                String dk = _kdfService.Dk(password, userAccess.Salt);
                if (dk == userAccess.Dk)
                {
                        HttpContext.Session.SetString(
                        "userAccessId",
                        userAccess.Id.ToString()
                    );
                    // відповідь може бути порожньою
                    return Ok();
                }
            }
            return Json(authHeader);
        }
    }

}