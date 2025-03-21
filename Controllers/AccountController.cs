using System.Security.Claims;
using GasStationApp.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BCrypt.Net; 

namespace GasStationApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("ModelState geçersiz!");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine("Hata: " + error.ErrorMessage);
                }

                return View(model);
            }

            try
            {
                string city = string.IsNullOrEmpty(model.City) ? "Bilinmiyor" : model.City;
                string province = string.IsNullOrEmpty(model.Province) ? "Bilinmiyor" : model.Province;

                string hashedPassword = model.UserPassword; 

                var user = new UserModel
                {
                    UserNameLastName = model.UserNameLastName,
                    UserPhoneNumber = model.UserPhoneNumber,
                    UserEmail = model.UserEmail,
                    UserPassword = hashedPassword,
                    StationCode = model.StationCode,
                    StationName = model.StationName,
                    CompanyTaxNumber = model.CompanyTaxNumber,
                    City = city,
                    Province = province
                };

                await _context.Users.AddAsync(user);
                int affectedRows = await _context.SaveChangesAsync();

                if (affectedRows == 0)
                {
                    Console.WriteLine("Kullanıcı veritabanına eklenemedi!");
                    ModelState.AddModelError("", "Kullanıcı kaydedilemedi!");
                    return View(model);
                }

                Console.WriteLine("Kullanıcı başarıyla kaydedildi!");
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Bir hata oluştu: " + ex.Message);
                ModelState.AddModelError("", "Kayıt sırasında bir hata oluştu: " + ex.Message);
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(RegisterViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.UserEmail) || string.IsNullOrWhiteSpace(model.UserPassword))
            {
                ModelState.AddModelError(string.Empty, "E-posta ve şifre zorunludur!");
                return View(model);
            }

            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserEmail == model.UserEmail);

            if (user == null || user.UserPassword != model.UserPassword)
            {
                ModelState.AddModelError(string.Empty, "E-posta veya şifre hatalı!");
                return View(model);
            }

            if (string.IsNullOrEmpty(user.UserEmail))
            {
                ModelState.AddModelError(string.Empty, "Geçersiz e-posta adresi.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Kullanıcı ID'sini ekledik
                new Claim(ClaimTypes.Name, user.UserNameLastName),
                new Claim(ClaimTypes.Email, user.UserEmail ?? string.Empty) // Null kontrolü ekledik
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties { IsPersistent = true };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

            return RedirectToAction("StorageInfo", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            Response.Headers["Cache-Control"] = "no-store, no-cache, must-revalidate";
            Response.Headers["Pragma"] = "no-cache";
            Response.Headers["Expires"] = "0";

            return RedirectToAction("Index", "Home");
        }

        

    }
}
