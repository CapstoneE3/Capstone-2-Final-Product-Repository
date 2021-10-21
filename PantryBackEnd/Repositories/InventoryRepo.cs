using PantryBackEnd.Models;
using System.Collections.Generic;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
namespace PantryBackEnd.Repositories
{

    public class InventoryRepo : IInventoryRepo
    {
        private pantryContext context;
        public InventoryRepo(pantryContext context)
        {
            this.context = context;
        }
        public void AddProduct(InventoryList product)
        {
            context.InventoryLists.Add(product);
            context.SaveChanges();
        }
        public void updateItem()
        {
            context.SaveChanges();
        }
        public Dictionary<string, object> GetInventoryList(Guid acc_id)
        {
            int listCount = 0;
            List<FrontEndInventoryFormat> formats;
            Dictionary<string, object> invList = new Dictionary<string, object>();
            /*string[] category = {"Kids & Lunch Box","Entertaining At Home","Bakery","Fruit & Vegetables",
                                "Meat & Seafood","From The Deli","Dairy, Eggs & Meals","Conveniece Meals",
                                "Pantry","Frozen","Drinks","International Foods","Household","Health & Beauty",
                                "Baby","Pet","Liquor","Tobacco","Sugar & Sweeteners"};
            */
            List<String> category = new List<string>();
            List<Category> cats = context.Categories.ToList();
            
            foreach(Category c in cats)
            {
                category.Add(c.Category1);
            }

            foreach (string i in category)
            {
                List<Product> prods = context.Products.Where(a => a.Category.Equals(i)).Include(b => b.InventoryLists.Where(c => c.AccId == acc_id)).ToList();
                if (prods.Count > 0)
                {
                    formats = new List<FrontEndInventoryFormat>();
                    foreach (Product c in prods)
                    {

                        if (c.InventoryLists.Count != 0)
                        {
                            listCount = 0;
                            FrontEndInventoryFormat b = new FrontEndInventoryFormat();
                            foreach (InventoryList a in c.InventoryLists)
                            {
                                ExpDateAndPrice eap = new ExpDateAndPrice
                                {
                                    expDate = a.ExpDate,
                                    count = (int)a.Count
                                };
                                if (listCount == 0)
                                {
                                    if(c.Price != null)
                                    {
                                        b.itemID = a.ItemId;
                                        b.name = c.Name;
                                        b.price = (decimal)c.Price;
                                        b.quantity = c.Quantity;
                                        b.Expiry_Count.Add(eap);
                                    }
                                    else
                                    {
                                        b.itemID = a.ItemId;
                                        b.name = c.Name;
                                        b.quantity = c.Quantity;
                                        b.Expiry_Count.Add(eap);
                                    }
                                }
                                else
                                {
                                    b.Expiry_Count.Add(eap);
                                }
                                listCount++;


                            }
                            formats.Add(b);
                        }
                    }
                    invList.Add(i, formats);
                }
                else
                {
                    invList.Add(i, new List<FrontEndInventoryFormat>());
                }
            }
            return invList;
        }


        public string removeProduct(Guid acc_id, string item_id, int count, DateTime exp)
        {
            List<InventoryList> invlist = context.InventoryLists.ToList();
            foreach(InventoryList il in invlist)
            {
                if(il.AccId == acc_id && il.ItemId == item_id && il.ExpDate == exp)
                {
                    if(count == il.Count)
                    {
                        //invlist.Remove(il);
                        context.Remove(context.InventoryLists.Single(a => a.ItemId.Equals(item_id) && a.AccId == acc_id && exp == a.ExpDate));
                        break;
                    }
                    else if(count < il.Count)
                    {
                        il.Count -= count;
                        break;
                    }
                    else
                    {
                        return "failed";
                    }
                    
                }
            }
            //if (context.InventoryLists.Where(a => a.ItemId == item_id && a.AccId.Equals(acc_id) ) != null)
            //{
                
                //context.Remove(context.InventoryLists.Where(a => a.ItemId == item_id && a.AccId.Equals(acc_id)));
                //context.Remove(context.InventoryLists.Single(a => a.AccId == acc_id && a.ItemId.Equals(item_id)));               
                context.SaveChanges();
            //}
            return "success";
        }
        public async Task randomRecipe()
        {
             var client = new HttpClient();
             dynamic details;
             var request = new HttpRequestMessage
             {
                 Method = HttpMethod.Get,
                 RequestUri = new Uri("https://spoonacular-recipe-food-nutrition-v1.p.rapidapi.com/recipes/random?number=10000"),
                 Headers =
                 {
                     { "x-rapidapi-host", "spoonacular-recipe-food-nutrition-v1.p.rapidapi.com" },
                     { "x-rapidapi-key", "50140e856emsh7d1de4f17f3e37ep1db084jsnaf474ebb6158" },
                 },
             };
            using (var response = await client.SendAsync(request))
            {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    details = JsonConvert.DeserializeObject<dynamic>(body);
            }
                    await recipeFormat(details);
        }
    
