using Microsoft.AspNetCore.Mvc;
using MyProject.Domain.Dtos;
using MyProject.Services.FluentValidation;
using MyProject.Services.Services.AccountServices;

namespace MyProject.Controllers;

public class AccountController : Controller
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Get: Registor/Create
    [HttpGet]
    public IActionResult Registor()
    {
        return View();
    }
    // Post: Registor/Create
    [HttpPost]

    public async Task<IActionResult> Registor([Bind("Name,Email,ConfirmPassword,Password,Role")] Registor registorDto)
    {
        var validator = await new RegistorValidator().ValidateAsync(registorDto);
        if (!validator.IsValid)
        {
            foreach (var error in validator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(registorDto);
        }

        if (ModelState.IsValid)
        {
            var a= await _accountService.Registor(registorDto);
            return RedirectToAction("Index", "Product");
        }
        return View(registorDto);
    }

    [HttpGet]

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]

    // Post: Login/Create

    public async Task<IActionResult> Login([Bind("Password,Email")] Login loginDto)
    {
        var validator = await new LoginValidator().ValidateAsync(loginDto);
        if (!validator.IsValid)
        {
            foreach (var error in validator.Errors)
            {
                ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
            }
            return View(loginDto);

        }

        if (ModelState.IsValid)
        {
            await _accountService.Login(loginDto);
            return RedirectToAction("Index", "Product");
        }
        return View(loginDto);
    }



}
