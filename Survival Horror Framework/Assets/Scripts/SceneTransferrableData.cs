////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        20.12.20
// Date last edited:    03.02.21
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
        public static TransferrableInventoryTileData[] InventoryTileData;
        public static string NextSceneEntrancePointName; // The name of the SceneEntrancePoint instance within the next scene which will be used to position and orient the player character when it loads.
    }
}
