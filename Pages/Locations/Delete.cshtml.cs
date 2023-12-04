using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;

namespace csci340_iseegreen.Pages_Locations
{
    public class DeleteModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public DeleteModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        [BindProperty]
      public Locations Locations { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }

            var locations = await _context.Locations.FirstOrDefaultAsync(m => m.Id == id);

            if (locations == null)
            {
                return NotFound();
            }
            else 
            {
                Locations = locations;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null || _context.Locations == null)
            {
                return NotFound();
            }
            var locations = await _context.Locations.FindAsync(id);

            if (locations != null)
            {
                Locations = locations;
                _context.Locations.Remove(Locations);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
