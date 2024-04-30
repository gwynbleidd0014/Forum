// Copyright (C) TBC Bank. All Rights Reserved.

using Microsoft.AspNetCore.Mvc;

namespace Forum.Web.Controllers;

public class HomeController : Controller
{
    public HomeController()
    {
    }

    [HttpGet]
    public IActionResult Error()
    {
        var errorString = HttpContext.Session.GetString("ErrorMessage");
        ViewBag.Error = errorString;
        return View();
    }
}
