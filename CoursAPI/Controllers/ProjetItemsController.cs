using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CoursAPI;
using CoursAPI.Models;

namespace CoursAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjetItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProjetItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ProjetItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProjetItem>>> GetProjetItems()
        {
            return await _context.ProjetItems.ToListAsync();
        }


        // POST: api/ProjetItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ProjetItem>> PostProjetItem(ProjetItem projetItem)
        {
            _context.ProjetItems.Add(projetItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProjetItem", new { id = projetItem.Id }, projetItem);
        }

        // DELETE: api/ProjetItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProjetItem(int id)
        {
            var projetItem = await _context.ProjetItems.FindAsync(id);
            if (projetItem == null)
            {
                return NotFound();
            }

            _context.ProjetItems.Remove(projetItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProjetItemExists(int id)
        {
            return _context.ProjetItems.Any(e => e.Id == id);
        }
    }
}
