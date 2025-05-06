using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Category;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Post;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;
using PixelIt.Services;
using System.Security.Claims;

namespace PixelIt.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostController : Controller
    {
        private readonly PostService _postService;
        private readonly CategoryService _categoryService;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(PostService postService, CategoryService categoryService, UserManager<ApplicationUser> userManager)
        {
            _postService = postService;
            _categoryService = categoryService;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GetPost>? posts = await _postService.GetPost();
                if (posts == null || !posts.Any())
                {
                    return NotFound(new { message = "Nessun post trovato" });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante il recupero dei post", error = ex.Message });
            }
        }

        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> GetById(Guid postId)
        {
            try
            {
                var post = await _postService.GetPostByIdAsync(postId);
                if (post == null)
                {
                    return NotFound(new { message = "Post non trovato" });
                }

                return Ok(post);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante il recupero del post", error = ex.Message });
            }
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetByUserId(string userId)
        {
            try
            {
                var posts = await _postService.GetPostsByUserIdAsync(userId);
                if (posts == null || !posts.Any())
                {
                    return NotFound(new { message = "Nessun post trovato per questo utente" });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante il recupero dei post dell'utente", error = ex.Message });
            }
        }

        [HttpGet("my-posts")]
        public async Task<IActionResult> GetMyPosts()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Utente non autenticato" });
                }

                var posts = await _postService.GetPostsByUserIdAsync(userId);
                if (posts == null || !posts.Any())
                {
                    return NotFound(new { message = "Non hai ancora pubblicato post" });
                }

                return Ok(posts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante il recupero dei tuoi post", error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(CreatePostDto postDto)
        {
            if (!ModelState.IsValid)
            {
                return await ReturnCreateViewWithCategoriesAsync(postDto);
            }

            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                ModelState.AddModelError("", "Utente non autenticato");
                return await ReturnCreateViewWithCategoriesAsync(postDto);
            }

            var postCreated = await _postService.CreatePostAsync(postDto, userId);
            if (!postCreated)
            {
                ModelState.AddModelError("", "Errore durante la creazione del post");
                return await ReturnCreateViewWithCategoriesAsync(postDto);
            }

            TempData["SuccessMessage"] = "Post creato con successo!";
            return RedirectToAction("Index", "Home");
        }

        private async Task<IActionResult> ReturnCreateViewWithCategoriesAsync(CreatePostDto postDto)
        {
            ViewBag.Categories = await _categoryService.GetCategories();
            return View("Create", postDto);
        }


        // API per l'aggiornamento di un post
        [HttpPut("{idPost:guid}")]
        public async Task<IActionResult> Edit([FromForm] EditPostDto updatePost, Guid idPost)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Ottieni l'ID dell'utente corrente
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Utente non autenticato" });
                }

                try
                {
                    var editPost = await _postService.EditPostAsync(updatePost, idPost, userId);
                    if (!editPost)
                    {
                        return BadRequest(new { message = "Post non trovato o errore durante l'aggiornamento" });
                    }
                    return Ok(new { message = "Post aggiornato con successo" });
                }
                catch (UnauthorizedAccessException)
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante l'aggiornamento del post", error = ex.Message });
            }
        }

        // Vista MVC per l'aggiornamento di un post
        [HttpGet("edit/{idPost:guid}")]
        public async Task<IActionResult> EditView(Guid idPost)
        {
            try
            {
                // Ottieni l'ID dell'utente corrente
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Utente non autenticato";
                    return RedirectToAction("Login", "Account");
                }

                // Ottieni il post
                var post = await _postService.GetPostByIdAsync(idPost);
                if (post == null)
                {
                    TempData["ErrorMessage"] = "Post non trovato";
                    return RedirectToAction("Index", "Home");
                }

                // Verifica che l'utente sia il proprietario del post
                if (post.IdUser != userId)
                {
                    // Verifica se l'utente è un amministratore
                    var user = await _userManager.FindByIdAsync(userId);
                    if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
                    {
                        TempData["ErrorMessage"] = "Non sei autorizzato a modificare questo post";
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Prepara il DTO per l'aggiornamento
                var editDto = new EditPostDto
                {
                    Description = post.Description,
                    PostImage = post.PostImage,
                    PostCategories = post.PostCategories
                };

                // Carica le categorie per la dropdown
                ViewBag.Categories = await _categoryService.GetCategories();
                ViewBag.PostId = idPost;

                return View("Edit", editDto);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // Action per l'aggiornamento di un post dalla form MVC
        [HttpPost("edit/{idPost:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(EditPostDto updatePost, Guid idPost)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    // Ottieni l'ID dell'utente corrente
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (string.IsNullOrEmpty(userId))
                    {
                        TempData["ErrorMessage"] = "Utente non autenticato";
                        return RedirectToAction("Login", "Account");
                    }

                    try
                    {
                        var editPost = await _postService.EditPostAsync(updatePost, idPost, userId);
                        if (editPost)
                        {
                            TempData["SuccessMessage"] = "Post aggiornato con successo!";
                            return RedirectToAction("Index", "Home");
                        }

                        ModelState.AddModelError("", "Errore durante l'aggiornamento del post");
                    }
                    catch (UnauthorizedAccessException)
                    {
                        TempData["ErrorMessage"] = "Non sei autorizzato a modificare questo post";
                        return RedirectToAction("Index", "Home");
                    }
                }

                // Se arriviamo qui, c'è stato un errore
                ViewBag.Categories = await _categoryService.GetCategories();
                ViewBag.PostId = idPost;
                return View("Edit", updatePost);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }

        // API per l'eliminazione di un post
        [HttpDelete("{idPost:guid}")]
        public async Task<IActionResult> Delete(Guid idPost)
        {
            try
            {
                // Ottieni l'ID dell'utente corrente
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Utente non autenticato" });
                }

                try
                {
                    var deletePost = await _postService.DeletePostAsync(idPost, userId);
                    if (!deletePost)
                    {
                        return NotFound(new { message = "Post non trovato" });
                    }
                    return Ok(new { message = "Post eliminato con successo" });
                }
                catch (UnauthorizedAccessException)
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Errore durante l'eliminazione del post", error = ex.Message });
            }
        }

        // Action per l'eliminazione di un post dalla form MVC
        [HttpPost("delete/{idPost:guid}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePost(Guid idPost)
        {
            try
            {
                // Ottieni l'ID dell'utente corrente
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    TempData["ErrorMessage"] = "Utente non autenticato";
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    var deletePost = await _postService.DeletePostAsync(idPost, userId);
                    if (deletePost)
                    {
                        TempData["SuccessMessage"] = "Post eliminato con successo!";
                    }
                    else
                    {
                        TempData["ErrorMessage"] = "Post non trovato";
                    }
                }
                catch (UnauthorizedAccessException)
                {
                    TempData["ErrorMessage"] = "Non sei autorizzato a eliminare questo post";
                }

                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = ex.Message;
                return View("Error");
            }
        }
    }
}