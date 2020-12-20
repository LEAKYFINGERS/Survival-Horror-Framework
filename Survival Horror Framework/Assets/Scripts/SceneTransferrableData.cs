////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        20.12.20
// Date last edited:    20.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A static class used to preseve data such as player status, inventory, etc. between scenes.
public static class SceneTransferrableData
{
    public static string NextSceneEntrancePointName; // The name of the SceneEntrancePoint instance within the next scene which will be used to position and orient the player character when it loads.
}
