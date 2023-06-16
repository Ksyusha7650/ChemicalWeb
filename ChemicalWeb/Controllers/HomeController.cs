using ChemicalWeb.DAL;
using ChemicalWeb.Views.Home;
using Microsoft.AspNetCore.Mvc;

namespace ChemicalWeb.Controllers;

public class HomeController : Controller {
    
    [HttpGet]
    public IActionResult Index() {
        var model = new ModelIndex();
        return View(model);
    }
    
    [HttpPost]
    public IActionResult Index(string login, string password) {
        var isUser = DataBaseWorker.CheckUserData(login, password);
        return isUser ? login == "admin" ? 
            RedirectToAction("Admin", "Admin") : RedirectToAction("User", "User") 
            : RedirectToAction("Index");
    }
}