////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        03.01.21
// Date last edited:    12.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A script derived from MenuTile which when activated tries to add the specified PickUpItem to the player inventory as an InventoryItem, then destroys it and then exits the game menu.
    public class ConfirmAddItemToInventoryMenuTile : MenuTile
    {
        public PickupItem ItemToAdd; // The item to be destroyed and added to the inventory as an InventoryItem when this menu tile is activated.

        public override void ActivateTile(GameMenu gameMenu)
        {
            if (!ItemToAdd)
            {
                throw new System.Exception("The " + name + " menu tile must have a PickUpItem specified to add to the player inventory before ActivateTile() is called.");
            }

            // Tries to add the InventoryItem representation of the PickUpItem to the player inventory - if successful, destroys the PickUpItem and exits the game menu.
            gameMenu.TryAddItemToPlayerInventory(ItemToAdd.InventoryRepresentation);
            Destroy(ItemToAdd.gameObject);
            gameMenu.DeactivateMenu();
        }
    }
}
