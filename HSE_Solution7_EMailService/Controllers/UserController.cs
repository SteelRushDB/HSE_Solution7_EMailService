using HSE_Solution7_EMailService.Models;
using Microsoft.AspNetCore.Mvc;

namespace HSE_Solution7_EMailService.Controllers;

public class UserController : Controller
{
    private readonly JsonFileService _jsonFileService;

    public UserController()
    {
        _jsonFileService = new JsonFileService();
    }
    
    [HttpGet]
    public IActionResult Index()
    {
        var users = _jsonFileService.ReadUsersFromFile();
        return View(users);
    }
    
    
    [HttpGet]
    public IActionResult Details(string Email)
    {
        // Получение данных пользователя по email и передача их в представление
        var user = _jsonFileService.GetUserByEmail(Email);
        if (user == null)
        {
            return View("404");
        }
        return View(user);
    }
    
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    public IActionResult Create(User user)
    {
        if (_jsonFileService.GetUserByEmail(user.Email) != null)
        {
            // Добавляем ошибку в ModelState
            ModelState.AddModelError("Email", "Пользователь с таким email уже существует.");
        }
        
        if (ModelState.IsValid)
        {
            var users = _jsonFileService.ReadUsersFromFile();
            var createdUser = _jsonFileService.CreateUser(user.UserName, user.Email);
            users.Add(createdUser);
            _jsonFileService.WriteUsersToFile(users);
            return RedirectToAction(nameof(Index));
        }
        return View(user);
    }
}