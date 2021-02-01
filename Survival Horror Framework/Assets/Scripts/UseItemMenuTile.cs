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
    // The menu tile which allows the player to use/equip the item in the currently selected inventory tile.
    public class UseItemMenuTile : MenuTile
    {
        [HideInInspector]
        //public InventoryItem InventoryItemToUse;
        public InventoryTile InventoryTileToUse;

        public override void ActivateTile(GameMenu gameMenu)
        {
            if(InventoryTileToUse == null)
            {
                throw new System.Exception("In order for the UseItemTile " + name + " to be activated, the value for InventoryItemToUse cannot be null.");
            }

            if (!InventoryTileToUse.IsEmpty)
            {
                InventoryTileToUse.DestroyStoredInventoryItems(1);
                gameMenu.SetParentMenuTileGroup(gameMenu.DefaultParentMenuTileGroup);
            }

            InventoryTileToUse = null;

            //Debug.Log("Using inventory item " + InventoryItemToUse.DisplayName);
        }
    }
}
