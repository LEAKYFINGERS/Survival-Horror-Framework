////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        20.12.20
// Date last edited:    10.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    public struct TransferrableInventoryTileData
    {
        public InventoryItem StoredInventoryItem;        
        public int StoredInventoryItemCount;
    }
    
    // A static class used to preseve data such as player status, inventory, etc. between scenes.
    public static class SceneTransferrableData
    {
        public static List<string> PermanentlyDestroyedObjectUniqueIDs; // A list containing the unique IDs of all permanently destroyed objects so that they can be referenced and automatically deleted on scene load.
        public static List<string> UnlockedSceneTransitionObjects; // A list containing the names of all SceneTransitionObjects which have at some point been 'unlocked' - used to ensure previously 'locked' doors remain unlocked throughout scene loads and reloads.
        public static TransferrableInventoryTileData[] InventoryTileData;        
        public static string NextSceneEntrancePointName; // The name of the SceneEntrancePoint instance within the next scene which will be used to position and orient the player character when it loads.

        static SceneTransferrableData()
        {
            PermanentlyDestroyedObjectUniqueIDs = new List<string>();
            UnlockedSceneTransitionObjects = new List<string>();
        }   
    }
}
