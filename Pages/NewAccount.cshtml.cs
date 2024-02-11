using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecureSoftware.DataAccess;
using SecureSoftware.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SecureSoftware.Pages
{
    public class NewAccountModel : PageModel
    {
        private readonly Context _context;

        public NewAccountModel(Context context)
        {
            _context = context;
        }

        [BindProperty]
        [StringLength(100)]
        public string Username { get; set; } = null!;

        [BindProperty]
        [StringLength(500)]
        public string Password { get; set; } = null!;

        public Service Service = new();

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.AddModelError(string.Empty, string.Empty);

            Username = Username.Trim();
            Password = Password.Trim();

            Regex.Replace(Username, "<.*?>", string.Empty);
            Username = await Service.EncryptStringAsync(Username);
            Password = await Service.HashAsync(Password);

            var NewUser = new User()
            {
                Username = Username,
                Password = Password
            };

            await _context.Users.AddAsync(NewUser);
            await _context.SaveChangesAsync();

            HttpContext.Session.SetInt32("IdUser", NewUser.IdUser);

            return Redirect("/Map");
        }

    }
}
