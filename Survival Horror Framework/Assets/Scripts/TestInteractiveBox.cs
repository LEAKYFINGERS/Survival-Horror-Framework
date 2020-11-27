////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        23.11.20
// Date last edited:    24.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    public class TestInteractiveBox : InteractiveObject
    {
        public DialogDisplay SceneDialogDisplay;
        public Dialog InteractDialog;

        public override void Interact()
        {
            if (!isPaused)
            {
                GetComponent<Renderer>().material.color = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f));
                Debug.Log("Interacted with test box.");

                SceneDialogDisplay.PauseSceneAndDisplayDialog(InteractDialog);
            }
        }        
    }
}
