using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using csci340_iseegreen.Data;
using csci340_iseegreen.Models;
using Microsoft.AspNetCore.Components.Web;
using System.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Configuration;
using Microsoft.CodeAnalysis.Differencing;
using System.Drawing;


namespace csci340_iseegreen.Pages_Search
{
    public class DetailsModel : PageModel
    {
        private readonly csci340_iseegreen.Data.ISeeGreenContext _context;

        public DetailsModel(csci340_iseegreen.Data.ISeeGreenContext context)
        {
            _context = context;
        }

        public List<Taxa> Taxon {get; set;} = default!;

        public List<Lists> SelectList {get; set;} = default!;

        public string Identifier {get; set;} = default!;

        public string Error {get; set;} = default!;

        public int id {get; set;}

        public required string scientifcName {get; set;} = "";

        public async Task<IActionResult> OnGetAsync(string KewID)
        {
            IQueryable<csci340_iseegreen.Models.Taxa> taxaIQ = from t in _context.Taxa.Include(g => g.Genus).Include(f => f.Genus!.Family).Include(c => c.Genus!.Family!.Category) select t;

            taxaIQ = taxaIQ.Where(s => s.KewID.Equals(KewID));

            var taxon = await taxaIQ.ToListAsync();

            if (taxon == null) {
                return NotFound();
            }
            else {
                Taxon = taxon;
            }

            if (_context.Lists != null)
            {
                SelectList = await _context.Lists
                .ToListAsync();
            }
            Identifier = KewID;
            Taxa tree = Taxon[0];
            string scientifcName = tree.GenusID + " " + tree.SpecificEpithet.ToLower(); 
            
            string idString = "";
            if (tree.PereID != null) {
                if (tree.PereID == 0) {
                    ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";
                    ViewData["description"] = "Description not available";
                    return Page();
                }
                int id = tree.PereID ?? 0;
                idString = id.ToString();
            } 
            else {
                var client = new HttpClient();
                idString = await GetIDfromName(scientifcName, client);
            }

            if (idString == "Error")
            {
                ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";
                ViewData["description"] = "Description not available";
                tree.PereID = 0;
                await _context.SaveChangesAsync();
                return Page();
            }
            else {
                tree.PereID = int.Parse(idString);
                await _context.SaveChangesAsync();
            }

            if (tree.Description != null) {
                ViewData["description"] = tree.Description;
            } else {
                await GetDescription(idString, tree);
            } 

            if (tree.url != null) {
                ViewData["image"] = tree.url;
            } else {
                await GetImage(idString, tree);
            }
            await _context.SaveChangesAsync();

            return Page();
        }

        private async Task<string> GetImage(string id, Taxa tree)
        {
            var client = new HttpClient();
            using (client)
            {
                try
                {
                    string url = $"https://perenual.com/api/species/details/{id}?key=sk-il1O6717dcf920ca97383";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successfully got the data, may have description");
                    var jsonObject = JObject.Parse(responseData);
                    if (jsonObject["default_image"] is JObject imageObject)
                    {
                        string image_url = imageObject["original_url"]?.ToString() ?? "unknown"; // You can change this to "regular_url", "medium_url", etc.
                        if (image_url != null)
                        {
                            Console.WriteLine("Image URL: " + image_url);
                            ViewData["image"] = image_url;
                            tree.url = image_url;
                        }
                        else
                        {
                            ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";
                            Console.WriteLine("No image URL found.");
                        }
                    }
                    else
                    {
                        ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";

                        Console.WriteLine("No image object found.");
                    }
                    return "Image not available";
                }
                catch (HttpRequestException e)
                {
                    ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";

                    Console.WriteLine($"Request error: {e.Message}");
                    return "Error";
                }
                catch (Exception ex)
                {
                    ViewData["image"] = "https://perenual.com/storage/species_image/6489_quercus_alba/og/51276992180_5212077a9b_b.jpg";

                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return "Error";
                }
            }
        }

