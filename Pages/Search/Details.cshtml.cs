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
            //await MakeApiCall();

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
            for (int i = 0; i < Taxon.Count; i++)
            {
                string genus = Taxon[i].GenusID;
                string species = Taxon[i].SpecificEpithet;
                Console.WriteLine("Genus: " + Taxon[i].GenusID);
                Console.WriteLine("Species: " + Taxon[i].SpecificEpithet);
                scientifcName = genus + " " + species.ToLower();
                Console.WriteLine("Scientific Name: " + scientifcName);
            }
            
            var client = new HttpClient();
            string id = await GetIDfromName(scientifcName, client);
            Console.WriteLine("ID in OnGet is: " + id);

            // Function that gets the Description from the ID
            //string description = await GetDescription(id, client);
            //Console.WriteLine("Description in OnGet is: " + description);
            //Taxon[0].Description = description;



            return Page();
        }

        private async Task<string> GetDescription(string id, HttpClient client)
        {
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
                        if (id != null)
                        {
                            string desc_url = $"https://perenual.com/api/species/details/{id}?key=sk-il1O6717dcf920ca97383";
                            var desc_response = await client.GetAsync(url);
                            desc_response.EnsureSuccessStatusCode();
                            var desc_responseData = await desc_response.Content.ReadAsStringAsync();
                            Console.WriteLine("Successfully got the data, may have description");
                            var desc_jsonObject = JObject.Parse(desc_responseData);
                            var description = desc_jsonObject["description"]?.ToString();

                            // Check if 'default_image' exists and extract the 'original_url'
                            if (desc_jsonObject["default_image"] is JObject imageObject)
                            {
                                string image_url = imageObject["original_url"]?.ToString() ?? "unknown"; // You can change this to "regular_url", "medium_url", etc.
                                if (image_url != null)
                                {
                                    Console.WriteLine("Image URL: " + image_url);
                                    ViewData["image"] = image_url;
                                }
                                else
                                {
                                    Console.WriteLine("No image URL found.");
                                }
                            }
                            else
                            {
                                Console.WriteLine("No image object found.");
                            }


                            //Console.WriteLine("Image URL: " + image_url);
                            ViewData["description"] = description;
                            Console.WriteLine("Description: " + description);
                            Console.WriteLine("Successfully got description");


                            return id;
                        }
                        else
                        {
                            Console.WriteLine("No ID found for the given query.");
                            return "Error";
                        }
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
                //Console.WriteLine("Identifier: " + Identifier);
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
