using ASPNETITSTEP.Data;
using ASPNETITSTEP.Data.Entities;
using ASPNETITSTEP.Services.Kdf;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
namespace ASPNETITSTEP.Controllers
{
    public class UserController(DataContext dataContext, IKdfService kdfService) : Controller
    {
        private readonly DataContext _dataContext = dataContext;
        private readonly IKdfService _kdfService = kdfService;
        public IActionResult BasicAuth()
        {
            UserAccess? usserAccess;
            try
            {
                usserAccess = AuthenticateUser();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            if (usserAccess == null)
            {
                return Unauthorized("Credentials rejected: check login and password");

            }
            HttpContext.Session.SetString("UserAccessId", usserAccess.Id.ToString());
            return Ok();
        }

        public IActionResult BasicAuthJwt()
        {
            UserAccess? usserAccess;
            try
            {
                usserAccess = AuthenticateUser();
            }
            catch (Exception ex)
            {
                return Unauthorized(ex.Message);
            }
            if (usserAccess == null)
            {
                return Unauthorized("Credentials rejected: check login and password");

            }
            var header = new
            {
                alg = "HS256",
                typ = "JWT"
            };
            long time = (DateTime.Now.Ticks - DateTime.UnixEpoch.Ticks) / 10000000;
            var payload = new
            {
                sub = usserAccess.Login,
                iat = time,
                exp = time + 100000,
                name = usserAccess.UserData.FullName,
                email = usserAccess.UserData.Email
            };
            String body = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(
     Encoding.UTF8.GetBytes(
         JsonSerializer.Serialize(header)))
     + "." +
     Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(
     Encoding.UTF8.GetBytes(
         JsonSerializer.Serialize(payload)));
            String signature = Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(System.Security.Cryptography.HMACSHA256.HashData(
                Encoding.UTF8.GetBytes(body + "." + Microsoft.AspNetCore.Authentication.Base64UrlTextEncoder.Encode(Encoding.UTF8.GetBytes(JsonSerializer.Serialize(header)))),
                Encoding.UTF8.GetBytes("secret")));
            return Ok(body + "." + signature);
        }
        private UserAccess? AuthenticateUser()
        {
            String authHeader = HttpContext.Request.Headers.Authorization.ToString();
            if (authHeader == String.Empty)
            {
                throw new Exception("Missing Authorization header");
            }
            String scheme = "Basic ";
            if (!authHeader.StartsWith(scheme))
            {
                throw new Exception("Authorization scheme is not Basic");
            }
            String credentials = authHeader[scheme.Length..];
            byte[] rawData;
            try
            {
                rawData = Convert.FromBase64String(credentials);
            }
            catch
            {
                throw new Exception(
                    "Authorization credentials must be valid Base64::section 4");
            }
            String userPass;
            try
            {
                userPass = Encoding.UTF8.GetString(rawData);
            }
            catch
            {
                throw new Exception(
                    "User-pass must be valid UTF8 string");
            }
            String[] parts = userPass.Split(':', 2);
            if (parts.Length != 2)
            {
                throw new Exception(
                    "User-pass must be concatenated by ':'");
            }
            String login = parts[0];
            String password = parts[1];
            if (_dataContext
                .UserAccesses
                .Include(ua => ua.UserData)
                .Include(ua => ua.UserRole)
                .AsNoTracking()
                .FirstOrDefault(ua => ua.Login == login)
                is UserAccess userAccess)
            {
                String dk = _kdfService.Dk(password, userAccess.Salt);
                if (dk == userAccess.Dk)
                {
                    return userAccess;
                }
            }
            return null;
        }
    }

}