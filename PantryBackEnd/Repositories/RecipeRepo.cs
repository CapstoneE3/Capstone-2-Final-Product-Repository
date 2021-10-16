using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PantryBackEnd.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace PantryBackEnd.Repositories
{
    public class RecipeRepo : IRecipe
    {
        private pantryContext context;
        public RecipeRepo(pantryContext context)
        {
            this.context = context;
        }

        public async void randomRecipe()
        {
            var client = new HttpClient();
            dynamic details;
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/recipes/random?number=20"),
                Headers =
                {
                    { "x-rapidapi-host", "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com" },
                    { "x-rapidapi-key", "50140e856emsh7d1de4f17f3e37ep1db084jsnaf474ebb6158" },
                },
            };
            using (var response = await client.SendAsync(request))
            {
                string json = @"[
  {
    'Title': 'Json.NET is awesome!',
    'Author': {
      'Name': 'James Newton-King',
      'Twitter': '@JamesNK',
      'Picture': '/jamesnk.png'
    },
    'Date': '2013-01-23T19:30:00',
    'BodyHtml': '&lt;h3&gt;Title!&lt;/h3&gt;\r\n&lt;p&gt;Content!&lt;/p&gt;'
  }
]";
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                details = JsonConvert.DeserializeObject<dynamic>(body);
                Console.WriteLine(body);
            }
            recipeFormat(details);
        }
    
        public void recipeFormat(dynamic details)
        {
            IList<Recipe> list = new List<Recipe>(); 
            foreach(dynamic a in details.recipes)
            {
                Recipe rec = new Recipe();

                
                rec.RecipeId = Convert.ToInt32(a.id);
                
                
                rec.RecipeName = a.title;
                rec.RecipeDescription = a.summary;
                //context.Recipes.Add(rec);
                
                RecipeDocument recDoc = new RecipeDocument();
                recDoc.Url = a.sourceUrl;
                recDoc.RecipeId = Convert.ToInt32(a.id);
                //context.RecipeDocuments.Add(recDoc);
                rec.RecipeDocument = recDoc;


                ICollection<RecipeStep> recipeStep = new HashSet<RecipeStep>();
                foreach(var b in a.analyzedInstructions)
                {
                    foreach(var instruction in b.steps)
                    {
                        RecipeStep recStep = new RecipeStep
                        {
                            RecipeId = Convert.ToInt32(a.id),
                            StepId = instruction.number,
                            Instructions = instruction.step
                        };
                        recipeStep.Add(recStep);
                    }
                }
                // context.RecipeSteps.AddRange(recipeStep);
    
                rec.RecipeSteps = recipeStep;
                               
                ICollection<RecipeIngredient> RecipeIngredients = new HashSet<RecipeIngredient>();
                foreach (var item in a.extendedIngredients)
                {
                    RecipeIngredient ingredient = new RecipeIngredient{
                        RecipeId = Convert.ToInt32(a.id),
                        IngredientId = item.ID,
                        Amount = item.amount,
                        UnitOfMeasure = item.unit
                    };
                    RecipeIngredients.Add(ingredient);
                }
                //context.RecipeIngredients.AddRange(RecipeIngredients);
                //context.SaveChanges();
                rec.RecipeIngredients = RecipeIngredients;
                list.Add(rec);
            }
            context.Recipes.AddRange(list);
        }

    }
}