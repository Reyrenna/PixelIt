using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PixelIt.Data;
using PixelIt.DTOs.Account;
using PixelIt.DTOs.Category;
using PixelIt.DTOs.Comment;
using PixelIt.DTOs.Like;
using PixelIt.DTOs.Post;
using PixelIt.DTOs.PostCategory;
using PixelIt.Models;

namespace PixelIt.Services
{
    public class PostService
    {
        private readonly ApplicationDbContext _context;

        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }

        // Servizio per il salvataggio asincrono del Post
        private async Task<bool> TryPostSaveAsync()
        {
            try
            {
                return await _context.SaveChangesAsync() > 0;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Log dell'errore specifico
                Console.WriteLine($"Errore di concorrenza: {ex.Message}");
                return false;
            }
            catch (DbUpdateException ex)
            {
                // Log dell'errore specifico
                Console.WriteLine($"Errore di aggiornamento database: {ex.Message}");
                return false;
            }
        }

        public async Task<List<GetPost>> GetPost()
        {
            try
            {
                var postList = await _context
                    .Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Likes)
                        .ThenInclude(l => l.User)
                    .Include(p => p.PostCategories)
                        .ThenInclude(pc => pc.Category)
                    .OrderByDescending(p => p.PostDate) // Ordina i post più recenti prima
                    .ToListAsync();

                var postDtoList = postList
                    .Select(p => new GetPost()
                    {
                        IdPost = p.IdPost, // Aggiunto IdPost nel DTO
                        PostImage = p.PostImage,
                        Description = p.Description,
                        PostDate = p.PostDate,
                        IdUser = p.IdUser, // Aggiunto IdUser nel DTO
                        User = new UserPostDto() // Aggiunto dettagli utente
                        {
                            Id = p.User.Id,
                            Nickname = p.User.Nickname,
                            ProfilePicture = p.User.ProfilePicture
                        },
                        PostCategories = p.PostCategories.Select(c => new PostCategorySimpleDto()
                        {
                            Category = new List<GetCategoriesDto>
                            {
                                new GetCategoriesDto
                                {
                                    IdCategory = c.Category.IdCategory, // Aggiunto IdCategory
                                    CategoryName = c.Category.CategoryName
                                }
                            }
                        }).ToList(),
                        Likes = p.Likes.Select(l => new LikeSimpleDto()
                        {
                            IdLike = l.IdLike, // Aggiunto IdLike
                            LikeDate = l.LikeDate,
                            IdPost = l.IdPost,
                            IdUser = l.IdUser,
                            User = new UserPostDto()
                            {
                                Id = l.User.Id,
                                Nickname = l.User.Nickname,
                                ProfilePicture = l.User.ProfilePicture
                            }
                        }).ToList(),
                        Comments = p.Comments.Select(c => new CommentSimpleDto()
                        {
                            IdComment = c.IdComment, // Aggiunto IdComment
                            IdPost = c.IdPost,
                            UserId = c.IdUser,
                            CommentDate = c.CommentDate,
                            CommentText = c.CommentText,
                            User = new UserPostDto()
                            {
                                Id = c.User.Id,
                                Nickname = c.User.Nickname,
                                ProfilePicture = c.User.ProfilePicture
                            },
                            Post = new PostCommentDto()
                            {
                                IdPost = c.Post.IdPost,
                            }
                        }).ToList(),
                    }).ToList();

                return postDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante il recupero dei post", ex);
            }
        }

        // Metodo per ottenere un singolo post per ID
        public async Task<GetPost> GetPostByIdAsync(Guid idPost)
        {
            try
            {
                var post = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Likes)
                        .ThenInclude(l => l.User)
                    .Include(p => p.PostCategories)
                        .ThenInclude(pc => pc.Category)
                    .FirstOrDefaultAsync(p => p.IdPost == idPost);

                if (post == null)
                {
                    return null;
                }

                var postDto = new GetPost()
                {
                    IdPost = post.IdPost,
                    PostImage = post.PostImage,
                    Description = post.Description,
                    PostDate = post.PostDate,
                    IdUser = post.IdUser,
                    User = new UserPostDto()
                    {
                        Id = post.User.Id,
                        Nickname = post.User.Nickname,
                        ProfilePicture = post.User.ProfilePicture
                    },
                    PostCategories = post.PostCategories.Select(c => new PostCategorySimpleDto()
                    {
                        Category = new List<GetCategoriesDto>
                        {
                            new GetCategoriesDto
                            {
                                IdCategory = c.Category.IdCategory,
                                CategoryName = c.Category.CategoryName
                            }
                        }
                    }).ToList(),
                    Likes = post.Likes.Select(l => new LikeSimpleDto()
                    {
                        IdLike = l.IdLike,
                        LikeDate = l.LikeDate,
                        IdPost = l.IdPost,
                        IdUser = l.IdUser,
                        User = new UserPostDto()
                        {
                            Id = l.User.Id,
                            Nickname = l.User.Nickname,
                            ProfilePicture = l.User.ProfilePicture
                        }
                    }).ToList(),
                    Comments = post.Comments.Select(c => new CommentSimpleDto()
                    {
                        IdComment = c.IdComment,
                        IdPost = c.IdPost,
                        UserId = c.IdUser,
                        CommentDate = c.CommentDate,
                        CommentText = c.CommentText,
                        User = new UserPostDto()
                        {
                            Id = c.User.Id,
                            Nickname = c.User.Nickname,
                            ProfilePicture = c.User.ProfilePicture
                        },
                        Post = new PostCommentDto()
                        {
                            IdPost = c.Post.IdPost,
                        }
                    }).ToList(),
                };

                return postDto;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero del post con ID {idPost}", ex);
            }
        }

        // Metodo per ottenere i post di un utente specifico
        public async Task<List<GetPost>> GetPostsByUserIdAsync(string userId)
        {
            try
            {
                var postList = await _context.Posts
                    .Include(p => p.User)
                    .Include(p => p.Comments)
                        .ThenInclude(c => c.User)
                    .Include(p => p.Likes)
                        .ThenInclude(l => l.User)
                    .Include(p => p.PostCategories)
                        .ThenInclude(pc => pc.Category)
                    .Where(p => p.IdUser == userId)
                    .OrderByDescending(p => p.PostDate)
                    .ToListAsync();

                var postDtoList = postList.Select(p => new GetPost()
                {
                    IdPost = p.IdPost,
                    PostImage = p.PostImage,
                    Description = p.Description,
                    PostDate = p.PostDate,
                    IdUser = p.IdUser,
                    User = new UserPostDto()
                    {
                        Id = p.User.Id,
                        Nickname = p.User.Nickname,
                        ProfilePicture = p.User.ProfilePicture
                    },
                    PostCategories = p.PostCategories.Select(c => new PostCategorySimpleDto()
                    {
                        Category = new List<GetCategoriesDto>
                        {
                            new GetCategoriesDto
                            {
                                IdCategory = c.Category.IdCategory,
                                CategoryName = c.Category.CategoryName
                            }
                        }
                    }).ToList(),
                    Likes = p.Likes.Select(l => new LikeSimpleDto()
                    {
                        IdLike = l.IdLike,
                        LikeDate = l.LikeDate,
                        IdPost = l.IdPost,
                        IdUser = l.IdUser,
                        User = new UserPostDto()
                        {
                            Id = l.User.Id,
                            Nickname = l.User.Nickname,
                            ProfilePicture = l.User.ProfilePicture
                        }
                    }).ToList(),
                    Comments = p.Comments.Select(c => new CommentSimpleDto()
                    {
                        IdComment = c.IdComment,
                        IdPost = c.IdPost,
                        UserId = c.IdUser,
                        CommentDate = c.CommentDate,
                        CommentText = c.CommentText,
                        User = new UserPostDto()
                        {
                            Id = c.User.Id,
                            Nickname = c.User.Nickname,
                            ProfilePicture = c.User.ProfilePicture
                        },
                        Post = new PostCommentDto()
                        {
                            IdPost = c.Post.IdPost,
                        }
                    }).ToList(),
                }).ToList();

                return postDtoList;
            }
            catch (Exception ex)
            {
                throw new Exception($"Errore durante il recupero dei post dell'utente {userId}", ex);
            }
        }

        public async Task<bool> CreatePostAsync(CreatePostDto createPost, string userId)
        {
            try
            {
                string webRootPath = null;

                // Gestione dell'immagine se presente
                if (createPost.PostImage != null)
                {
                    var fileName = createPost.PostImage.FileName;
                    var uniqueFileName = Guid.NewGuid() + "_" + fileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", uniqueFileName);

                    // Assicurati che la directory esista
                    var directory = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await createPost.PostImage.CopyToAsync(stream);
                    }

                    webRootPath = Path.Combine("uploads", "images", uniqueFileName);
                }

                var newPost = new Post()
                {
                    IdPost = Guid.NewGuid(),
                    Description = createPost.Description,
                    PostDate = DateTime.UtcNow,
                    PostImage = webRootPath,
                    IdUser = userId,
                    PostCategories = new List<PostCategory>()
                };

                // Aggiunta delle categorie esistenti
                if (createPost.PostCategories != null && createPost.PostCategories.Any())
                {
                    foreach (var pcDto in createPost.PostCategories)
                    {
                        if (pcDto.Category != null && pcDto.Category.Any())
                        {
                            foreach (var catDto in pcDto.Category)
                            {
                                // Controlla se la categoria esiste già
                                var existingCategory = await _context.Categories
                                    .FirstOrDefaultAsync(c => c.CategoryName == catDto.CategoryName);

                                if (existingCategory == null)
                                {
                                    // Crea una nuova categoria se non esiste
                                    existingCategory = new Category
                                    {
                                        IdCategory = Guid.NewGuid(),
                                        CategoryName = catDto.CategoryName
                                    };
                                    _context.Categories.Add(existingCategory);
                                }

                                // Aggiungi la relazione PostCategory
                                newPost.PostCategories.Add(new PostCategory
                                {
                                    PostId = newPost.IdPost,
                                    CategoryId = existingCategory.IdCategory
                                });
                            }
                        }
                    }
                }

                // Aggiungi il post al contesto
                _context.Posts.Add(newPost);

                return await TryPostSaveAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante la creazione del post", ex);
            }
        }

        public async Task<bool> EditPostAsync(EditPostDto updatePost, Guid idPost, string userId)
        {
            try
            {
                var editPost = await _context.Posts
                    .Include(p => p.PostCategories)
                    .FirstOrDefaultAsync(p => p.IdPost == idPost);

                if (editPost == null)
                {
                    return false;
                }

                // Verifica che l'utente sia il proprietario del post
                if (editPost.IdUser != userId)
                {
                    throw new UnauthorizedAccessException("Non sei autorizzato a modificare questo post");
                }

                // Aggiorna i dati del post
                editPost.Description = updatePost.Description;

                // Aggiorna la data solo se richiesto, altrimenti mantieni la data originale
                if (updatePost.UpdateDate)
                {
                    editPost.PostDate = DateTime.UtcNow;
                }

                // Gestione dell'immagine se è stata fornita una nuova
                if (updatePost.NewPostImage != null)
                {
                    // Elimina la vecchia immagine se non è quella di default
                    if (!editPost.PostImage.EndsWith("default-post.jpg"))
                    {
                        var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", editPost.PostImage);
                        if (File.Exists(oldImagePath))
                        {
                            File.Delete(oldImagePath);
                        }
                    }

                    // Salva la nuova immagine
                    var fileName = updatePost.NewPostImage.FileName;
                    var uniqueFileName = Guid.NewGuid() + "_" + fileName;
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", "images", uniqueFileName);

                    await using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await updatePost.NewPostImage.CopyToAsync(stream);
                    }

                    editPost.PostImage = Path.Combine("uploads", "images", uniqueFileName);
                }
                else if (!string.IsNullOrEmpty(updatePost.PostImage) && updatePost.PostImage != editPost.PostImage)
                {
                    // È stato fornito un percorso di immagine diverso
                    editPost.PostImage = updatePost.PostImage;
                }

                // Gestione delle categorie
                if (updatePost.PostCategories != null)
                {
                    // Rimuovi le categorie attuali
                    editPost.PostCategories.Clear();

                    // Aggiungi le nuove categorie
                    foreach (var postCategoryDto in updatePost.PostCategories)
                    {
                        if (postCategoryDto.CategoryId != Guid.Empty)
                        {
                            // Usa categoryId se fornito
                            var postCategory = new PostCategory
                            {
                                PostId = idPost,
                                CategoryId = postCategoryDto.CategoryId
                            };
                            editPost.PostCategories.Add(postCategory);
                        }
                        else if (postCategoryDto.Category != null && postCategoryDto.Category.Any())
                        {
                            // Usa il nome della categoria per trovare o creare la categoria
                            foreach (var catDto in postCategoryDto.Category)
                            {
                                var existingCategory = await _context.Categories
                                    .FirstOrDefaultAsync(c => c.CategoryName == catDto.CategoryName);

                                if (existingCategory == null)
                                {
                                    existingCategory = new Category
                                    {
                                        IdCategory = Guid.NewGuid(),
                                        CategoryName = catDto.CategoryName
                                    };
                                    _context.Categories.Add(existingCategory);
                                }

                                editPost.PostCategories.Add(new PostCategory
                                {
                                    PostId = idPost,
                                    CategoryId = existingCategory.IdCategory
                                });
                            }
                        }
                    }
                }

                _context.Posts.Update(editPost);
                return await TryPostSaveAsync();
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Rilancia l'eccezione per gestirla al livello superiore
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'aggiornamento del post", ex);
            }
        }

        public async Task<bool> DeletePostAsync(Guid idPost, string userId)
        {
            try
            {
                var post = await _context.Posts.FindAsync(idPost);
                if (post == null)
                {
                    return false;
                }

                // Verifica che l'utente sia il proprietario del post o un amministratore
                if (post.IdUser != userId)
                {
                    // Verifica se l'utente è un amministratore (modifica in base alla tua logica di autorizzazione)
                    var user = await _context.Users.FindAsync(userId);
                    if (user == null || !await _context.UserRoles.AnyAsync(ur => ur.UserId == userId && ur.RoleId == "Admin"))
                    {
                        throw new UnauthorizedAccessException("Non sei autorizzato a eliminare questo post");
                    }
                }

                // Elimina l'immagine del post se non è quella di default
                if (!post.PostImage.EndsWith("default-post.jpg"))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", post.PostImage);
                    if (File.Exists(imagePath))
                    {
                        File.Delete(imagePath);
                    }
                }

                _context.Posts.Remove(post);
                return await TryPostSaveAsync();
            }
            catch (UnauthorizedAccessException)
            {
                throw; // Rilancia l'eccezione per gestirla al livello superiore
            }
            catch (Exception ex)
            {
                throw new Exception("Errore durante l'eliminazione del post", ex);
            }
        }
    }
}
