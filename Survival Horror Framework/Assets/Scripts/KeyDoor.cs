////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        10.02.21
// Date last edited:    10.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for a door which requires the player to use a key object to unlock it before allowing the player to travel between scenes. 
    public class KeyDoor : SceneTransitionObject
    {
        public InventoryItem Key;
    }
}
