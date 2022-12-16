using Microsoft.AspNetCore.Mvc;

namespace CareerConnect.Controllers
{
    public class BaseController : Controller
    {
        protected void AddMessage(string? message)
        {
            message ??= "Undefined error";
            ViewData["Message"] = message;
        }
    }
}
