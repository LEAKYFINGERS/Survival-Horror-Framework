//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        31.10.20
// Date last edited:    31.10.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A singleton script used to send 'Pause' and 'Unpause' messages to every gameobject within the scene that implements the 'Pause()' and 'Unpause()' functions.
    public class PauseHandler : MonoBehaviour
    {      
        public bool IsScenePaused
        {
            get { return isScenePaused; }
        }

        // Calls the 'Pause' function for every gameobject within the scene which implements the 'Pause()' function within an attached script.
        public void PauseScene()
        {
            if (!isScenePaused)
            {
                Object[] objects = FindObjectsOfType(typeof(GameObject));
                foreach (GameObject gameObject in objects)
                    gameObject.SendMessage("Pause", SendMessageOptions.DontRequireReceiver);

                isScenePaused = true;
                Debug.Log("Scene paused"); // DEBUG
            }
        }        

        // Calls the 'Unpause' function for every gameobject within the scene which implements the 'Unpause()' function within an attached script.
        public void UnpauseScene()
        {
            if (isScenePaused)
            {
                Object[] objects = FindObjectsOfType(typeof(GameObject));
                foreach (GameObject gameObject in objects)
                    gameObject.SendMessage("Unpause", SendMessageOptions.DontRequireReceiver);

                isScenePaused = false;
                Debug.Log("Scene unpaused"); // DEBUG
            }
        }


        private bool isScenePaused;
    }
}
