
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using News.Data;
using News.Models;

namespace News.Controllers
{
    // [Route("auth")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;
        private readonly NewsContext _context;

        public AuthController(ILogger<AuthController> logger, NewsContext context)
        {
            _logger = logger;
            _context = context;
        }

        // /login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.Users.FirstOrDefaultAsync(
                    u => u.Email.Equals(model.Email)
                );

                if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                {
                    ModelState.AddModelError("error", "wrong email or password");
                }
                else
                {

                    var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.DisplayName),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role!.Name),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                    await HttpContext.SignInAsync(claimsPrincipal);

                    return Redirect("/");
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var query = _context.Users.AsQueryable();

                var emailExist = await query.FirstOrDefaultAsync(u => u.Email.Equals(model.Email));

                var displayNameExist = await query.FirstOrDefaultAsync(u => u.DisplayName.Equals(model.DisplayName));


                if (emailExist != null)
                {
                    ModelState.AddModelError("Email", "Email have already use");
                }

                if (displayNameExist != null)
                {
                    ModelState.AddModelError("DisplayName", "Display name have already use");
                }

                if (!model.Password.Equals(model.RePassword))
                {
                    ModelState.AddModelError("error", "Passwords and password re-entry are not the same");
                }
                else if (emailExist == null && displayNameExist == null)
                {
                    var passwrodHash = BCrypt.Net.BCrypt.HashPassword(model.Password);

                    var user = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = model.Email,
                        DisplayName = model.DisplayName,
                        Password = passwrodHash,
                        RoleId = 2
                    };

                    _context.Add(user);
                    _context.SaveChanges();

                    return RedirectToAction("Login", "Auth");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme
            );

            return Redirect("/");
        }

        // google login
        // [Route("google-login")]
        public IActionResult GoogleLogin()
        {
            var properties = new AuthenticationProperties { RedirectUri = Url.Action("GoogleResponse") };

            return Challenge(properties, GoogleDefaults.AuthenticationScheme);
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            var emailClaim = result.Principal!.FindFirst(ClaimTypes.Email);
            if (emailClaim != null)
            {
                var email = emailClaim.Value;

                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(email));

                if (user == null)
                {
                    var newUser = new User
                    {
                        Id = Guid.NewGuid(),
                        Email = email,
                        DisplayName = email.Split("@")[0],
                        RoleId = 2
                    };

                    _context.Add(newUser);
                    _context.SaveChanges();

                    user = newUser;
                }


                var role = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);

                var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.DisplayName),
                        new Claim("UserId", user.Id.ToString()),
                        new Claim(ClaimTypes.Role, role!.Name),
                    };

                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

                await HttpContext.SignInAsync(claimsPrincipal);

            }

            return Redirect("/");
        }
    }
}

