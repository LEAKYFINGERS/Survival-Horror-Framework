////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.02.21
// Date last edited:    02.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for a menu tile used to display one of the two reticles shown when the player is attempting to combine items.
    public class CombineInventoryItemsReticle : MenuTile
    {
        public InventoryTile AttachedInventoryTile; // The inventory tile which this reticle hovers over and reads to see if it contains an inventory item to be combined.        
        [HideInInspector]
        public InventoryTile InitiallySelectedInventoryTile; // The initially-selected inventory tile which contains the item to be combined with the item stored in the attached inventory tile.        
        public ItemRecipeBook RecipeBook; // Contains all the possible item combinations the player can make.

        public override void ActivateTile(GameMenu gameMenu)
        {
            if(!AttachedInventoryTile.IsEmpty)
            {
                // If both tiles contain the same type of inventory item and the attached tile isn't full, transfers as many as possible from the initially selected tile (until the attached tile is full/the initial tile is empty).
                if(InitiallySelectedInventoryTile.StoredInventoryItemDisplayName == AttachedInventoryTile.StoredInventoryItemDisplayName && !AttachedInventoryTile.IsFull)
                {
                    while(!AttachedInventoryTile.IsFull && !InitiallySelectedInventoryTile.IsEmpty)
                    {
                        AttachedInventoryTile.StoreInventoryItem(InitiallySelectedInventoryTile.StoredInventoryItem);
                        InitiallySelectedInventoryTile.DestroyStoredInventoryItems(1);
                    }

                    InitiallySelectedInventoryTile = null;
                    gameMenu.SetParentMenuTileGroup(gameMenu.DefaultParentMenuTileGroup, InitiallySelectedInventoryTile); // Resets the menu to be back in the first 'layer' but with the newly combined item/s selected.
                }
                // Else if the combination of the two different items is a valid recipe in the recipe book, destroys them both and stores the combined result in the intially-selected tile.
                else
                {
                    InventoryItem combinedItem = RecipeBook.GetCombinationResult(InitiallySelectedInventoryTile.StoredInventoryItem, AttachedInventoryTile.StoredInventoryItem);                    
                    if(combinedItem != null)
                    {
                        InitiallySelectedInventoryTile.DestroyStoredInventoryItems();
                        InitiallySelectedInventoryTile.StoreInventoryItem(combinedItem);

                        AttachedInventoryTile.DestroyStoredInventoryItems();

                        gameMenu.SetParentMenuTileGroup(gameMenu.DefaultParentMenuTileGroup, InitiallySelectedInventoryTile); // Resets the menu to be back in the first 'layer' but with the newly combined item/s selected.
                        InitiallySelectedInventoryTile = null;
                    }
                }
            }            
        }
    }
}