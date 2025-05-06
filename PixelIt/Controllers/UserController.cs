//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using PixelIt.DTOs.Account;
//using PixelIt.DTOs.ImageCollection;
//using PixelIt.Models;
//using PixelIt.Services;
//using System.IO;
//using System.Threading.Tasks;

//namespace PixelIt.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class UserController : ControllerBase
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly SignInManager<ApplicationUser> _signInManager;
//        private readonly ILogger<UserController> _logger;
//        private readonly ImageCollectionService _imageCollectionService;

//        public UserController(
//            UserManager<ApplicationUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            SignInManager<ApplicationUser> signInManager,
//            ILogger<UserController> logger,
//            ImageCollectionService imageCollectionService)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _signInManager = signInManager;
//            _logger = logger;
//            _imageCollectionService = imageCollectionService;
//        }

//        [HttpPost("registration")]
//        public async Task<IActionResult> Registration([FromForm] CreateUserDto createUser)
//        {
//            try
//            {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }

//                // Gestisci l'immagine del profilo
//                string profilePicturePath = Path.Combine("uploads", "profile images", "default.png");

//                if (createUser.ProfilePicture != null && createUser.ProfilePicture.Length > 0)
//                {
//                    var fileName = createUser.ProfilePicture.FileName;
//                    var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
//                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile images", uniqueFileName);

//                    var directory = Path.GetDirectoryName(filePath);
//                    if (!Directory.Exists(directory))
//                    {
//                        Directory.CreateDirectory(directory);
//                    }

//                    await using (var stream = new FileStream(filePath, FileMode.Create))
//                    {
//                        await createUser.ProfilePicture.CopyToAsync(stream);
//                    }

//                    profilePicturePath = Path.Combine("uploads", "profile images", uniqueFileName);
//                }

//                // Crea un nuovo utente
//                var newUser = new ApplicationUser
//                {
//                    // Assicurati che l'email sia inclusa nel DTO
//                    Email = createUser.Email,    // Necessario per Identity
//                    Name = createUser.Name,
//                    Surname = createUser.Surname,
//                    Nickname = createUser.Nickname,
//                    ProfilePicture = profilePicturePath,
//                    DateOfBirth = createUser.DateOfBirth,
//                    DateOfRegistration = DateTime.UtcNow,
//                    // Non impostare Password qui, verrà gestito da UserManager
//                };

//                // Crea l'utente con Identity
//                var result = await _userManager.CreateAsync(newUser, createUser.Password);

//                if (!result.Succeeded)
//                {
//                    foreach (var error in result.Errors)
//                    {
//                        ModelState.AddModelError(string.Empty, error.Description);
//                    }
//                    return BadRequest(ModelState);
//                }

//                // Ora che l'utente è stato creato con successo, creiamo la collezione di immagini
//                if (createUser.ImageCollections != null)
//                {
//                    var imageCollection = new ImageCollectionSimpleDto
//                    {
//                        IdUser = newUser.Id // Ora abbiamo l'ID utente generato
//                    };

//                    // Suppongo che tu abbia un metodo per gestire il caricamento delle immagini nella collezione
//                    var collectionCreated = await _imageCollectionService.CreateImageCollection(imageCollection);

//                    if (!collectionCreated)
//                    {
//                        _logger.LogWarning($"Non è stato possibile creare la collezione di immagini per l'utente {newUser.Id}");
//                        // Non facciamo fallire la registrazione solo per questo, ma logghiamo l'errore
//                    }
//                }

//                // Accedi l'utente dopo la registrazione
//                await _signInManager.SignInAsync(newUser, isPersistent: false);

//                return Ok(new
//                {
//                    Message = "Registrazione completata con successo",
//                    UserId = newUser.Id
//                });
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Errore durante la registrazione dell'utente");
//                return StatusCode(500, "Si è verificato un errore interno durante la registrazione");
//            }
//        }


//        [HttpGet]

