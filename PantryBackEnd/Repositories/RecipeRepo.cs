using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using PantryBackEnd.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;

namespace PantryBackEnd.Repositories
{
    public class RecipeRepo : IRecipe
    {
        private pantryContext context;
        public RecipeRepo(pantryContext context)
        {
            this.context = context;
        }
        public List<AllIngredients> AllIngridient()
        {
            var ok = context.Ingredients.Join(context.RecipeIngredients, ingredients => ingredients.IngredientId,
             RecipeIngredient => RecipeIngredient.IngredientId,
             (ingredients, RecipeIngredient) => new AllIngredients
             {
                 ids = ingredients.IngredientId,
                 name = ingredients.Name,
                 UnitOfMeasure = RecipeIngredient.UnitOfMeasure
             }).Distinct().ToList();
            return ok;
        }

        public async Task RemoveRecipe(int id)
        {
            Recipe d = await context.Recipes.Where(a => a.RecipeId == id).Include(b => b.RecipeLists).Include(c => c.RecipeSteps).Include(d => d.RecipeIngredients).Include(e => e.RecipeDocument).SingleAsync();
            context.Recipes.Remove(d);
            await context.SaveChangesAsync();
        }
        public List<Recipe> calculateRecipeScores(Guid accountId)
        {
            Console.WriteLine("Account ID is: " + accountId);

            List<Recipe> recipeImportList = context.Recipes.Include(a => a.RecipeIngredients).Include(d => d.RecipeDocument).Take(30).ToList();
            // Should give us a list of 30 random recipes 

            List<InventoryList> inventory = context.InventoryLists.Where(a => a.AccId == accountId).Include(c => c.Item).ToList();

            //List<FrontEndInventoryFormat> userInventory = (List<FrontEndInventoryFormat>) inventoryUser;
            // List<InventoryList> inventoryUser = InventoryRepo.GetInventoryList(accountId);
            // CHECK THIS FUNCTION CALL!!!

            int onePriority = 1;
            // "Alcoholic Beverages""Beverages""Cereal""Dried Fruits""Ethnic Foods""Gourmet""Savory Snacks"
            int twoPriority = 2;
            // "Condiments""Health Foods""Nut butters, Jams, and Honey""Nuts""Sweet Snacks""Tea and Coffee"
            int threePriority = 3;
            // "Canned and Jarred""Cheese""Frozen""Gluten Free""Oil, Vinegar, Salad Dressing"
            // "Refrigerated""Spices and Seasonings"
            int fourPriority = 5;
            // "Bakery/Bread""Baking""Bread""Pasta and Rice"
            int fivePriority = 7;
            // "Meat""Milk, Eggs, Other Dairy""Produce""Seafood"


            //int[] allRecipeScores = null; 
            List<int> allRecipeScores = new List<int>();

            //List<Recipe> goodRecipes = null; 
            List<Recipe> goodRecipes = new List<Recipe>();

            for (int ii = 0; ii < recipeImportList.Count(); ii++)
            {
                // CHECK IF ITS .SIZE() OR .LENGTH() FOR ARRAYS

                int totalRecipeScore = 0;

                Console.WriteLine(recipeImportList[ii].RecipeId);

                List<RecipeIngredient> ingredientsForRecipe = recipeImportList[ii].RecipeIngredients.ToList();
                // CHECK THIS FUNCTION CALL 

                for (int jj = 0; jj < ingredientsForRecipe.Count(); jj++)
                {
                    //Console.WriteLine(jj);
                    RecipeIngredient currentIngredient = ingredientsForRecipe[jj];


                    for (int kk = 0; kk < inventory.Count(); kk++)

                        // we need to loop through all of the products in the users inventory using the kk for-loop above 
                        // On each iteration, we want to grab the kk-th product and assign it to 'prod' in the call below. 

                        //Product prod = inventoryUser[kk]
                        // CHECK THIS FUNCTION CALL 



                        if (currentIngredient.IngredientId == inventory[kk].Item.IngredientId)
                        {
                            string prodCategory = inventory[kk].Item.Category;

                            if ((prodCategory.Equals("Alcoholic Beverages")) || (prodCategory.Equals("Beverages"))
                            || (prodCategory.Equals("Cereal")) || (prodCategory.Equals("Dried Fruits")) || (prodCategory.Equals("Ethnic Foods"))
                            || (prodCategory.Equals("Gourmet")) || (prodCategory.Equals("Savory Snacks")))
                            {

                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (onePriority * 2);
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + onePriority;
                                }

                            }
                            else if ((prodCategory.Equals("Condiments"))
                            || (prodCategory.Equals("Health Foods")) || (prodCategory.Equals("Nut butters, Jams, and Honey"))
                            || (prodCategory.Equals("Nuts")) || (prodCategory.Equals("Sweet Snacks"))
                            || (prodCategory.Equals("Tea and Coffee")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (twoPriority * 2);
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + twoPriority;
                                }
                            }
                            else if ((prodCategory.Equals("Frozen")) || (prodCategory.Equals("Cheese")) ||
                             (prodCategory.Equals("Canned and Jarred")) || (prodCategory.Equals("Refrigerated")) ||
                             (prodCategory.Equals("Spices and Seasonings")) || (prodCategory.Equals("Gluten Free")) ||
                             (prodCategory.Equals("Oil, Vinegar, Salad Dressing")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (threePriority * 2);
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + threePriority;
                                }
                            }
                            else if ((prodCategory.Equals("Bakery/Bread")) || (prodCategory.Equals("Baking")) ||
                             (prodCategory.Equals("Bread")) || (prodCategory.Equals("Pasta and Rice")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (fourPriority * 2);
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + fourPriority;
                                }
                            }
                            else if ((prodCategory.Equals("Meat")) || (prodCategory.Equals("Milk,Eggs,Other Dairy")) ||
                             (prodCategory.Equals("Produce")) || (prodCategory.Equals("Seafood")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (fivePriority * 2);
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + fivePriority;
                                }
                            }

                        }


                }

                allRecipeScores.Add(totalRecipeScore);

            }

