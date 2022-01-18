using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MeetingManager.Data;
using MeetingManager.Models;
using LibraryApi.Attributes;
using BC = BCrypt.Net.BCrypt;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;

namespace MeetingManager
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiKey]
    public class UsersController : ControllerBase
    {
        private readonly MeetingManagerContext _context;

        public UsersController(MeetingManagerContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUser()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            user.UserDetail = new UserDetail()
            {
                Name = "",
                SecondName = "",
                LastName = "",
                Address = "",
                Phone = "",
                City = "",
                Country = "",
                Region = "",
                PostCode = ""
            };

            user.Cart = new Cart(){};
            var hashPasssword = BC.HashPassword(user.Password);
            user.Password = hashPasssword;

            _context.User.Add(user);

            await _context.SaveChangesAsync();

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json");
            var config = builder.Build();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(config["Smtp:Host"]);

            mail.From = new MailAddress(config["Smtp:NotificationEmail"]);
            mail.To.Add(user.EmailAddress);
            mail.Subject = "MeetingManager | User account created!";
            mail.Body = "We noticed that you create user account in our service. We are happy that you choose our service ! \n \n \n" +
                "Username: " + user.UserName + "\n" +
                "EmailAddress: " + user.EmailAddress + "\n";

            SmtpServer.Port = Int16.Parse(config["Smtp:Port"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // POST: api/Users/LoginUser
        [HttpPost("LoginUser")]
        public async Task<ActionResult<User>> LoginUser([FromBody] LoginModel loginData)
        {
            var user = await _context.User.Where(u => (u.EmailAddress == loginData.EmailAddressOrUserName || u.UserName == loginData.EmailAddressOrUserName )).FirstOrDefaultAsync();

            if (user == null || !BC.Verify(loginData.Password, user.Password))
            {
                return NotFound();
            }

            return user;
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            var builder = new ConfigurationBuilder()
                   .AddJsonFile("appsettings.json");
            var config = builder.Build();

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient(config["Smtp:Host"]);

            mail.From = new MailAddress(config["Smtp:NotificationEmail"]);
            mail.To.Add(user.EmailAddress);
            mail.Subject = "MeetingManager | User account deleted!";
            mail.Body = "Your account was deleted Bye Bye! \n \n \n" +
                "Username: " + user.UserName + "\n" +
                "EmailAddress: " + user.EmailAddress + "\n";

            SmtpServer.Port = Int16.Parse(config["Smtp:Port"]);
            SmtpServer.Credentials = new System.Net.NetworkCredential(config["Smtp:Username"], config["Smtp:Password"]);
            SmtpServer.EnableSsl = true;

            SmtpServer.Send(mail);

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
