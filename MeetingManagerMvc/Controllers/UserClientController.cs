using MeetingManager.Models;
using MeetingManagerMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using BC = BCrypt.Net.BCrypt;


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
                            Password = registryUser.Password,
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

        [Authorize]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        // GET: UserClientController/Edit/5
        [Authorize]
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Users/" + id);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();

                UserRegistryModel registerUser = new()
                {
                    UserName = user.UserName,
                    EmailAddress = user.EmailAddress
                };

                return View(registerUser);
            }
            return NotFound();
        }

        // POST: UserClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Edit(int id, [Bind("UserName,EmailAddress,Password,RepeatPassword")] UserRegistryModel registerUser)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (registerUser.Password == registerUser.RepeatPassword)
                    {
                        User user = new()
                        {
                            Id = id,
                            UserName = registerUser.UserName,
                            EmailAddress = registerUser.EmailAddress,
                            Password = BC.HashPassword(registerUser.Password),
                        };

                        HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + "Users/" + id, user);
                        response.EnsureSuccessStatusCode();

                        return Redirect("/UserDetailClient/Index");
                    }
                    else
                    {
                        ViewBag.Message = "Inserted passwords doesn't match";

                        return View(registerUser);
                    }
                }
                catch (Exception exception)
                {
                    return Redirect("/Home/Error");
                }
                
            }

            return View(registerUser);
        }

        // GET: UserClientController/Delete/5
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "Users/" + id);
            if (response.IsSuccessStatusCode)
            {
                User user = await response.Content.ReadAsAsync<User>();

                UserRegistryModel deleteUserModel = new()
                {
                    UserName = user.UserName,
                    Password = user.Password,
                    EmailAddress = user.EmailAddress,
                    RepeatPassword = ""
                };

                return View(deleteUserModel);
            }

            return NotFound();
        }

        // POST: UserClientController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<ActionResult> Delete(int id, [Bind("UserName,EmailAddress,RepeatPassword,Password")] UserRegistryModel deleteUser)
        {
            if (ModelState.IsValid)
            {
                if (BC.Verify(deleteUser.RepeatPassword, deleteUser.Password))
                {
                    HttpResponseMessage response = await client.DeleteAsync(WebApiPath + "Users/" + id);
                    response.EnsureSuccessStatusCode();
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    return LocalRedirect("/");
                }
                else
                {
                    ViewBag.Message = "Wrong password was provided try again!";

                    return View(deleteUser);
                }
            }

            return View(deleteUser);
        }
    }
}

