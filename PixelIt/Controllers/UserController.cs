using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PixelIt.Models;
using PixelIt.DTOs.Account;
using PixelIt.ViewModel.User;
using PixelIt.Services;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PixelIt.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<UserController> _logger;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        // GET: User/Registration
        public IActionResult Registration()
        {
            var viewModel = new AddAccountViewModel
            {
                CreateUser = new CreateUserDto()
            };
            return View(viewModel);
        }

        // POST: User/RegistrationUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistrationUser(AddAccountViewModel model)
        {
            _logger.LogInformation("Ricevuta richiesta di registrazione");
            try
            {
                if (model == null || model.CreateUser == null)
                {
                    _logger.LogWarning("Modello nullo ricevuto");
                    return View("Registration", new AddAccountViewModel { CreateUser = new CreateUserDto() });
                }

                _logger.LogInformation("Email: {Email}, Nome: {Name}", model.CreateUser.Email, model.CreateUser.Name);

                string profilePicturePath = "uploads/profile images/default.png";

                if (model.CreateUser.ProfilePicture != null && model.CreateUser.ProfilePicture.Length > 0)
                {
                    //    var existingUser = await _userManager.FindByEmailAsync(model.Email);
                    //    if (ModelState.IsValid && existingUser == null)
                    //    {
                    //        var result = (IActionResult)await _userService.CreateUserAsync(model, password, pp, vi1, vi2, roleName);
                    //        await _signInManager.SignInAsync((ApplicationUser)result, isPersistent: false);

                    //        return RedirectToAction("Index", "Home");
                    //    }
                    //    else
                    //    {
                    //        ModelState.AddModelError("CreateUser.Email", "Questa email è già registrata");
                    //        return View("Registration", model);
                    //    }

                    //}

                    // Gestisci l'immagine del profilo
                    try
                    {
                        var fileName = Path.GetFileName(model.CreateUser.ProfilePicture.FileName);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile images");

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var filePath = Path.Combine(directory, uniqueFileName);

                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.CreateUser.ProfilePicture.CopyToAsync(stream);
                        }

                        profilePicturePath = $"uploads/profile images/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Errore durante il salvataggio dell'immagine del profilo");
                    }
                }

                var Verification1 = "";
                var Verification2 = "";

                if (model.CreateUser.VerificationImage1 != null && model.CreateUser.VerificationImage1.Length > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(model.CreateUser.VerificationImage1.FileName);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images collection");

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var filePath = Path.Combine(directory, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.CreateUser.VerificationImage1.CopyToAsync(stream);
                        }
                        Verification1 = $"uploads/images collection/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Errore durante il salvataggio dell'immagine di verifica 1");
                    }
                }

                if (model.CreateUser.VerificationImage2 != null && model.CreateUser.VerificationImage2.Length > 0)
                {
                    try
                    {
                        var fileName = Path.GetFileName(model.CreateUser.VerificationImage2.FileName);
                        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images collection");

                        if (!Directory.Exists(directory))
                        {
                            Directory.CreateDirectory(directory);
                        }

                        var filePath = Path.Combine(directory, uniqueFileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.CreateUser.VerificationImage2.CopyToAsync(stream);
                        }
                        Verification2 = $"uploads/images collection/{uniqueFileName}";
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Errore durante il salvataggio dell'immagine di verifica 2");
                    }
                }
                var existingUser = await _userManager.FindByEmailAsync(model.CreateUser.Email);
                if (ModelState.IsValid && existingUser == null)
                {
                    _logger.LogInformation("Creazione utente con email: {Email}", model.CreateUser.Email);
                }
                else
                {
                    ModelState.AddModelError("CreateUser.Email", "Questa email è già registrata");
                    return View("Registration", model);
                }
                var newUser = new ApplicationUser
                {
                    UserName = model.CreateUser.Email,
                    Email = model.CreateUser.Email,
                    Name = model.CreateUser.Name,
                    Surname = model.CreateUser.Surname,
                    Nickname = model.CreateUser.Nickname,
                    ProfilePicture = profilePicturePath,
                    ProfileDescription = model.CreateUser.ProfileDescription,
                    DateOfBirth = model.CreateUser.DateOfBirth,
                    VerificationImage1 = Verification1,
                    VerificationImage2 = Verification2,
                    DateOfRegistration = DateTime.UtcNow,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser, model.CreateUser.Password);

                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        _logger.LogWarning("Errore creazione utente: {Code} - {Description}", error.Code, error.Description);
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    return View("Registration", model);
                }

                var userCreated = await _userManager.FindByEmailAsync(model.CreateUser.Email);
                if (userCreated != null)
                {
                    await _userManager.AddToRoleAsync(userCreated, "User");
                    _logger.LogInformation("Utente creato con ID: {UserId}", userCreated.Id);
                }
                else
                {
                    _logger.LogWarning("Utente non trovato dopo la creazione");
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione dell'utente");
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la registrazione: " + ex.Message);
                return View("Registration", model);
            }
        }

        // GET: User/Login
        public IActionResult Login()
        {
            var viewModel = new LogInViewModel
            {
                LogInUser = new LogInDto()
            };
            return View(viewModel);
        }

        // POST: User/LoginUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginUser(LogInViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.LogInUser.Email, model.LogInUser.Password, isPersistent: false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("Utente {Email} ha effettuato il login con successo", model.LogInUser.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Credenziali non valide");
                }
            }
            return View("Login", model);
        }
    }
}