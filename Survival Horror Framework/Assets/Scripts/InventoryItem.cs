////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.11.20
// Date last edited:    29.11.20
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
        public Transform ModelPrefab; // The prefab used to spawn the model which can be viewed and manipulated by the player.
        public string Name = "<color=#188E26>Inventory Item</color>";
    }
}
