////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    16.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(Canvas))]
    // The script for the in-game menu which handles inventory, the map, and discovered files.
    public class GameMenu : MonoBehaviour
    {
        public PauseHandler ScenePauseHandler;
        public float ForwardOffsetFromCamera = 0.31f; // The offset used to position the menu canvas in front of the active camera.


        private Canvas canvasComponent;
        private bool isMenuActive;
        private bool wasMenuInputDownDuringPreviousUpdate;

        private void Awake()
        {
            canvasComponent = GetComponent<Canvas>();

            SetMenuActiveState(false);
        }

        private void Update()
        {
            // Toggles the active state of the menu if the 'Menu' input is pressed.
            if(Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
            {
                SetMenuActiveState(!isMenuActive);
            }
        }

        private void LateUpdate()
        {
            wasMenuInputDownDuringPreviousUpdate = Input.GetAxis("Menu") == 1.0f;
        }

        // Activates or deactivates the menu if the scene isn't already currently paused (e.g. for camera transition stutter effect).
        private void SetMenuActiveState(bool activeState)
        {            
            if (!ScenePauseHandler.IsScenePaused || ScenePauseHandler.IsScenePaused && isMenuActive && !activeState)
            {
                if (activeState)
                {
                    ScenePauseHandler.PauseScene();
                }
                else
                {
                    ScenePauseHandler.UnpauseScene();
                }
                canvasComponent.enabled = activeState;

                isMenuActive = activeState;
            }
        }
    }
}
