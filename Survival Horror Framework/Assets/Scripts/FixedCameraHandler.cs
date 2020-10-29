////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    29.10.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script used to handle the activation and deactivation of all the FixedCamera instances within a room.
    public class FixedCameraHandler : MonoBehaviour
    {
        public List<FixedCamera> FixedCameras; // The list of fixed cameras to handle.

        private void Start()
        {
            foreach(FixedCamera camera in FixedCameras)
            {
                camera.SetCameraComponentsActiveStatus(false); // Initially deactivates all the fixed cameras.
                camera.PlayerEnteredActivationTrigger += SetActiveCamera; // Subscribes the SetActiveCamera() function to be called when any of the fixed cameras publishes a PlayerEnteredActivationTrigger event in order to activate that specific camera.
            }
        }

        // Activates the specified fixed camera and deactivates all others.
        private void SetActiveCamera(FixedCamera activatedCamera)
        {
            foreach(FixedCamera camera in FixedCameras)
            {
                camera.SetCameraComponentsActiveStatus(camera == activatedCamera);
            }
        }
    }
}
