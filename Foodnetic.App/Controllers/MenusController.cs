﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Foodnetic.App.Controllers
{
    public class MenusController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}