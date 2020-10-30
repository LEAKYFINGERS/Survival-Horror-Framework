////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    30.10.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script used to handle the activation and deactivation of all the FixedCamera instances within a room.
    public class FixedCameraHandler : MonoBehaviour
    {
        public List<FixedCamera> FixedCameras;


        private List<FixedCamera> fixedCamerasWithPlayerInActivationTriggerThisFrame; // A list of the fixed cameras which contain an object tagged "Player" within any of their FixedCameraActivationTriggers during the current frame.
        private bool playerExitedACameraActivationTriggerThisFrame;

        private bool AreAnyFixedCamerasActive
        {
            get
            {
                foreach (FixedCamera camera in FixedCameras)
                {
                    if (camera.CameraComponentsAreActive)
                        return true;
                }
                return false;
            }
        }
        
        private void Awake()
        {
            fixedCamerasWithPlayerInActivationTriggerThisFrame = new List<FixedCamera>();
        }

        private void Start()
        {
            foreach (FixedCamera camera in FixedCameras)
            {
                // Initially deactivates all the fixed cameras so that the one with the activation trigger the player is standing within will be activated.  
                camera.CameraComponentsAreActive = false;     
                camera.PlayerEnteredActivationTrigger += SetAsActiveCameraIfNoneAreActive;

                // Adds any camera with the player within one of it's activation triggers to the list each frame.
                camera.PlayerEnteredActivationTrigger += AddCameraToListWithPlayerInActivationTriggerThisFrame;
                camera.PlayerStayedInActivationTrigger += AddCameraToListWithPlayerInActivationTriggerThisFrame;

                camera.PlayerExitedActivationTrigger += FlagPlayerExitingCameraActivationTrigger;
            }
        }

        private void LateUpdate()
        {
            // If the player has exited a camera activation trigger during this frame, activates the camera with the activation trigger which the player is currently within and which also has the center position that's closest to the player.
            if (playerExitedACameraActivationTriggerThisFrame)
            {
                // If the player is currently colliding with only one camera activation trigger, sets the associated camera as the active camera.
                if (fixedCamerasWithPlayerInActivationTriggerThisFrame.Count == 1)                
                    SetAsActiveCameraAndDeactivateAllOthers(fixedCamerasWithPlayerInActivationTriggerThisFrame[0]);                
                else
                {
                    FixedCamera cameraWithActivationTriggerClosestToPlayer = fixedCamerasWithPlayerInActivationTriggerThisFrame[0];
                    foreach (FixedCamera fixedCamera in fixedCamerasWithPlayerInActivationTriggerThisFrame)
                    {
                        if (fixedCamera.DistanceFromClosestTriggerCenterToPlayer < cameraWithActivationTriggerClosestToPlayer.DistanceFromClosestTriggerCenterToPlayer)
                            cameraWithActivationTriggerClosestToPlayer = fixedCamera;
                    }
                    SetAsActiveCameraAndDeactivateAllOthers(cameraWithActivationTriggerClosestToPlayer);
                }
            }
            
            fixedCamerasWithPlayerInActivationTriggerThisFrame.Clear(); // Clears the list of activation triggers containing the player every frame.
            playerExitedACameraActivationTriggerThisFrame = false; // Resets the 'player exited activation trigger' flag every frame.
        }

        private void FlagPlayerExitingCameraActivationTrigger(FixedCamera exitedCamera)
        {
            playerExitedACameraActivationTriggerThisFrame = true;
        }

        private void AddCameraToListWithPlayerInActivationTriggerThisFrame(FixedCamera activeCamera)
        {
            fixedCamerasWithPlayerInActivationTriggerThisFrame.Add(activeCamera);
        }        

        private void SetAsActiveCameraIfNoneAreActive(FixedCamera activeCamera)
        {
            if (!AreAnyFixedCamerasActive)            
                SetAsActiveCameraAndDeactivateAllOthers(activeCamera);
        }

        private void SetAsActiveCameraAndDeactivateAllOthers(FixedCamera activeCamera)
        {
            foreach (FixedCamera camera in FixedCameras)
            {
                camera.CameraComponentsAreActive = (camera == activeCamera);
            }
        }
    }
}
