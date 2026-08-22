using Microsoft.AspNetCore.Mvc;
namespace ASPNETITSTEP.Controllers
{
    public class UserController : Controller
    {
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
            return Json(authHeader);
        }
    }

}