            for (int ii = 0; ii < allRecipeScores.Count(); ii++)
            {
                Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores[ii]);
            }
            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 

            int recipeOneScore = allRecipeScores.Max(); // gives a score 
            int recipeOneIndex = allRecipeScores.ToList().IndexOf(recipeOneScore);
            goodRecipes.Add(recipeImportList[recipeOneIndex]);
            allRecipeScores.RemoveAt(recipeOneIndex);
            recipeImportList.RemoveAt(recipeOneIndex);
            // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
            // because we're about to store it within our goodRecipes list 

            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 

            int recipeTwoScore = allRecipeScores.Max(); // gives a score 
            int recipeTwoIndex = allRecipeScores.ToList().IndexOf(recipeTwoScore);
            goodRecipes.Add(recipeImportList[recipeTwoIndex]);
            allRecipeScores.RemoveAt(recipeTwoIndex);
            recipeImportList.RemoveAt(recipeTwoIndex);
            // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
            // because we're about to store it within our goodRecipes list 

            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 

            int recipeThreeScore = allRecipeScores.Max(); // gives a score 
            int recipeThreeIndex = allRecipeScores.ToList().IndexOf(recipeThreeScore);
            goodRecipes.Add(recipeImportList[recipeThreeIndex]);
            allRecipeScores.RemoveAt(recipeThreeIndex);
            recipeImportList.RemoveAt(recipeThreeIndex);
            // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
            // because we're about to store it within our goodRecipes list 

            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 

            int recipeFourScore = allRecipeScores.Max(); // gives a score 
            int recipeFourIndex = allRecipeScores.ToList().IndexOf(recipeFourScore);
            goodRecipes.Add(recipeImportList[recipeFourIndex]);
            allRecipeScores.RemoveAt(recipeFourIndex);
            recipeImportList.RemoveAt(recipeFourIndex);
            // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
            // because we're about to store it within our goodRecipes list

            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 

            int recipeFiveScore = allRecipeScores.Max(); // gives a score 
            int recipeFiveIndex = allRecipeScores.ToList().IndexOf(recipeFiveScore);
            goodRecipes.Add(recipeImportList[recipeFiveIndex]);
            allRecipeScores.RemoveAt(recipeFiveIndex);
            recipeImportList.RemoveAt(recipeFiveIndex);
            // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
            // because we're about to store it within our goodRecipes list

            //Console.WriteLine("Here are all " + allRecipeScores.Count() + " recipe scores: " + allRecipeScores); 


