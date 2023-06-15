using ChemicalWeb.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ChemicalWeb.Controllers;

public class UserController : Controller {
    public IActionResult User() {
        return View();
    }
    
    public ViewResult GetMaterialsInfo(string material) {
        var materialsInfo = DataBaseWorker.GetMaterialsInfoForLabel(material);
        ViewBag.density = materialsInfo[0].Value;
        ViewBag.specificHeat = materialsInfo[1].Value;
        ViewBag.meltingTemperature = materialsInfo[2].Value;
        ViewBag.currentMaterial = material;
        return View("user");
    }
}