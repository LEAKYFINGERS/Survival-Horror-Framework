////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        03.01.21
// Date last edited:   03.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A script derived from MenuTile which when activated tries to add the specified inventory item to the player inventory stored within in the game menu and then exits the menu.
    public class ConfirmAddItemMenuTile : MenuTile
    {
        public InventoryItem ItemToAdd;

        public override void ActivateTile(GameMenu gameMenu)
        {
            if(!ItemToAdd)
            {
                throw new System.Exception("The " + name + " menu tile must have an inventory item specified to add to the player inventory before ActivateTile() is called.");
            }

            

            if (!gameMenu.TryAddItemToPlayerInventory(ItemToAdd))
            {
                // TODO - create dialog stating inventory is currently full and deactivate menu.
            }



            gameMenu.DeactivateMenu();
        }
    }
}