            return goodRecipes;

        }

        public List<frontEndRecipeDisplayAll> displayRecommendation(List<Recipe> recs)
        {
            List<frontEndRecipeDisplayAll> recipes = new List<frontEndRecipeDisplayAll>();
            foreach (Recipe a in recs)
            {
                frontEndRecipeDisplayAll single = new frontEndRecipeDisplayAll
                {
                    RecipeId = a.RecipeId,
                    RecipeName = a.RecipeName,
                    PhotoUrl = null
                };
                if (a.RecipeDocument != null)
                {
                    single.PhotoUrl = a.RecipeDocument.PhotoUrl;
                }
                List<ingredients> ings = new List<ingredients>();
                foreach (RecipeIngredient b in a.RecipeIngredients)
                {
                    ingredients ing = new ingredients
                    {
                        ids = b.IngredientId,
                        name = b.Name
                    };
                    if (b.LinkProd != null)
                    {
                        ing.linkProd = b.LinkProd;
                    }
                    ings.Add(ing);
                }
                single.ingredientsList = ings;
                recipes.Add(single);
            }

            return recipes;
        }

        public List<frontEndRecipeDisplayAll> browseApiRecipes(int index, Guid id)
        {
            List<Recipe> recipes = context.Recipes.AsNoTracking().Where(d => d.RecipeLists.Any(j => j.AccId == j.AccId) == false).Include(a => a.RecipeDocument).Include(b => b.RecipeIngredients).
            OrderBy(c => c.RecipeId).Skip(index).Take(20).ToList();

            List<frontEndRecipeDisplayAll> returnObj = new List<frontEndRecipeDisplayAll>();

            //return recipes;

            foreach (Recipe a in recipes)
            {
                List<ingredients> ings = new List<ingredients>();

                foreach (RecipeIngredient b in a.RecipeIngredients)
                {
                    ingredients ing = new ingredients
                    {
                        ids = b.IngredientId,
                        name = b.Name
                    };
                    if (b.LinkProd != null)
                    {
                        ing.linkProd = b.LinkProd;
                    }
                    ings.Add(ing);
                }

                if (a.RecipeDocument == null)
                {
                    frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                    {
                        RecipeId = a.RecipeId,
                        RecipeName = a.RecipeName,
                        ingredientsList = ings,
                    };
                    returnObj.Add(newObj);
                }
                else
                {

                    frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                    {
                        RecipeId = a.RecipeId,
                        RecipeName = a.RecipeName,
                        ingredientsList = ings,
                        PhotoUrl = a.RecipeDocument.PhotoUrl,
                        url = a.RecipeDocument.Url
                    };
                    returnObj.Add(newObj);
                }
            }

            return returnObj;
        }

        public Task<List<frontEndRecipeDisplayAll>> Search(string value)
        {
            var t = Task.Run(() =>
            {
                List<Recipe> recipes = context.Recipes.AsNoTracking().Where(f => f.RecipeName.IndexOf(value, StringComparison.OrdinalIgnoreCase) >= 0).Where(d => d.RecipeLists.Any(j => j.AccId == j.AccId) == false).Include(a => a.RecipeDocument).Include(b => b.RecipeIngredients).
            OrderBy(c => c.RecipeId).ToList();

                List<frontEndRecipeDisplayAll> returnObj = new List<frontEndRecipeDisplayAll>();

                //return recipes;

                foreach (Recipe a in recipes)
                {
                    List<ingredients> ings = new List<ingredients>();

                    foreach (RecipeIngredient b in a.RecipeIngredients)
                    {
                        ingredients ing = new ingredients
                        {
                            ids = b.IngredientId,
                            name = b.Name
                        };
                        if (b.LinkProd != null)
                        {
                            ing.linkProd = b.LinkProd;
                        }
                        ings.Add(ing);
                    }

                    if (a.RecipeDocument == null)
                    {
                        frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                        {
                            RecipeId = a.RecipeId,
                            RecipeName = a.RecipeName,
                            ingredientsList = ings,
                        };
                        returnObj.Add(newObj);
                    }
                    else
                    {

                        frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                        {
                            RecipeId = a.RecipeId,
                            RecipeName = a.RecipeName,
                            ingredientsList = ings,
                            PhotoUrl = a.RecipeDocument.PhotoUrl,
                            url = a.RecipeDocument.Url
                        };
                        returnObj.Add(newObj);
                    }
                }
                return returnObj;
            }
            );
            return t;

        }
        public List<frontEndRecipeDisplayAll> getRecipes(Guid id, int index)
        {
            //List<Recipe> recipes = context.Recipes.AsNoTracking().Include(a => a.RecipeDocument).Include(b => b.RecipeLists.
            //Where(c => c.AccId == id)).Include(d => d.RecipeIngredients).OrderBy(e => e.RecipeId).Skip(index).Take(20).ToList();

            List<RecipeList> recipeLists = context.RecipeLists.AsNoTracking().Where(x => x.AccId == id).Include(a => a.Recipe.RecipeDocument).
            Include(b => b.Recipe.RecipeIngredients).OrderBy(e => e.RecipeId).Skip(index).Take(20).ToList();

            List<frontEndRecipeDisplayAll> returnObj = new List<frontEndRecipeDisplayAll>();



            //return recipes;

            foreach (RecipeList a in recipeLists)
            {
                List<ingredients> ings = new List<ingredients>();

                foreach (RecipeIngredient b in a.Recipe.RecipeIngredients)
                {
                    ingredients ing = new ingredients
                    {
                        ids = b.IngredientId,
                        name = b.Name
                    };
                    if (b.LinkProd != null)
                    {
                        ing.linkProd = b.LinkProd;
                    }
                    ings.Add(ing);
                }

                if (a.Recipe.RecipeDocument == null)
                {
                    frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                    {
                        RecipeId = a.RecipeId,
                        RecipeName = a.Recipe.RecipeName,
                        ingredientsList = ings,
                    };
                    returnObj.Add(newObj);
                }
                else
                {

                    frontEndRecipeDisplayAll newObj = new frontEndRecipeDisplayAll
                    {
                        RecipeId = a.RecipeId,
                        RecipeName = a.Recipe.RecipeName,
                        ingredientsList = ings,
                        PhotoUrl = a.Recipe.RecipeDocument.PhotoUrl
                    };
                    returnObj.Add(newObj);
                }
            }

            return returnObj;
        }

        public frontEndRecipeClickDetails getRecipeInfo(Guid id, int recipeId)
        {
            frontEndRecipeClickDetails aha = new frontEndRecipeClickDetails();
            return aha;
        }


        public async Task<CustomRecipe> convertToCustom(int recipeID)
        {
            try
            {
                Recipe recipes = await context.Recipes.Where(a => a.RecipeId == recipeID).Include(d => d.RecipeIngredients).Include(e => e.RecipeSteps)
                .Include(b => b.RecipeDocument).AsNoTracking().SingleAsync();
                if (recipes.RecipeDocument == null)
                {
                    return null;
                }
                else if (recipes.RecipeDocument.Url == null)
                {
                    return null;
                }
                //.Where(c => c.PhotoUrl != null)
                List<string> str = new List<string>();
                foreach (RecipeStep z in recipes.RecipeSteps)
                {
                    str.Add(z.Instructions);
                }

                List<customIngredients> custIng = new List<customIngredients>();
                foreach (RecipeIngredient x in recipes.RecipeIngredients)
                {
                    customIngredients ing = new customIngredients
                    {
                        amount = (double)x.Amount,
                        unitOfMeasure = x.UnitOfMeasure,
                        ingredientName = x.Name,
                        ingredientId = x.IngredientId
                    };
                    if (x.LinkProd != null)
                    {
                        ing.linkProduct = x.LinkProd;
                    }
                    custIng.Add(ing);
                }

                CustomRecipe cusRec = new CustomRecipe
                {
                    recipeName = "Custom " + recipes.RecipeName,
                    desc = recipes.RecipeDescription,
                    steps = str,
                    ingredientsList = custIng,
                    photoUrl = recipes.RecipeDocument.PhotoUrl,
                    url = recipes.RecipeDocument.Url
                };

                return cusRec;

            }
            catch (InvalidOperationException)
            {
                return null;
            }
        }
        public async Task<string> createRecipe(Guid id, CustomRecipe obj)
        {
            //CustomRecipe obj
            //string recipeName, string recipeDesc, List<string> steps
            var recID = "";
            try
            {
                string recipeName = obj.recipeName;
                List<customIngredients> ingredients = obj.ingredientsList;

                Recipe recipes = await context.Recipes.AsNoTracking().Where(a => a.RecipeName.Equals(recipeName)).Include(b => b.RecipeLists.Where(c => c.AccId == id)).SingleAsync();
                return "Exists";
            }
            catch (Exception)
            {
                try
                {
                    string recipeName = obj.recipeName;
                    List<string> steps = obj.steps;
                    string recipeDesc = obj.desc;
                    List<customIngredients> ingredients = obj.ingredientsList;

                    int random;
                    do
                    {
                        Random rand = new Random();
                        random = rand.Next(10000);
                        recID = random.ToString();

                    } while (context.Recipes.Where(a => a.RecipeId == random) == null);

                    ICollection<RecipeStep> recStep = new HashSet<RecipeStep>();

                    for (int i = 0; i < steps.Count(); i++)
                    {
                        RecipeStep ok = new RecipeStep
                        {
                            StepId = i,
                            Instructions = steps[i],
                            RecipeId = random
                        };
                        recStep.Add(ok);
                    }

                    ICollection<RecipeIngredient> recIng = new HashSet<RecipeIngredient>();

                    foreach (customIngredients a in ingredients)
                    {
                        RecipeIngredient ing = new RecipeIngredient
                        {
                            RecipeId = random,
                            IngredientId = a.ingredientId,
                            Amount = (float)a.amount,
                            UnitOfMeasure = a.unitOfMeasure,
                            Name = a.ingredientName,
                            OriginalName = a.amount.ToString() + a.unitOfMeasure + a.ingredientName,
                            LinkProd = a.linkProduct
                        };

                        if (a.linkProduct != null)
                        {
                            ing.LinkProd = a.linkProduct;
                        }
                        recIng.Add(ing);
                    }

                    Recipe rec;
                    if (obj.photoUrl == null)
                    {
                        rec = new Recipe
                        {
                            RecipeId = random,
                            RecipeName = recipeName,
                            RecipeDescription = recipeDesc,
                            RecipeSteps = recStep,
                            RecipeDocument = null,
                            RecipeIngredients = recIng
                        };
                    }
                    else if (obj.photoUrl != null && obj.url == null)
                    {
                        rec = new Recipe
                        {
                            RecipeId = random,
                            RecipeName = recipeName,
                            RecipeDescription = recipeDesc,
                            RecipeSteps = recStep,
                            RecipeDocument = new RecipeDocument { PhotoUrl = obj.photoUrl, RecipeId = random, Url = "custom recipe" },
                            RecipeIngredients = recIng
                        };
                    }
                    else
                    {
                        rec = new Recipe
                        {
                            RecipeId = random,
                            RecipeName = recipeName,
                            RecipeDescription = recipeDesc,
                            RecipeSteps = recStep,
                            RecipeDocument = new RecipeDocument { PhotoUrl = obj.photoUrl, RecipeId = random, Url = obj.url },
                            RecipeIngredients = recIng
                        };

                    }

                    RecipeList reclist = new RecipeList
                    {
                        RecipeId = random,
                        AccId = id,
                        Recipe = rec
                    };

                    context.RecipeLists.Add(reclist);
                    context.SaveChanges();

                    return recID;

                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "Error";
                }
            }
        }

        public frontEndRecipeStep getRecipeSteps(int recipeID)
        {
            //List<frontEndRecipeStep> frontEndRecipeSteps = new List<frontEndRecipeStep>();
            List<string> inst = new List<string>();
            frontEndRecipeStep recStep = new frontEndRecipeStep
            {
                RecipeId = recipeID,
            };

            foreach (RecipeStep i in context.RecipeSteps.Where(a => a.RecipeId == recipeID).ToList())
            {
                inst.Add(i.Instructions);
            }
            recStep.Instructions = inst;

            return recStep;
        }
        public frontEndRecipeDisplayAll getInfo(int recipeID)
        {
            Recipe rec = context.Recipes.AsNoTracking().Where(a => a.RecipeId == recipeID).Include(b => b.RecipeDocument).
                Include(c => c.RecipeIngredients).Single();
            List<ingredients> ings = new List<ingredients>();

            foreach (RecipeIngredient a in rec.RecipeIngredients)
            {
                ingredients ing = new ingredients
                {
                    ids = a.IngredientId,
                    name = a.Name
                };
                if (a.LinkProd != null)
                {
                    ing.linkProd = a.LinkProd;
                }
                ings.Add(ing);

            }

            frontEndRecipeDisplayAll obj;
            if (rec.RecipeDocument == null)
            {
                obj = new frontEndRecipeDisplayAll
                {
                    RecipeId = recipeID,
                    RecipeName = rec.RecipeName,
                    PhotoUrl = null,
                    ingredientsList = ings
                };
            }
            else
            {
                obj = new frontEndRecipeDisplayAll
                {
                    RecipeId = recipeID,
                    RecipeName = rec.RecipeName,
                    PhotoUrl = rec.RecipeDocument.PhotoUrl,
                    ingredientsList = ings
                };
            }

            return obj;
        }

        public fullRecipeDetails fullInfo(int id)
        {
            List<RecipeIngredient> recIng = context.RecipeIngredients.Where(a => a.RecipeId == id).ToList();
            Recipe rec = context.Recipes.Where(a => a.RecipeId == id).Include(b => b.RecipeDocument).Single();
            frontEndRecipeStep steps = getRecipeSteps(id);
            List<customIngredients> custIng = new List<customIngredients>();
            foreach (RecipeIngredient a in recIng)
            {
                customIngredients ing = new customIngredients
                {
                    amount = a.Amount,
                    unitOfMeasure = a.UnitOfMeasure,
                    ingredientName = a.Name,
                    ingredientId = a.IngredientId,
                    linkProduct = a.LinkProd

                };

                custIng.Add(ing);
            }


            fullRecipeDetails details = new fullRecipeDetails
            {
                ingredientsList = custIng,
                RecipeId = id,
                RecipeName = rec.RecipeName,
                desc = rec.RecipeDescription,
                steps = steps
            };

            if (rec.RecipeDocument != null)
            {
                if (rec.RecipeDocument.PhotoUrl != null)
                {
                    details.PhotoUrl = rec.RecipeDocument.PhotoUrl;
                }
                else
                {
                    details.PhotoUrl = null;
                }
            }
            else
            {
                details.PhotoUrl = null;
            }

            return details;

        }
        public frontEndRecipeClickDetails addDescToInfo(frontEndRecipeDisplayAll info)
        {
            frontEndRecipeClickDetails obj = new frontEndRecipeClickDetails();
            obj.basicInfo = info;

            Recipe rec = context.Recipes.AsNoTracking().Where(a => a.RecipeId == obj.basicInfo.RecipeId).Single();
            obj.desc = rec.RecipeDescription;

            return obj;
        }

        public String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId)
        {
            Product prod = new Product
            {
                ItemId = itemId,
                Quantity = quantity,
                Category = category,
                Name = name,
                Searchtag = searchtag,
                IngredientId = ingredientId
            };
            context.Products.Add(prod);
            context.SaveChanges();
            return "Added";
        }

        // public String addProductTest(string itemId, string quantity, string category, string name, string searchtag, int ingredientId)
        // {
        //     Product prod = new Product
        //     {
        //         ItemId = itemId,
        //         Quantity = quantity,
        //         Category = category,
        //         Name = name,
        //         Searchtag = searchtag,
        //         IngredientId = ingredientId
        //     };
        //     context.Products.Add(prod);
        //     context.SaveChanges();
        //     return "Added";
        // }

        // public TillShoppingItems getProductDTTest(Guid userId)
        // {
        //     List<ProductDt> prodDt = new List<ProductDt>();
        //     List<Product> products = context.Products.ToList();

        //     foreach (Product a in products)
        //     {
        //         ProductDt prodD = new ProductDt
        //         {
        //             productID = a.ItemId,
        //             exp = new DateTime(2021, 5, 1, 8, 30, 52),
        //             count = 1,
        //         };
        //         prodD.NotificationTime = prodD.exp.Subtract(new TimeSpan(4, 0, 0, 0, 0));
        //         prodDt.Add(prodD);
        //     }

        //     TillShoppingItems till = new TillShoppingItems
        //     {
        //         items = prodDt,
        //         AccountId = userId
        //     };

        //     return till;
        // }
        public Ingredient getAllProductsForIng(int ingId)
        {
            Ingredient prods = context.Ingredients.Where(a => a.IngredientId == ingId).Include(b => b.Products).Single();
            return prods;
        }
    }
}