        private async Task<string> GetDescription(string id, Taxa tree)
        {
            var client = new HttpClient();
            using (client)
            {
                try
                {
                    string url = $"https://perenual.com/api/species/details/{id}?key=sk-il1O6717dcf920ca97383";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successfully got the data, may have description");
                    var jsonObject = JObject.Parse(responseData);
                    var description = jsonObject["description"]?.ToString();
                    ViewData["description"] = description;
                    if (description != null)
                    {
                        Console.WriteLine("Description: " + description);
                        tree.Description = description;
                    }
                    else
                    {
                        ViewData["description"] = "Description not available";
                        Console.WriteLine("No description found.");
                    }
                    Console.WriteLine("Description: " + description);
                    Console.WriteLine("Successfully got description");
                    return description ?? "Description not available";
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return "Error";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return "Error";
                }
            }
        }

        public async Task<string> GetIDfromName(string name, HttpClient client)
        {
            using (client)
            {
                try
                {
                    string url = $"https://perenual.com/api/species-list?key=sk-il1O6717dcf920ca97383&q={name}";
                    var response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    var responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successfully got data from name");
                    var jsonObject = JObject.Parse(responseData);
                    var data = jsonObject["data"] as JArray;
                    if (data != null && data.Count > 0)
                    {
                        var firstEntry = data[0];
                        var id = firstEntry["id"]?.ToString();
                        Console.WriteLine("First entry ID: " + id);
                        if (id != null) return id; else return "Error";
                    }
                    else
                    {
                        Console.WriteLine("No data found for the given query.");
                        return "Error";
                    }
                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine($"Request error: {e.Message}");
                    return "Error";
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                    return "Error";
                }
            }
        }

        public async Task<IActionResult> OnPostAddList(string KewID, int? list) {
            if (list == null) {
                Error = "You have to select a list.";
                IQueryable<csci340_iseegreen.Models.Taxa> taxaIQ = from t in _context.Taxa.Include(g => g.Genus).Include(f => f.Genus!.Family).Include(c => c.Genus!.Family!.Category) select t;

                taxaIQ = taxaIQ.Where(s => s.KewID.Equals(KewID));

                var taxon = await taxaIQ.ToListAsync();

                if (taxon == null)
                {
                    return NotFound();
                }
                else
                {
                    Taxon = taxon;
                }

                if (_context.Lists != null)
                {
                    SelectList = await _context.Lists
                    .ToListAsync();
                }
                Identifier = KewID;
                return Page();
            } 

            ListItems? item = null;

            if (item == null)
            {
                item = await _context.ListItems.SingleOrDefaultAsync(l => l.KewID == KewID && l.ListID == list.Value);
            }

            if (item != null)
            {
                Error = "This plant is already in that list.";

                IQueryable<csci340_iseegreen.Models.Taxa> taxaIQ = _context.Taxa
                    .Include(g => g.Genus)
                    .Include(f => f.Genus!.Family)
                    .Include(c => c.Genus!.Family!.Category)
                    .Where(s => s.KewID.Equals(KewID));

                var taxon = await taxaIQ.ToListAsync();

                if (taxon == null)
                {
                    return NotFound();
                }
                else
                {
                    Taxon = taxon;
                }

                if (_context.Lists != null)
                {
                    SelectList = await _context.Lists.ToListAsync();
                }

                Identifier = KewID;
                return Page();
            }

            item = new ListItems
            {
                KewID = KewID,
                ListID = list.Value,
                TimeDiscovered = DateTime.Now,
                Plant = await _context.Taxa.SingleOrDefaultAsync(t => t.KewID == KewID) ?? throw new InvalidOperationException("Plant not found."),
                List = await _context.Lists.SingleOrDefaultAsync(l => l.Id == list.Value) ?? throw new InvalidOperationException("List not found.")
            };

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListItems/Index", new { itemid = list.ToString() });
        }
    }
}