        public Task recipeFormat(dynamic details)
        {
            IList<Recipe> list = new List<Recipe>();
            IList<Ingredient> ingre = new List<Ingredient>();
            List<string> category = context.Categories.AsNoTracking().Select(p => p.Category1).ToList();
            List<Category> categoryAdd = new List<Category>();
            List<int> ingIDs = context.Ingredients.AsNoTracking().Select(d => d.IngredientId).ToList();
            foreach(dynamic a in details.recipes)
            {
                int recid = Convert.ToInt32(a.id);
                if(context.Recipes.Any( b => b.RecipeId == recid )== false && a.image != null)
                {
                    
                    Recipe rec = new Recipe();
                            
                    rec.RecipeId = Convert.ToInt32(a.id);
    
                    rec.RecipeName = a.title;
                    rec.RecipeDescription = a.summary;

                    RecipeDocument recDoc = new RecipeDocument();
                    recDoc.Url = a.sourceUrl;
                    recDoc.RecipeId = Convert.ToInt32(a.id);
                    recDoc.PhotoUrl = a.image;
                    rec.RecipeDocument = recDoc;


                    int count = 0;
                    ICollection<RecipeStep> recipeStep = new HashSet<RecipeStep>();
                    foreach(var b in a.analyzedInstructions)
                    {
                        foreach(var instruction in b.steps)
                        {
                            RecipeStep recStep = new RecipeStep
                            {
                                RecipeId = Convert.ToInt32(a.id),
                                StepId = count,
                                Instructions = instruction.step
                            };
                            count++;
                            recipeStep.Add(recStep);
                        }
                    }
        
                    rec.RecipeSteps = recipeStep;
                    Boolean addBool = true;            
                    ICollection<RecipeIngredient> RecipeIngredients = new HashSet<RecipeIngredient>();
                    foreach (var item in a.extendedIngredients)
                    {
                        
                        try
                        {   
                            addBool = true;
                            RecipeIngredient ingredient = new RecipeIngredient{
                                RecipeId = Convert.ToInt32(a.id),
                                IngredientId = Convert.ToInt32(item.id),
                                Amount = Convert.ToSingle(item.amount),
                                UnitOfMeasure = item.unit,
                                Name = item.name,
                                OriginalName = item.originalString
                            };
                            RecipeIngredients.Add(ingredient);
                            
                            String cat = item.aisle;
                            String[] ory = cat.Split(';');
                            int K = Convert.ToInt32(item.id);
                            
                            Ingredient ing = new Ingredient{
                                IngredientId = K,
                                Name = item.name,
                                Category = ory[0]
                            };
        

                            Category cate = new Category{
                                Category1 = ory[0]
                            };                

                            
                            if(cate.Category1==null)
                            {
                                addBool=false;
                                continue;
                            }
                            
                            if(category.Contains(cate.Category1) == false)
                            {
                                category.Add(cate.Category1);
                                categoryAdd.Add(cate);
                            }
                            /*
                            if((category.Contains(cate.Category1) == false) && (ingIDs.Contains(K) == false) && cate.Category1 != null)
                            {
                                category.Add(cate.Category1);
                                categoryAdd.Add(cate);
                                ingIDs.Add(K);
                                ingre.Add(ing);
                            }
                            else
                            {
                                addBool = false;
                            }*/

                            if(ingIDs.Contains(K) == false)
                            {
                                ingIDs.Add(K);
                                ingre.Add(ing);
                            }
                            
                        }
                        catch(Exception)
                        {
                            //unlucky
                        }

                    }
                    if(addBool == false)
                    {
                        continue;
                    }                    
                    rec.RecipeIngredients = RecipeIngredients;
                    if(addBool == true)
                    {
                        list.Add(rec);

                    }

                }
                         
            }
                context.Categories.AddRange(categoryAdd);
                context.Ingredients.AddRange(ingre);
                context.Recipes.AddRange(list);
                context.SaveChanges();

            return Task.CompletedTask;
        }
    }
    
}