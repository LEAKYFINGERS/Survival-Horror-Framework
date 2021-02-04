////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        28.11.20
// Date last edited:    04.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for an item which the player can pick up within the world and convert into an InventoryItem in their inventory to be used.
    [RequireComponent(typeof(PermanentlyDestructibleObject))]
    public class PickupItem : InteractiveObject
    {
        public GameMenu SceneGameMenu; // The game menu used to add the inventory item to the player inventory for future use.
        public InventoryItem InventoryRepresentation; // A scriptable object containing all the data used to represent the pickup once it's in the inventory of the player.        

        public override void Interact()
        {
            SceneGameMenu.ActivateMenuAndTryToAddItem(this);
        }
    }
}
