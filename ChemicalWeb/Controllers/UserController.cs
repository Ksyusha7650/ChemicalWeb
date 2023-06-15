using ChemicalWeb.DAL;
using Microsoft.AspNetCore.Mvc;

namespace ChemicalWeb.Controllers;

public class UserController : Controller {
    public IActionResult User() {
        return View();
    }
    
    public ViewResult GetMaterialsInfo(string material) {
        var materialsInfo = DataBaseServer.GetMaterialsInfoForLabel(material);
        if (materialsInfo.Count == 3)
        {
            ViewBag.density = materialsInfo[0].Value;
            ViewBag.specificHeat = materialsInfo[1].Value;
            ViewBag.meltingTemperature = materialsInfo[2].Value;
        }
        ViewBag.currentMaterial = material;
        var coefficientsInfo = DataBaseServer.GetCoefficientsInfoForLabel(material);
        if (coefficientsInfo.Count == 5)
        {
            ViewBag.c1 = coefficientsInfo[0].Value;
            ViewBag.c2 = coefficientsInfo[1].Value;
            ViewBag.c3 = coefficientsInfo[2].Value;
            ViewBag.c4 = coefficientsInfo[3].Value;
            ViewBag.c5 = coefficientsInfo[4].Value;
        }
        return View("user");
    }

    public ContentResult ExportDataBase()
    {
        var path = $"{Environment.CurrentDirectory}\\dump.sql";
        return DataBaseWorker.Backup(path) ? Content($"Успех! Файл: {path}") : Content("Произошла ошибка!");
    }
       
    
    public ContentResult ImportDataBase() {
        var path = $"{Environment.CurrentDirectory}\\dump.sql";
        return DataBaseWorker.Restore(path) ? Content("Успех!") : Content("Произошла ошибка!");
    }
    
    public IActionResult GoBack() => RedirectToAction("Index", "Home");

    }