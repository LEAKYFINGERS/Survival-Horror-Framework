////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        12.01.21
// Date last edited:    12.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The menu tile which allows the player to 'inspect' the item in the currently selected inventory tile.
    public class CheckItemMenuTile : MenuTile
    {
        [HideInInspector]
        public InventoryItem InventoryItemToCheck;

        // Pushes the item interaction menu tile group onto the stack if this inventory tile currently contains an inventory item.
        public override void ActivateTile(GameMenu gameMenu)
        {
            if(InventoryItemToCheck == null)
            {
                throw new System.Exception("In order for the CheckitemMenuTile " + name + " to be activated, the value for InventoryItemToCheck cannot be null.");
            }

            base.ActivateTile(gameMenu);

            gameMenu.BeginCheckInventoryItemProcess(InventoryItemToCheck);
        }
    }
}
