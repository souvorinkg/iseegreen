using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using System.Data;
using System.Runtime.CompilerServices;

namespace csci340_iseegreen.Pages.Search
{
    public class IndexModel : PageModel
    {
        private readonly ISeeGreenContext _context;
        private readonly IConfiguration Configuration;

        public IndexModel(ISeeGreenContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
            CategoryOptions = new List<(string, string)>();
            foreach (var cat in context.Categories)
            {
                CategoryOptions.Add((cat.Category, cat.Description));
            }
        }

        public PaginatedList<Models.Taxa> Taxa { get;set; } = default!;
        public List<string> Families { get; set; } = new List<string>();
        public IList<(string, string)> CategoryOptions { get; set; }
        public List<Models.Lists> SelectList { get; set; } = new List<Models.Lists>();

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? FamilyString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? GenusString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? CategoryFilter { get; set; }

        [BindProperty]
        public int? SelectedListId { get; set; }

        [BindProperty]
        public string? SelectedKewId { get; set; }
        [BindProperty]
        public int? PageIndex {get; set;}

        public SelectList? Categories { get; set; }

        public string SpeciesSort { get; set; }
        public string GenusSort { get; set; }
        public string CurrentSpecies { get; set; }
        public string CurrentGenus { get; set; }
        public string CurrentFamily { get; set; }
        public string CurrentSort { get; set; }
        public string CurrentFilter { get; set; }
        public string CurrentCat { get; set; }
        

        private async Task LoadTaxaAsync(string sortOrder, int? pageIndex)
        {
            IQueryable<Models.Taxa> taxaIQ = from t in _context.Taxa
                                            .Include(g => g.Genus)
                                            .Include(f => f.Genus.Family)
                                            .Include(c => c.Genus.Family.Category)
                                            select t;

            if (SearchString != null) {
                SearchString = SearchString.ToUpper();
                CurrentSpecies = SearchString;
            }
            else {
                SearchString = CurrentSpecies;
            }

            if (GenusString != null) {
                GenusString = GenusString.ToUpper();
                CurrentGenus = GenusString;
    
            }
            else {
                GenusString = CurrentGenus;
            }

            if (FamilyString != null) {
                FamilyString = FamilyString.ToUpper();
                CurrentFamily = FamilyString;
    
            }
            else {
                FamilyString = CurrentFamily;
            }
            


            if (!string.IsNullOrEmpty(SearchString))
            {
                taxaIQ = taxaIQ.Where(s => s.SpecificEpithet.ToUpper().Contains(SearchString));
            }
            if (!string.IsNullOrEmpty(FamilyString))
            {
                taxaIQ = taxaIQ.Where(s => s.Genus.FamilyID.ToUpper().Contains(FamilyString));
            }
            if (!string.IsNullOrEmpty(GenusString))
            {
                taxaIQ = taxaIQ.Where(s => s.GenusID.ToUpper().Contains(GenusString));
            }
            if (!string.IsNullOrEmpty(CategoryFilter))
            {
                taxaIQ = taxaIQ.Where(s => s.Genus.Family.CategoryID.Contains(CategoryFilter));
            }

            switch (sortOrder)
            {
                case "species_desc":
                    taxaIQ = taxaIQ.OrderByDescending(s => s.SpecificEpithet);
                    break;
                case "genus":
                    taxaIQ = taxaIQ.OrderBy(s => s.Genus);
                    break;
                case "genus_desc":
                    taxaIQ = taxaIQ.OrderByDescending(s => s.Genus);
                    break;
                default:
                    taxaIQ = taxaIQ.OrderBy(s => s.SpecificEpithet);
                    break;
            }
            var pageSize = Configuration.GetValue("PageSize", 10);
            Taxa = await PaginatedList<Models.Taxa>.CreateAsync(taxaIQ.AsNoTracking(), pageIndex ?? 1, pageSize);
        }

