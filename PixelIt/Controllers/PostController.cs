﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Category;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Post;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;
using PixelIt.Services;
using PixelIt.ViewModel.Post;
using PixelIt.ViewModel.User;
using System.Security.Claims;

namespace PixelIt.Controllers
{
    [Authorize]
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

        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    try
        //    {
        //        List<GetPost>? posts = await _postService.GetPost();
        //        if (posts == null || !posts.Any())
        //        {
        //            return NotFound(new { message = "Nessun post trovato" });
        //        }

        //        return Ok(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Errore durante il recupero dei post", error = ex.Message });
        //    }
        //}

        //[HttpGet("{postId:guid}")]
        //public async Task<IActionResult> GetById(Guid postId)
        //{
        //    try
        //    {
        //        var post = await _postService.GetPostByIdAsync(postId);
        //        if (post == null)
        //        {
        //            return NotFound(new { message = "Post non trovato" });
        //        }

        //        return Ok(post);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Errore durante il recupero del post", error = ex.Message });
        //    }
        //}

        //[HttpGet("user/{userId}")]
        //public async Task<IActionResult> GetByUserId(string userId)
        //{
        //    try
        //    {
        //        var posts = await _postService.GetPostsByUserIdAsync(userId);
        //        if (posts == null || !posts.Any())
        //        {
        //            return NotFound(new { message = "Nessun post trovato per questo utente" });
        //        }

        //        return Ok(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Errore durante il recupero dei post dell'utente", error = ex.Message });
        //    }
        //}

        //[HttpGet("my-posts")]
        //public async Task<IActionResult> GetMyPosts()
        //{
        //    try
        //    {
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return Unauthorized(new { message = "Utente non autenticato" });
        //        }

        //        var posts = await _postService.GetPostsByUserIdAsync(userId);
        //        if (posts == null || !posts.Any())
        //        {
        //            return NotFound(new { message = "Non hai ancora pubblicato post" });
        //        }

