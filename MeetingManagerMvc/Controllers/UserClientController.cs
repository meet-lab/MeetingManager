using MeetingManager.Models;
using MeetingManagerMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;


namespace MeetingManagerMvc.Controllers
{
    public class UserClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;
        private readonly IConfiguration _configuration;

        public UserClientController(IConfiguration configuration)
        {
            _configuration = configuration;
            WebApiPath = _configuration["MeetingManager:Url"];
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", _configuration["MeetingManager:ApiKey"]);
        }


        // GET: UserClientController
        public async Task<ActionResult> Index()
        {
            List<User> users = null;
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "/Users");
            if (response.IsSuccessStatusCode)
            {
                users = await response.Content.ReadAsAsync<List<User>>();
            }
            return View(users);
        }



        // GET: UserClientController/Details/5
        public async Task<ActionResult> Details(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();
                return View(user);
            }
            return NotFound();
        }

        public IActionResult RegistryUser(string ReturnUrl = "/")
        {
            UserRegistryModel registryModel = new()
            {
                ReturnUrl = ReturnUrl
            };

            return View(registryModel);
        }

        // POST: UserClientController/RegistryUser
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RegistryUser([Bind("UserName, EmailAddress, Password, RepeatPassword")] UserRegistryModel registryUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (registryUser.Password == registryUser.RepeatPassword)
                    {
                        User user = new()
                        {
                            UserName = registryUser.UserName,
                            EmailAddress = registryUser.EmailAddress,
                            Password = registryUser.Password
                        };

                        HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath + "Users", user);
                        response.EnsureSuccessStatusCode();

                        return Redirect("/UserClient/LoginUser");
                    }
                    else
                    {
                        ViewBag.Message = "Inserted passwords doesn't match";
                        return View(registryUser);
                    }

                }
                catch (Exception exception)
                {
                    return Redirect("/Home/Error");
                }
            }

            return View(registryUser);
        }

        public IActionResult LoginUser(string ReturnUrl = "/")
        {
            LoginModel loginModel = new()
            {
                ReturnUrl = ReturnUrl
            };

            return View(loginModel);
        }

        // POST: UserClientController/LoginUser
        [HttpPost]
        public async Task<IActionResult> LoginUser([Bind("EmailAddressOrUserName,Password,RememberLogin,ReturnUrl")] LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath + "Users/LoginUser", loginModel);

                try
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var user = await response.Content.ReadAsAsync<User>();

                        var claims = new List<Claim>() {
                            new Claim(ClaimTypes.Name, user.UserName == null ? "" : user.UserName),
                            new Claim(ClaimTypes.Email, user.EmailAddress == null ? "" : user.EmailAddress),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(identity);
                        await HttpContext.SignInAsync(
                           CookieAuthenticationDefaults.AuthenticationScheme,
                           principal,
                           new AuthenticationProperties() { IsPersistent = loginModel.RememberLogin }
                        );

                        return LocalRedirect(loginModel.ReturnUrl);
                    }
                    else
                    {
                        ViewBag.Message = "Provided credential is not valid.";
                        return View(loginModel);
                    }
                }
                catch (Exception exception)
                {
                    return Redirect("/Home/Error");
                }

            }

            return View(loginModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        // GET: UserClientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();
                return View(user);
            }
            return NotFound();
        }

        // POST: UserClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("Id,Name,IsComplete")] User user)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + id, user);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: UserClientController/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + id);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();
                return View(user);
            }
            return NotFound();
        }


        // POST: UserClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id, int notUsed = 0)
        {
            HttpResponseMessage response = await client.DeleteAsync(WebApiPath + id);
            response.EnsureSuccessStatusCode();
            return RedirectToAction(nameof(Index));
        }
    }
}

