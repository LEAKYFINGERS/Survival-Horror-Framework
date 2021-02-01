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
    // The scriptable object used to contain a recipe detailing two items which the player can combine within their inventory to generate a third.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Item Recipe")]
    public class ItemRecipe : ScriptableObject
    {
        public InventoryItem IngredientA;
        public InventoryItem IngredientB;
        public InventoryItem Result;
    }
}