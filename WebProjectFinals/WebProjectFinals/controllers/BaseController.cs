using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebProjectFinals.controllers
{
    public class BaseController : Controller
    {
        // GET: /<controller>/
        protected void AddMessage(string? message)
        {
            message ??= "Undefined error";
            ViewData["Message"] = message;
        }
    }
}

