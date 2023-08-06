using Dotnet6MvcLogin.Models.Domain;
using Dotnet6MvcLogin.Models.DTO;
using Dotnet6MvcLogin.Repositories.Abstract;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dotnet6MvcLogin.Controllers
{
    public class UserAuthenticationController : Controller
    {
        private readonly IUserAuthenticationService _authService;
        private readonly DatabaseContext _context;
        public UserAuthenticationController(IUserAuthenticationService authService, DatabaseContext context)
        {
            this._authService = authService;
            _context = context;
        }
        

        public IActionResult Login()
        {
            return View();
        }

       
        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var result = await _authService.LoginAsync(model);
            if (model.Role=="user")
            {
                Userinfo.userid = model.UserID;
                return RedirectToAction("Index", "Contacts");
            }
            if(result.StatusCode==1)
            {
                return RedirectToAction("Display", "Dashboard");
            }
            else
            {
                TempData["msg"] = result.Message;
                return RedirectToAction(nameof(Login));
            }
        }
       
        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationModel model)
        {
            
            if(!ModelState.IsValid) { return View(model); }
            model.Role = "user";
            var result = await this._authService.RegisterAsync(model);
            if (result.Message== "You have registered successfully")
            {
                Contact contact = new Contact();
                contact.Name = model.FirstName + " " + model.LastName;
                contact.Email = model.Email;
                contact.Address = "-";
                contact.City = "-";
                contact.State = "-";
                contact.Zip="-";
                contact.UserId = model.ID;
                contact.Status = "pending";
                _context.Add(contact);
                await _context.SaveChangesAsync();
            }
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(Registration));
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await this._authService.LogoutAsync();  
            return RedirectToAction(nameof(Login));
        }
        [AllowAnonymous]
        //public async Task<IActionResult> RegisterAdmin()
        //{
        //    RegistrationModel model = new RegistrationModel
        //    {
        //        Username = "admin",
        //        Email = "admin@gmail.com",
        //        FirstName = "John",
        //        LastName = "Doe",
        //        Password = "Admin@12345#"
        //    };
        //    model.Role = "admin";
        //    var result = await this._authService.RegisterAsync(model);
        //    return Ok(result);
        //}

        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult>ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
              return View(model);
            var result = await _authService.ChangePasswordAsync(model, User.Identity.Name);
            TempData["msg"] = result.Message;
            return RedirectToAction(nameof(ChangePassword));
        }

    }
}
