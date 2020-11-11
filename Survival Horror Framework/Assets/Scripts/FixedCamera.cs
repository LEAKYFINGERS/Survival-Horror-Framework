////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    11.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for a fixed camera which is activated and deactivated by a FixedCameraHandler according to whether an object tagged "Player" has entered, stayed, or exited any of its FixedCameraActivationTriggers.
    [RequireComponent(typeof(AudioListener))]
    [RequireComponent(typeof(Camera))]
    public class FixedCamera : MonoBehaviour
    {
        public delegate void FixedCameraEventHandler(FixedCamera sender);

        public FixedCameraActivationTrigger[] ActivationTriggers; // A list of associated trigger colliders which publish events detailing whether a object tagged "Player" has entered, stayed, or exited them during this frame. 
        public event FixedCameraEventHandler PlayerEnteredActivationTrigger;
        public event FixedCameraEventHandler PlayerStayedInActivationTrigger;
        public event FixedCameraEventHandler PlayerExitedActivationTrigger;

        public float DistanceFromClosestTriggerCenterToPlayer
        {
            get
            {
                float distanceFromClosestTriggerCenter = 9999.9f;
                foreach (FixedCameraActivationTrigger trigger in ActivationTriggers)
                {
                    if (trigger.DistanceFromTriggerCenterToPlayer < distanceFromClosestTriggerCenter)
                    {
                        distanceFromClosestTriggerCenter = trigger.DistanceFromTriggerCenterToPlayer;
                    }
                }

                return distanceFromClosestTriggerCenter;
            }
        }

        // The active status of the audio listener and camera components of the fixed camera.
        public bool CameraComponentsAreActive
        {
            get
            {
                return (audioListenerComponent.enabled && cameraComponent.enabled);
            }
            set
            {
                audioListenerComponent.enabled = value;
                cameraComponent.enabled = value;
            }
        }


        private AudioListener audioListenerComponent;
        private Camera cameraComponent;

        private void Awake()
        {
            audioListenerComponent = GetComponent<AudioListener>();
            cameraComponent = GetComponent<Camera>();
        }

        private void Start()
        {
            // Subscribes each of the player trigger events so that they will be invoked by any of the attached FixedCameraActivationTriggers.
            foreach (FixedCameraActivationTrigger trigger in ActivationTriggers)
            {
                trigger.TriggerEnteredByPlayer += OnActivationTriggerEnteredByPlayer;
                trigger.PlayerStayedInTrigger += OnActivationTriggerStayedInByPlayer;
                trigger.TriggerExitedByPlayer += OnActivationTriggerExitedByPlayer;
            }
        }

        private void OnActivationTriggerEnteredByPlayer()
        {
            if (PlayerEnteredActivationTrigger != null)
            {
                PlayerEnteredActivationTrigger.Invoke(this);
            }
        }

        private void OnActivationTriggerStayedInByPlayer()
        {
            if (PlayerStayedInActivationTrigger != null)
            {
                PlayerStayedInActivationTrigger.Invoke(this);
            }
        }

        private void OnActivationTriggerExitedByPlayer()
        {
            if (PlayerExitedActivationTrigger != null)
            {
                PlayerExitedActivationTrigger.Invoke(this);
            }
        }
    }
}
