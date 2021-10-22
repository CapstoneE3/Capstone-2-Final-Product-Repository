export type Recipe = {
  rid: number;
  name: string;
  ingredients: string[];
  availIngredients?: string[];
  img: string;
  fav: boolean;
};

export const browseRecipes: Recipe[] = [
  {
    rid: 123457,
    name: "wappy spagettee",
    ingredients: ["Breat", "meed", "beeph", "shugir", "bols", "potatos"],
    availIngredients: ["Breat", "meed", "beeph", "shugir", "bols"],
    fav: false,
    img: "https://spoonacular.com/recipeImages/73420-312x231.jpg",
  },
];

export const likedRecipes: Recipe[] = [
  {
    rid: 123457,
    name: "wappy spagettee",
    ingredients: ["Breat", "meed", "beeph", "shugir", "bols", "potatos"],
    availIngredients: ["Breat", "meed", "beeph", "shugir", "bols"],
    fav: true,
    img: "https://spoonacular.com/recipeImages/73420-312x231.jpg",
  },
];
