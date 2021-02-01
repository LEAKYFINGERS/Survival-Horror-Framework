////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.02.21
// Date last edited:    01.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The class for a script used to store and access all of the different item combinations/recipes the player can access in their inventory.
    public class ItemRecipeBook : MonoBehaviour
    {
        public ItemRecipe[] Recipes;

        // If the combination of the two items (in either order) matches one of the stored recipes, returns the result - else returns null.
        public InventoryItem GetCombinationResult(InventoryItem ingredientA, InventoryItem ingredientB)
        {
            foreach(ItemRecipe recipe in Recipes)
            {
                if((ingredientA == recipe.IngredientA && ingredientB == recipe.IngredientB) || (ingredientA == recipe.IngredientB && ingredientB == recipe.IngredientA))
                {
                    return recipe.Result;
                }
            }

            return null;
        }
    }
}