        //        return Ok(posts);
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Errore durante il recupero dei tuoi post", error = ex.Message });
        //    }
        //}


        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Utente non autenticato";
                return RedirectToAction("Login", "Account");
            }
            //var createPost = new CreatePostDto();
            var createPost = new AddPostViewModel
            {
                Post = new CreatePostDto(),
                Categories = new CreateCategoryDto(),
            };
            ViewBag.Categories = await _categoryService.GetCategories();
            return View( createPost);

        }


        [HttpPost("Create")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreatePost(AddPostViewModel createPost)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Utente non autenticato";
                return RedirectToAction("Login", "Account");
            }

            if (createPost == null || createPost.Post == null)
            {
                ModelState.AddModelError("", "Dati del post non validi");
                return View("Create", new AddPostViewModel { Post = new CreatePostDto(), Categories = new CreateCategoryDto() });
            }

            string webRootPath = null;
            try
            {
                if (createPost.Post.PostImage != null && createPost.Post.PostImage.Length > 0)
                {
                    var fileName = Path.GetFileName(createPost.Post.PostImage.FileName);
                    var uniqueFileName = Guid.NewGuid() + "_" + fileName;
                    var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images");

                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    var filePath = Path.Combine(directory, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await createPost.Post.PostImage.CopyToAsync(stream);
                    }

                    webRootPath = Path.Combine("uploads", "images", uniqueFileName);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Errore durante il caricamento dell'immagine: " + ex.Message);
                return View("Create", new AddPostViewModel { Post = new CreatePostDto(), Categories = new CreateCategoryDto() });
            }

            var postDto = new Post
            {
                IdPost = Guid.NewGuid(),
                Description = createPost.Post.Description,
                PostDate = DateTime.UtcNow,
                PostImage = webRootPath,
                IdUser = user.Id,
                PostCategories = (ICollection<PostCategory>)createPost.Post.PostCategories,
                User = user
            };

            // Salva il post nel database
            var postCreated = await _postService.SavePost(postDto);
            if (!postCreated)
            {
                ModelState.AddModelError("", "Errore durante la creazione del post");
                return View("Create", new AddPostViewModel { Post = new CreatePostDto(), Categories = new CreateCategoryDto() });
            }
            //if (!ModelState.IsValid)
            //{
            //    return await ReturnCreateViewWithCategoriesAsync(createPost);
            //}
            //var postCreated = await _postService.CreatePostAsync(createPost);
            //if (!postCreated)
            //{
            //    ModelState.AddModelError("", "Errore durante la creazione del post");
            //    return await ReturnCreateViewWithCategoriesAsync(createPost);
            //}

            TempData["SuccessMessage"] = "Post creato con successo!";
            return RedirectToAction("Index", "Home");
        }

        
        //private async Task<IActionResult> ReturnCreateViewWithCategoriesAsync(CreatePostDto postDto)
        //{
        //    ViewBag.Categories = await _categoryService.GetCategories();
        //    return View("Create", postDto);
        //}

        // Vista MVC per l'aggiornamento di un post
        //[HttpGet("edit/{idPost:guid}")]
        //public async Task<IActionResult> EditView(Guid idPost)
        //{
        //    try
        //    {
        //        // Ottieni l'ID dell'utente corrente
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            TempData["ErrorMessage"] = "Utente non autenticato";
        //            return RedirectToAction("Login", "Account");
        //        }

        //        // Ottieni il post
        //        var post = await _postService.GetPostByIdAsync(idPost);
        //        if (post == null)
        //        {
        //            TempData["ErrorMessage"] = "Post non trovato";
        //            return RedirectToAction("Index", "Home");
        //        }

        //        // Verifica che l'utente sia il proprietario del post
        //        if (post.IdUser != userId)
        //        {
        //            // Verifica se l'utente è un amministratore
        //            var user = await _userManager.FindByIdAsync(userId);
        //            if (user == null || !await _userManager.IsInRoleAsync(user, "Admin"))
        //            {
        //                TempData["ErrorMessage"] = "Non sei autorizzato a modificare questo post";
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }

        //        // Prepara il DTO per l'aggiornamento
        //        var editDto = new EditPostDto
        //        {
        //            Description = post.Description,
        //            PostImage = post.PostImage,
        //            PostCategories = post.PostCategories
        //        };

        //        // Carica le categorie per la dropdown
        //        ViewBag.Categories = await _categoryService.GetCategories();
        //        ViewBag.PostId = idPost;

        //        return View("Edit", editDto);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //        return View("Error");
        //    }
        //}

        //// Action per l'aggiornamento di un post dalla form MVC
        //[HttpPost("edit/{idPost:guid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> EditPost(EditPostDto updatePost, Guid idPost)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            // Ottieni l'ID dell'utente corrente
        //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //            if (string.IsNullOrEmpty(userId))
        //            {
        //                TempData["ErrorMessage"] = "Utente non autenticato";
        //                return RedirectToAction("Login", "Account");
        //            }

        //            try
        //            {
        //                var editPost = await _postService.EditPostAsync(updatePost, idPost, userId);
        //                if (editPost)
        //                {
        //                    TempData["SuccessMessage"] = "Post aggiornato con successo!";
        //                    return RedirectToAction("Index", "Home");
        //                }

        //                ModelState.AddModelError("", "Errore durante l'aggiornamento del post");
        //            }
        //            catch (UnauthorizedAccessException)
        //            {
        //                TempData["ErrorMessage"] = "Non sei autorizzato a modificare questo post";
        //                return RedirectToAction("Index", "Home");
        //            }
        //        }

        //        // Se arriviamo qui, c'è stato un errore
        //        ViewBag.Categories = await _categoryService.GetCategories();
        //        ViewBag.PostId = idPost;
        //        return View("Edit", updatePost);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //        return View("Error");
        //    }
        //}

        //// API per l'eliminazione di un post
        //[HttpDelete("{idPost:guid}")]
        //public async Task<IActionResult> Delete(Guid idPost)
        //{
        //    try
        //    {
        //        // Ottieni l'ID dell'utente corrente
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            return Unauthorized(new { message = "Utente non autenticato" });
        //        }

        //        try
        //        {
        //            var deletePost = await _postService.DeletePostAsync(idPost, userId);
        //            if (!deletePost)
        //            {
        //                return NotFound(new { message = "Post non trovato" });
        //            }
        //            return Ok(new { message = "Post eliminato con successo" });
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            return Forbid();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, new { message = "Errore durante l'eliminazione del post", error = ex.Message });
        //    }
        //}

        //// Action per l'eliminazione di un post dalla form MVC
        //[HttpPost("delete/{idPost:guid}")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeletePost(Guid idPost)
        //{
        //    try
        //    {
        //        // Ottieni l'ID dell'utente corrente
        //        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        //        if (string.IsNullOrEmpty(userId))
        //        {
        //            TempData["ErrorMessage"] = "Utente non autenticato";
        //            return RedirectToAction("Login", "Account");
        //        }

        //        try
        //        {
        //            var deletePost = await _postService.DeletePostAsync(idPost, userId);
        //            if (deletePost)
        //            {
        //                TempData["SuccessMessage"] = "Post eliminato con successo!";
        //            }
        //            else
        //            {
        //                TempData["ErrorMessage"] = "Post non trovato";
        //            }
        //        }
        //        catch (UnauthorizedAccessException)
        //        {
        //            TempData["ErrorMessage"] = "Non sei autorizzato a eliminare questo post";
        //        }

        //        return RedirectToAction("Index", "Home");
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = ex.Message;
        //        return View("Error");
        //    }
        //}
    }
}