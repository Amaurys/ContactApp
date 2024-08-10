using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ContactApp.Data;
using ContactApp.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ContactApp.Controller
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class ContactsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ContactsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetContacts()
        {
            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized();
            }

            var userId = int.Parse(userIdClaim.Value);
            var contacts = await _context.Contacts.Where(c => c.UserId == userId).ToListAsync();
            return Ok(contacts);
        }

        [HttpPost]
        public async Task<IActionResult> AddContact(Contact contact)
        {
            contact.UserId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value);
            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetContacts), new { id = contact.Id }, contact);
        }
        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateContact(int id, Contact updatedContact)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null || contact.UserId != int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {
                return NotFound();
            }

            contact.Name = updatedContact.Name;
            contact.PhoneNumber = updatedContact.PhoneNumber;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContact(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null || contact.UserId != int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value))
            {
                return NotFound();
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}