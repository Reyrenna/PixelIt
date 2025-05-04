using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PixelIt.Models;


namespace PixelIt.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RoleService> _logger;

        public RoleService(
            RoleManager<IdentityRole> roleManager,
            UserManager<ApplicationUser> userManager,
            ILogger<RoleService> logger)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> RoleExistsAsync(string roleName)
        {
            return await _roleManager.RoleExistsAsync(roleName);
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (await RoleExistsAsync(roleName))
            {
                return IdentityResult.Success;
            }

            var result = await _roleManager.CreateAsync(new IdentityRole(roleName));
            if (result.Succeeded)
            {
                _logger.LogInformation($"Ruolo '{roleName}' creato con successo");
            }
            else
            {
                _logger.LogWarning($"Errore nella creazione del ruolo '{roleName}'");
            }

            return result;
        }

        public async Task<IdentityResult> AssignRoleToUserAsync(ApplicationUser user, string roleName)
        {
            // Verifica se il ruolo esiste
            if (!await RoleExistsAsync(roleName))
            {
                await CreateRoleAsync(roleName);
            }

            // Verifica se l'utente ha già il ruolo
            if (await _userManager.IsInRoleAsync(user, roleName))
            {
                return IdentityResult.Success;
            }

            return await _userManager.AddToRoleAsync(user, roleName);
        }

        public async Task<IdentityResult> RemoveRoleFromUserAsync(ApplicationUser user, string roleName)
        {
            if (!await _userManager.IsInRoleAsync(user, roleName))
            {
                return IdentityResult.Success;
            }

            return await _userManager.RemoveFromRoleAsync(user, roleName);
        }
    }
}