//        public async Task<IActionResult> Get()
//        {
//            var users = await _userManager.Users.ToListAsync();
//            return Ok(users);
//        }
//    }
//}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PixelIt.Models;
using PixelIt.DTOs.Account;
using PixelIt.ViewModel.Account;
using PixelIt.DTOs.ImageCollection;
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
        private readonly ImageCollectionService _imageCollectionService;

        public UserController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<UserController> logger,
            ImageCollectionService imageCollectionService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
            _imageCollectionService = imageCollectionService;
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
        public async Task<IActionResult> RegistrationUser(AddAccountViewModel model)
        {
            _logger.LogInformation("Ricevuta richiesta di registrazione");

            if (model == null || model.CreateUser == null)
            {
                _logger.LogWarning("Modello nullo ricevuto");
                return View("Registration", new AddAccountViewModel { CreateUser = new CreateUserDto() });
            }

            _logger.LogInformation("Email: {Email}, Nome: {Name}", model.CreateUser.Email, model.CreateUser.Name);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("ModelState non valido");
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning($"Errore in {state.Key}: {error.ErrorMessage}");
                    }
                }
                return View("Registration", model);
            }

            try
            {
                // Verifica se l'email è già in uso
                var existingUser = await _userManager.FindByEmailAsync(model.CreateUser.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("CreateUser.Email", "Questa email è già registrata");
                    return View("Registration", model);
                }

                // Gestisci l'immagine del profilo
                string profilePicturePath = "uploads/profile images/default.png";

                //if (model.CreateUser.ProfilePicture != null && model.CreateUser.ProfilePicture.Length > 0)
                //{
                //    try
                //    {
                //        var fileName = Path.GetFileName(model.CreateUser.ProfilePicture.FileName);
                //        var uniqueFileName = Guid.NewGuid().ToString() + "_" + fileName;
                //        var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "profile images");

                //        if (!Directory.Exists(directory))
                //        {
                //            Directory.CreateDirectory(directory);
                //        }

                //        var filePath = Path.Combine(directory, uniqueFileName);

                //        using (var stream = new FileStream(filePath, FileMode.Create))
                //        {
                //            await model.CreateUser.ProfilePicture.CopyToAsync(stream);
                //        }

                //        profilePicturePath = $"uploads/profile images/{uniqueFileName}";
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Errore durante il salvataggio dell'immagine del profilo");
                //    }
                //}

                // Crea un nuovo utente
                var newUser = new ApplicationUser
                {
                    UserName = model.CreateUser.Email, // Importante per Identity
                    Email = model.CreateUser.Email,
                    Name = model.CreateUser.Name,
                    Surname = model.CreateUser.Surname,
                    Nickname = model.CreateUser.Nickname,
                    ProfilePicture = profilePicturePath,
                    ProfileDescription = model.CreateUser.ProfileDescription,
                    DateOfBirth = model.CreateUser.DateOfBirth, // Ora sono entrambi DateTime
                    DateOfRegistration = DateTime.UtcNow,
                    EmailConfirmed = true // Per semplicità
                };

                // Crea l'utente con Identity
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

                _logger.LogInformation("Utente creato con successo: {UserId}", newUser.Id);

                // Gestisci la collezione di immagini se presente
                //if (model.CreateUser.ImageCollections != null)
                //{
                //    var imageCollectionDto = new ImageCollectionSimpleDto
                //    {
                //        IdUser = newUser.Id
                //    };

                //    try
                //    {
                //        await _imageCollectionService.CreateImageCollection(imageCollectionDto);
                //        _logger.LogInformation("Collezione di immagini creata per l'utente {UserId}", newUser.Id);
                //    }
                //    catch (Exception ex)
                //    {
                //        _logger.LogError(ex, "Errore durante la creazione della collezione di immagini");
                //    }
                //}

                // Accedi l'utente dopo la registrazione
                await _signInManager.SignInAsync(newUser, isPersistent: false);

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la registrazione dell'utente");
                ModelState.AddModelError(string.Empty, "Si è verificato un errore durante la registrazione: " + ex.Message);
                return View("Registration", model);
            }
        }
    }
}