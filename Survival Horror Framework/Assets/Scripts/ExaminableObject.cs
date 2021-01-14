////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        14.01.21
// Date last edited:    14.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // An interactive object which pauses the scene and displays the specified dialog when interacted with by the player.
    public class ExaminableObject : InteractiveObject
    {
        public DialogDisplay SceneDialogDisplay;
        public Dialog InteractDialog;

        public override void Interact()
        {
            if (!isPaused)
            {
                SceneDialogDisplay.DisplayDialog(InteractDialog, true);
            }
        }
    }
}
