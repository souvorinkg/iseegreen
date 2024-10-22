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

        public int id {get; set;} = 1;

        public async Task<IActionResult> OnGetAsync(string KewID)
        {
            await MakeApiCall();

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

            return Page();
        }

        public async Task<IActionResult> OnPostAddList(string KewID, int? list) {
            await MakeApiCall();
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

            ListItems item;

            item = await _context.ListItems.SingleOrDefaultAsync(l => l.KewID == KewID && l.ListID == list.Value);

            if (item != null) {
                Error = "This plant is already in that list.";
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

            item = new ListItems {
                KewID = KewID,
                ListID = list.Value,
                TimeDiscovered = DateTime.Now
            };
            await MakeApiCall();

            await _context.AddAsync(item);
            await _context.SaveChangesAsync();

            return RedirectToPage("/ListItems/Index", new { itemid = list.ToString() });
        }


public async Task MakeApiCall()
{
    using (var client = new HttpClient())
    {
        try
        {
            string url = $"https://perenual.com/api/species-list?key=sk-il1O6717dcf920ca97383&q=Monstera deliciosa";

            // Make the GET request
            var response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();
            var responseData = await response.Content.ReadAsStringAsync();
            Console.WriteLine("Successfully got data");

            // Parse the JSON response
            var jsonObject = JObject.Parse(responseData);

            // Extract the "data" array
            var data = jsonObject["data"] as JArray;

            if (data != null && data.Count > 0)
            {
                // Get the first entry in the "data" array
                var firstEntry = data[0];

                // Extract the "id" field from the first entry
                var id = firstEntry["id"]?.ToString();
                
                // Print the extracted ID
                Console.WriteLine("First entry ID: " + id);
                if (id != null)
                {
                    url = $"https://perenual.com/api/species/details/5257?key=sk-il1O6717dcf920ca97383";
                    //url = $"https://perenual.com/api/species/details/{id}?key=sk-il1O6717dcf920ca97383";
                    response = await client.GetAsync(url);
                    response.EnsureSuccessStatusCode();
                    responseData = await response.Content.ReadAsStringAsync();
                    Console.WriteLine("Successfully got specific data");
                    
                    string description = GetDescription(responseData);
                    ViewData["description"] = description;
                    Console.WriteLine("Description: " + description);
                    Console.WriteLine("Successfully got description");

                }
                else
                {
                    Console.WriteLine("No ID found for the given query.");
                }
            }
            else
            {
                Console.WriteLine("No data found for the given query.");
            }
        }
        catch (HttpRequestException e)
        {
            Console.WriteLine($"Request error: {e.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}



        public string GetDescription(string responseData)
        {
            // Parse the JSON response data
            var jsonObject = JObject.Parse(responseData);

            // Extract the "description" field
            var description = jsonObject["description"]?.ToString();

            return description ?? "Description not available";
        }

        // private int getIDfromName(string name, response)
        // {
        //     string url = $"https://perenual.com/api/species-list?key=sk-il1O6717dcf920ca97383&q=Monstera deliciosa";
        //     var response = client.GetAsync(url);
        //     response.EnsureSuccessStatusCode();
        //     var responseData = await response.Content.ReadAsStringAsync();
        //     var jsonObject = JObject.Parse(responseData);
        //     int id = 1;
        //     id = (int)jsonObject[0]["id"];
        //     return id;
        // }
    }
}
