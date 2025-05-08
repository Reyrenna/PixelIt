using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PixelIt.Services;
using PixelIt.ViewModel;
using PixelIt.ViewModel.Post;

namespace PixelIt.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PostService _postService;

    public HomeController(ILogger<HomeController> logger, PostService postService)
    {
        _logger = logger;
        _postService = postService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Registration()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> Index(GetPostViewModel getPost)
    {
        try
        {
            var posts = await _postService.GetPostView();
            if (posts == null || !posts.Any())
            {
                return NotFound(new { message = "Nessun post trovato" });
            }
            return View("Index", posts);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Errore durante il recupero dei post", error = ex.Message });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
