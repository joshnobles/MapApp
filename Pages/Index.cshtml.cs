﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SecureSoftware.DataAccess;
using SecureSoftware.Entities;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace SecureSoftware.Pages
{
    public class IndexModel : PageModel
    {
        private readonly Context _context;

        public IndexModel(Context context)
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
            HttpContext.Session.Remove("IdUser");
        }

        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.AddModelError(string.Empty, string.Empty);

            if (Username is null || Password is null)
                return Page();

            Username = HttpUtility.HtmlEncode(Username.Trim());
            Password = HttpUtility.HtmlEncode(Password.Trim());

            Username = await Service.EncryptStringAsync(Username);
            Password = await Service.HashAsync(Password);

            var user = await _context.Users.FirstOrDefaultAsync(u => Username == u.Username && Password == u.Password);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Username or Password");
                return Page();
            }

            HttpContext.Session.SetInt32("IdUser", user.IdUser);

            return Redirect("/Map");
        }

        public IActionResult OnPostNewAccount()
        {
            return Redirect("/NewAccount");
        }

    }
}