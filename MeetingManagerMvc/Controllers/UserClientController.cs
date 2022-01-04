using MeetingManager.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration; //IConfiguration
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
        private readonly List<User> users;

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
            HttpResponseMessage response = await client.GetAsync(WebApiPath);
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


        // GET: UserClientController/Create
        public ActionResult Create()
        {
            return View();
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
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath, loginModel);
                response.EnsureSuccessStatusCode();

                if (response.IsSuccessStatusCode)
                {
                    //A claim is a statement about a subject by an issuer and
                    //represent attributes of the subject that are useful in the context of authentication and authorization operations.
                    var claims = new List<Claim>() {
                    new Claim(ClaimTypes.Name, loginModel.EmailAddressOrUserName)
                };
                    //Initialize a new instance of the ClaimsIdentity with the claims and authentication scheme
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    //Initialize a new instance of the ClaimsPrincipal with ClaimsIdentity
                    var principal = new ClaimsPrincipal(identity);
                    //Signin user and create cookie
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
            return View(loginModel);
        }

        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("/");
        }

        // POST: UserClientController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Id,Name,IsComplete")] User user)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PostAsJsonAsync(WebApiPath, user);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
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

