using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace StatisticsApp.Controllers
{
    public class UploadController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }


    }
}