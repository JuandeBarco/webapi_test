using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi_test.Models;

namespace webapi_test.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarContext _context;

        public CarController(CarContext context)
        {
            _context = context;

            // create 3 items when the database is empty
            if (_context.CarItems.Count() == 0)
            {
                _context.CarItems.Add(new CarItem { Brand = "Toyota", SubBrand = "Corolla", Color = "White", Year = 2010 });
                _context.CarItems.Add(new CarItem { Brand = "Toyota", SubBrand = "Camry", Color = "Black", Year = 2015 });
                _context.CarItems.Add(new CarItem { Brand = "Honda", SubBrand = "Accord", Color = "Red", Year = 2018 });
                _context.SaveChanges();
            }
        }

        // GET: api/Car
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CarItem>>> GetCarItems()
        {
          if (_context.CarItems == null)
          {
              return NotFound();
          }
            return await _context.CarItems.ToListAsync();
        }

        // GET: api/Car/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CarItem>> GetCarItem(int id)
        {
          if (_context.CarItems == null)
          {
              return NotFound();
          }
            var carItem = await _context.CarItems.FindAsync(id);

            if (carItem == null)
            {
                return NotFound();
            }

            return carItem;
        }

        // PUT: api/Car/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCarItem(int id, CarItem carItem)
        {
            if (id != carItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(carItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CarItemExists(id))
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

        // POST: api/Car
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<CarItem>> PostCarItem(CarItem carItem)
        {
          if (_context.CarItems == null)
          {
              return Problem("Entity set 'CarContext.CarItems'  is null.");
          }
            _context.CarItems.Add(carItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCarItem", new { id = carItem.Id }, carItem);
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCarItem(int id)
        {
            if (_context.CarItems == null)
            {
                return NotFound();
            }
            var carItem = await _context.CarItems.FindAsync(id);
            if (carItem == null)
            {
                return NotFound();
            }

            _context.CarItems.Remove(carItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CarItemExists(int id)
        {
            return (_context.CarItems?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
