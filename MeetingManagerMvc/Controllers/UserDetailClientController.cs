using MeetingManager.Models;
using MeetingManagerMvc.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Controllers
{
    [Authorize]
    public class UserDetailClientController : Controller
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;
        private readonly IConfiguration _configuration;

        public UserDetailClientController(IConfiguration configuration)
        {
            _configuration = configuration;
            WebApiPath = _configuration["MeetingManager:Url"];
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", _configuration["MeetingManager:ApiKey"]);
        }

        // GET: UserDetailClient/5
        public async Task<ActionResult> Index(int userId)
        {
            var identity = User.Claims.Where(e => e.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier").FirstOrDefault();
            if (identity == null)
            {
                return Redirect("/");
            }

            HttpResponseMessage response = await client.GetAsync(WebApiPath + "UsersDetail/" + identity.Value);
            if (response.IsSuccessStatusCode)
            {
                UserDetail detail = await response.Content.ReadAsAsync<UserDetail>();
                return View(detail);
            }

            return NotFound();
        }

        // GET: UserClientController/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + "UsersDetail/" + id);
            if (response.IsSuccessStatusCode)
            {
                UserDetail userDetail = await response.Content.ReadAsAsync<UserDetail>();
                return View(userDetail);
            }
            return NotFound();
        }

        // POST: UserClientController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, [Bind("UserDetailId,Name,SecondName,LastName,Address,City,Region,Country,PostCode,Phone,UserId")] UserDetail userDetail)
        {
            if (ModelState.IsValid)
            {
                HttpResponseMessage response = await client.PutAsJsonAsync(WebApiPath + "UsersDetail/" + userDetail.UserId, userDetail);
                response.EnsureSuccessStatusCode();
                return RedirectToAction(nameof(Index));
            }

            return View(userDetail);
        }
    }
}

