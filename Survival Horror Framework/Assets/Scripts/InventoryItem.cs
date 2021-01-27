////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.11.20
// Date last edited:    27.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A scriptable object used to store the data representing an item currently in the inventory of the player.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Inventory Item")]
    public class InventoryItem : ScriptableObject
    {
        public Dialog ExamineDialog; // The dialog displayed when the item is examined/checked.
        public Sprite InventoryTileSprite; // The sprite used to display the inventory item on top of an inventory tile to show that the player is currently storing it.
        public Transform ModelPrefab; // The prefab used to spawn the model which can be viewed and manipulated by the player.        
        public string DisplayName = "<color=#188E26>Inventory Item</color>";
        [Min(1)]
        public uint MaxStackCount = 1; // The maximum number of instances of this inventory item which can be stored within a single InventoryTile.
    }
}
