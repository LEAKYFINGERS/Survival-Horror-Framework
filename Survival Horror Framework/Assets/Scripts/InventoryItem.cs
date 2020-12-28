////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        28.11.20
// Date last edited:    28.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for an item which the player can pick up within the world and store in their inventory to be used.
    public class InventoryItem : InteractiveObject
    {
        public GameMenu SceneGameMenu; // The game menu used to add the inventory item to the player inventory for future use.

        public override void Interact()
        {
            SceneGameMenu.ActivateMenu(GameMenu.MenuActivationMode.AddItem);
        }
    }
}
