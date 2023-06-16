using ChemicalWeb.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ChemicalWeb.Controllers;

public class AdminController : Controller {

    public IActionResult Admin() {
        return View();
    }

    [HttpPost]
    public IActionResult AddUser(string login, string password)
    {
        DataBaseWorker.AddUser(login, password);
        return View("admin");
    }
    
    [HttpPost]
    public IActionResult ChangePassword(string login, string password)
    {
        DataBaseWorker.ChangePassword(login, password);
        return View("admin");
    }
    
    [HttpPost]
    public IActionResult DeleteUser(string login)
    {
        DataBaseWorker.DeleteUser(login);
        return View("admin");
    }
    
    [HttpPost]
    public IActionResult AddMaterial(string material)
    {
        DataBaseWorker.AddMaterial(material);
        return View("admin");
    }
    
    [HttpPost]
    public IActionResult ChangeMaterial(string login, string password)
    {
        DataBaseWorker.ChangePassword(login, password);
        return View("admin");
    }
    
    [HttpPost]
    public IActionResult DeleteMaterial(string material)
    {
        DataBaseWorker.DeleteMaterial(material);
        return View("admin");
    }
    
    public IActionResult GoBack() => RedirectToAction("Index", "Home");

}