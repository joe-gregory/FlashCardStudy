// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Identity.Client;
using Models;
using NuGet.Protocol.Core.Types;

namespace Web.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IEmailSender _emailSender;

        public IndexModel(UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
        }
        public string Username { get; set; }
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; }
        [TempData]
        public string StatusMessage { get; set; }
        [BindProperty]
        public string UserId { get; set; }
        public User User { get; set; }

        public async Task<bool> GetUserAsync()
        {
            User = await _userManager.GetUserAsync(base.User);
            if (User == null)
            {
                TempData["ErrorMessage"] = $"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.";
                return false;
            }
            else
            {
                var userName = await _userManager.GetUserNameAsync(User);
                var isEmailConfirmed = await _userManager.IsEmailConfirmedAsync(User);
                var email = await _userManager.GetEmailAsync(User);
                var userId = await _userManager.GetUserIdAsync(User);

                Username = userName;
                IsEmailConfirmed = isEmailConfirmed;

                Email = email;
                UserId = userId;
                return true;
            }
        }
        public async Task<IActionResult> OnGetAsync()
        {
            if (await GetUserAsync())
            {
                return Page();
            }
            else
            {
                return NotFound();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (await GetUserAsync())
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                
                var callbackUrl = Url.Page(
                    "/Account/ConfirmEmail",
                    pageHandler: null,
                    values: new { userId = UserId, code = code },
                    protocol: Request.Scheme);
                await _emailSender.SendEmailAsync(
                    Email,
                    "Study Stacks: Please confirm your email",
                    $"<p>Hi!</p> <p>This is Study Stacks. Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.</p>");

                TempData["SuccessMessage"] = "Verification email sent 🚀. Please check your email.";
                return Page();
            }
            else
            {
                TempData["ErrorMessage"] = "We had a problem sending your email 😑. Please contact us if problem persists.";
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(base.User)}'.");
            }
        }
    }
}
