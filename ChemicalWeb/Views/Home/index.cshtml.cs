using ChemicalWeb.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ChemicalWeb.Views.Home; 

public class ModelIndex : PageModel {
    public void OnGet() {
    }
    
    public Task<IActionResult> CheckUser(string? login, string? password) {
        var isUser = DataBaseWorker.CheckUserData(login, password);
        
        return Task.FromResult<IActionResult>(RedirectToAction(isUser ? "User" : "ModelIndex"));
    }
    
    
}