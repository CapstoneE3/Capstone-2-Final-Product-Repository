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

        public List<Recipe> calculateRecipeScores(Guid accountId)
        {
            
            List<Recipe> recipeImportList = context.Recipes.Include(a => a.RecipeIngredients).Take(30).ToList(); 
                // Should give us a list of 30 random recipes 

            List<InventoryList> inventory = context.InventoryLists.Where(a => a.AccId == accountId).Include(c => c.Item).ToList();

            //List<FrontEndInventoryFormat> userInventory = (List<FrontEndInventoryFormat>) inventoryUser;
            // List<InventoryList> inventoryUser = InventoryRepo.GetInventoryList(accountId);
                // CHECK THIS FUNCTION CALL!!!

            int onePriority = 1; 
                // Drinks, Liquor, Baby
            int twoPriority = 2; 
                // Kids & Lunch Box, Entertaining at Home
                // Convenience Meals, International Foods 
            int threePriority = 3;  
                // Frozen 
            int fourPriority = 5; 
                // Bakery, Pantry, From The Deli 
            int fivePriority = 7; 
                // Fruit and Vegetables, Meat and Seafood, and Dairy, Eggs and Meals

            
            //int[] allRecipeScores = null; 
            List<int> allRecipeScores = new List<int>();

            //List<Recipe> goodRecipes = null; 
            List<Recipe> goodRecipes = new List<Recipe>();

            for(int ii = 0; ii < recipeImportList.Count(); ii++)
            {
                // CHECK IF ITS .SIZE() OR .LENGTH() FOR ARRAYS
                
                int totalRecipeScore = 0; 

                List<RecipeIngredient> ingredientsForRecipe = recipeImportList[ii].RecipeIngredients.ToList();
                    // CHECK THIS FUNCTION CALL 

                for(int jj = 0; jj < ingredientsForRecipe.Count(); jj++)
                {
                    
                    RecipeIngredient currentIngredient = ingredientsForRecipe[jj];


                    for(int kk = 0; kk < inventory.Count(); kk++)

                        // we need to loop through all of the products in the users inventory using the kk for-loop above 
                        // On each iteration, we want to grab the kk-th product and assign it to 'prod' in the call below. 

                        //Product prod = inventoryUser[kk]
                            // CHECK THIS FUNCTION CALL 
                            


                        if(currentIngredient.IngredientId == inventory[kk].Item.IngredientId)
                        {
                            string prodCategory = inventory[kk].Item.Category; 

                            if((prodCategory.Equals("Drinks")) || (prodCategory.Equals("Liquor"))
                            || (prodCategory.Equals("Baby")))
                            {
                                
                                if((inventory[kk].ExpDate.Subtract(new TimeSpan(4,0,0,0,0)) > DateTime.Today)) 
                                {
                                    totalRecipeScore = totalRecipeScore + (onePriority*2); 
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + onePriority;
                                }

                            }
                            else if((prodCategory.Equals("Kids & Lunch Box")) 
                            || (prodCategory.Equals("Entertaining At Home")) || (prodCategory.Equals("Convenience Meals"))
                            || (prodCategory.Equals("International Foods")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4,0,0,0,0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (twoPriority*2); 
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + twoPriority;
                                }
                            }
                            else if((prodCategory.Equals("Frozen")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4, 0, 0, 0, 0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (threePriority*2); 
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + threePriority;
                                }
                            }
                            else if((prodCategory.Equals("Bakery")) || (prodCategory.Equals("Pantry"))
                            || (prodCategory.Equals("From The Deli")))
                            {
                                if ((inventory[kk].ExpDate.Subtract(new TimeSpan(4,0,0,0,0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (fourPriority*2); 
                                }
                                else
                                {
                                    totalRecipeScore = totalRecipeScore + fourPriority;
                                }
                            }
                            else if((prodCategory.Equals("Fruit & Vegetables")) || (prodCategory.Equals("Meat & Seafood")) 
                            || (prodCategory.Equals("Dairy, Eggs & Meals")))
                            {
                                if((inventory[kk].ExpDate.Subtract(new TimeSpan(4,0,0,0,0)) > DateTime.Today))
                                {
                                    totalRecipeScore = totalRecipeScore + (fivePriority*2); 
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

                int recipeOneScore = allRecipeScores.Max(); // gives a score 
                int recipeOneIndex = allRecipeScores.ToList().IndexOf(recipeOneScore);
                goodRecipes.Add(recipeImportList[recipeOneIndex]);
                allRecipeScores.RemoveAt(recipeOneIndex);
                recipeImportList.RemoveAt(recipeOneIndex);
                    // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
                    // because we're about to store it within our goodRecipes list 
                

                int recipeTwoScore = allRecipeScores.Max(); // gives a score 
                int recipeTwoIndex = allRecipeScores.ToList().IndexOf(recipeOneScore);
                goodRecipes.Add(recipeImportList[recipeTwoIndex]);
                allRecipeScores.RemoveAt(recipeTwoIndex);
                recipeImportList.RemoveAt(recipeTwoIndex);
                    // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
                    // because we're about to store it within our goodRecipes list 

                int recipeThreeScore = allRecipeScores.Max(); // gives a score 
                int recipeThreeIndex = allRecipeScores.ToList().IndexOf(recipeOneScore); 
                goodRecipes.Add(recipeImportList[recipeThreeIndex]);
                allRecipeScores.RemoveAt(recipeThreeIndex);
                recipeImportList.RemoveAt(recipeThreeIndex);
                    // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
                    // because we're about to store it within our goodRecipes list 
                

                int recipeFourScore = allRecipeScores.Max(); // gives a score 
                int recipeFourIndex = allRecipeScores.ToList().IndexOf(recipeOneScore);
                goodRecipes.Add(recipeImportList[recipeFourIndex]);
                allRecipeScores.RemoveAt(recipeFourIndex);
                recipeImportList.RemoveAt(recipeFourIndex);
                    // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
                    // because we're about to store it within our goodRecipes list
                

                int recipeFiveScore = allRecipeScores.Max(); // gives a score 
                int recipeFiveIndex = allRecipeScores.ToList().IndexOf(recipeOneScore); 
                goodRecipes.Add(recipeImportList[recipeFiveIndex]);
                allRecipeScores.RemoveAt(recipeFiveIndex);
                recipeImportList.RemoveAt(recipeFiveIndex);
                // Can pre-emptively remove our good recipe (representation) from allRecipeScores and recipeImportList
                // because we're about to store it within our goodRecipes list



            return goodRecipes; 

        }


        public List<Recipe> getRecipes(Guid id)
        {
            List<Recipe> recipes = context.Recipes.Include(a => a.RecipeDocument).Include(b => b.RecipeLists.Where(c => c.AccId == id)).ToList();
            return recipes;
        }

        public string createRecipe(Guid id, string recipeName, string recipeDesc)
        {
            try
            {
                Recipe recipes = context.Recipes.Where(a => a.RecipeName.Equals(recipeName)).Include(b => b.RecipeLists.Where(c => c.AccId == id)).Single();
                return "Exists";
            }
            catch(Exception)
            {   
                try
                {
                    int random;
                    do
                    {
                        Random rand = new Random();
                        random = rand.Next(10000);

                    }while(context.Recipes.Where(a => a.RecipeId == random) == null);
                        
                    Recipe rec = new Recipe{
                        RecipeId = random,
                        RecipeName = recipeName,
                        RecipeDescription = recipeDesc
                    };

                    RecipeList reclist = new RecipeList{
                        RecipeId = random,
                        AccId = id,
                        Recipe = rec
                    };

                    context.RecipeLists.Add(reclist);
                    context.SaveChanges();

                    return "Success";

                }
                catch(Exception)
                {
                    return "Error";
                }
            }
        }
    }
}