////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    21.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script used to handle the activation and deactivation of all the FixedCamera instances within a room.
    public class FixedCameraHandler : MonoBehaviour
    {
        public Canvas[] Canvases; // The canvases used to display the in-game menus, dialog, etc. to be automatically positioned in front of the currently active fixed camera.
        public FixedCamera[] FixedCameras;
        public PauseHandler ScenePauseHandler;
        public float ScenePauseOnCameraChangeDuration = 0.25f;
        public float CanvasForwardOffsetFromCamera = 0.31f; // The offset value used to position the each of the canvases in front of the active camera.   

        public void SetAllFixedCamerasActiveState(bool activeState)
        {
            foreach(FixedCamera camera in FixedCameras)
            {
                camera.gameObject.SetActive(activeState);
            }
        }


        private List<FixedCamera> fixedCamerasWithPlayerInActivationTriggerThisFrame; // A list of the fixed cameras which contain an object tagged "Player" within any of their FixedCameraActivationTriggers during the current frame.
        private bool isPaused;
        private bool playerExitedACameraActivationTriggerThisFrame;

        private FixedCamera ActiveCamera
        {
            get
            {
                foreach (FixedCamera camera in FixedCameras)
                {
                    if (camera.CameraComponentsAreActive)
                    {
                        return camera;
                    }
                }
                return null;
            }
        }

        private bool AreAnyFixedCamerasActive
        {
            get
            {
                foreach (FixedCamera camera in FixedCameras)
                {
                    if (camera.CameraComponentsAreActive)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        // A coroutine which pauses the scene for the duration specified by ScenePauseOnCameraChangeDuration before setting the specified fixed camera as the active one.
        private IEnumerator PauseSceneBeforeSwappingToCameraCoroutine(FixedCamera cameraToActivate)
        {
            if (ScenePauseOnCameraChangeDuration > 0.0f)
            {
                ScenePauseHandler.PauseScene();

                float timer = 0.0f;
                while (timer < ScenePauseOnCameraChangeDuration)
                {
                    timer += Time.deltaTime;
                    yield return null;
                }

                SetAsActiveCameraAndDeactivateAllOthers(cameraToActivate);
                ScenePauseHandler.UnpauseScene();
            }
            else
            {
                SetAsActiveCameraAndDeactivateAllOthers(cameraToActivate);
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
            if (!isPaused)
            {
                // If the player has exited a camera activation trigger during this frame, activates the camera with the activation trigger which the player is currently within and which also has the center position that's closest to the player.
                if (playerExitedACameraActivationTriggerThisFrame)
                {
                    FixedCamera cameraToActivate;

                    if (fixedCamerasWithPlayerInActivationTriggerThisFrame.Count == 0)
                    {
                        throw new System.Exception("The object tagged 'Player' has moved into an area where no linked FixedCamera activation triggers exist.");
                    }
                    // If the player is currently colliding with only one camera activation trigger, sets the associated camera as the active camera.
                    else if (fixedCamerasWithPlayerInActivationTriggerThisFrame.Count == 1)
                    {
                        cameraToActivate = fixedCamerasWithPlayerInActivationTriggerThisFrame[0];
                    }
                    else
                    {
                        FixedCamera cameraWithActivationTriggerClosestToPlayer = fixedCamerasWithPlayerInActivationTriggerThisFrame[0];
                        foreach (FixedCamera fixedCamera in fixedCamerasWithPlayerInActivationTriggerThisFrame)
                        {
                            if (fixedCamera.DistanceFromClosestTriggerCenterToPlayer < cameraWithActivationTriggerClosestToPlayer.DistanceFromClosestTriggerCenterToPlayer)
                            {
                                cameraWithActivationTriggerClosestToPlayer = fixedCamera;
                            }
                        }
                        cameraToActivate = cameraWithActivationTriggerClosestToPlayer;
                    }

                    if (cameraToActivate != ActiveCamera)
                    {
                        StartCoroutine("PauseSceneBeforeSwappingToCameraCoroutine", cameraToActivate);
                    }
                }
            }

            fixedCamerasWithPlayerInActivationTriggerThisFrame.Clear(); // Clears the list of activation triggers containing the player every frame.
            playerExitedACameraActivationTriggerThisFrame = false; // Resets the 'player exited activation trigger' flag every frame.
        }

        private void Pause()
        {
            isPaused = true;
        }

        private void Unpause()
        {
            isPaused = false;
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
            {
                SetAsActiveCameraAndDeactivateAllOthers(activeCamera);
            }
        }

        private void SetAsActiveCameraAndDeactivateAllOthers(FixedCamera activeCamera)
        {
            foreach (FixedCamera camera in FixedCameras)
            {
                camera.CameraComponentsAreActive = (camera == activeCamera);
            }

            // Parents and positions each canvas in front of the active camera.
            foreach (Canvas canvas in Canvases)
            {
                canvas.transform.SetParent(activeCamera.transform);
                canvas.transform.localPosition = Vector3.forward * CanvasForwardOffsetFromCamera;
                canvas.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