        public async Task<IActionResult> OnGetAsync(string sortOrder, int? pageIndex)
        {
            GenusSort = String.IsNullOrEmpty(sortOrder) ? "genus_desc" : "genus";
            SpeciesSort = String.IsNullOrEmpty(sortOrder) ? "species_desc" : "";
            CurrentSort = sortOrder;
            
            
            if (CategoryFilter != null)
            {
                CurrentCat = CategoryFilter;
            }

            CurrentSpecies = SearchString ?? CurrentSpecies;
            CurrentGenus = GenusString ?? CurrentGenus;
            CurrentFamily = FamilyString ?? CurrentFamily;

            Categories = new SelectList(_context.Categories, "Category", "Description");

            SelectList = await _context.Lists.ToListAsync();
            await LoadTaxaAsync(sortOrder, pageIndex);

            //StatusMessage = string.Empty;

            return Page();
        }

        public async Task<IActionResult> OnPostAddListAsync()
        {
            Console.WriteLine("Current page" + PageIndex);
            if (!SelectedListId.HasValue)
            {
                StatusMessage = "Please select a list.";
                
                return RedirectToPageWithContext(PageIndex);
            }

            if (string.IsNullOrEmpty(SelectedKewId))
            {
                StatusMessage = "Invalid plant selection.";
                return RedirectToPageWithContext(PageIndex);
            }

            var existingItem = await _context.ListItems
                .FirstOrDefaultAsync(l => l.KewID == SelectedKewId && l.ListID == SelectedListId.Value);

            if (existingItem != null)
            {
                StatusMessage = "This plant is already in that list.";
                return RedirectToPageWithContext(PageIndex);
            }

            var plant = await _context.Taxa.FirstOrDefaultAsync(t => t.KewID == SelectedKewId);
            var list = await _context.Lists.FirstOrDefaultAsync(l => l.Id == SelectedListId.Value);

            if (plant == null || list == null)
            {
                StatusMessage = "Plant or list not found.";
                return RedirectToPageWithContext(PageIndex);
            }

            var item = new Models.ListItems
            {
                KewID = SelectedKewId,
                ListID = SelectedListId.Value,
                TimeDiscovered = DateTime.Now,
                Plant = plant,
                List = list
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            StatusMessage = "Plant successfully added to list.";
            return RedirectToPageWithContext(PageIndex);
        }

        private IActionResult RedirectToPageWithContext(int? pageIndex)
        {
            // Create route values object with all current parameters
            var routeValues = new 
            { 
                sortOrder = CurrentSort,
                pageIndex = pageIndex ?? Taxa?.PageIndex ?? 1,
                SearchString = SearchString,
                FamilyString = FamilyString,
                GenusString = GenusString,
                CategoryFilter = CategoryFilter
            };

            return RedirectToPage("./Index", routeValues);
        }
        public JsonResult OnGetLikelyMatches(string query, int level) {
            if (level == 2){
                List<string> PotFamilies = [];
                foreach (var fam in _context.Families) {
                    if (fam.Family.Contains(query)) {
                        PotFamilies.Add(fam.Family);
                    }
                }
                List<string> cappedPotFamilies = PotFamilies.Take(10).ToList();
                return new JsonResult(cappedPotFamilies);
            } 
            if (level == 1) {
                List<string> PotGenera = new List<string>();
                foreach (var gen in _context.Genera) {
                    if (gen.GenusID.Contains(query)) {
                        PotGenera.Add(gen.GenusID);
                    }
                }
                List<string> cappedPotGenera = PotGenera.Take(10).ToList();
                return new JsonResult(cappedPotGenera);
            } 
            if (level == 0) {
                List<string> PotSpecies = new List<string>();
                foreach (var sp in _context.Taxa) {
                    if (sp.SpecificEpithet.Contains(query) && !PotSpecies.Contains(sp.SpecificEpithet)) {
                        PotSpecies.Add(sp.SpecificEpithet);
                    }
                }
                List<string> cappedPotSpecies = PotSpecies.Take(10).ToList();
                return new JsonResult(cappedPotSpecies);
            }
            List<string> nullResult = new List<string>();
            nullResult.Add("No results found");
            return new JsonResult(nullResult);
        }


    }
}