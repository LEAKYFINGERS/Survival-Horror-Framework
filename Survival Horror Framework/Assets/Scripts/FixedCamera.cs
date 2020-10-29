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
    // The script for a fixed camera which is activated and deactivated by a FixedCameraHandler accordingly when the player enters any of its activation triggers.
    [RequireComponent(typeof(AudioListener))]
    [RequireComponent(typeof(Camera))]
    public class FixedCamera : MonoBehaviour
    {
        public delegate void EventHandler(FixedCamera sender);

        public List<OnTriggerEnteredEventPublisher> ActivationTriggers; // A list of trigger colliders which publish a TriggerEntered event when entered. 
        public event EventHandler PlayerEnteredActivationTrigger; // An event published when an object tagged 'Player' enters any of the activation collider.

        // Sets the active status of the audio listener and camera components of the fixed camera.
        public void SetCameraComponentsActiveStatus(bool activeStatus)
        {
            audioListenerComponent.enabled = activeStatus;
            cameraComponent.enabled = activeStatus;
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
            // Subscribes the OnActivationTriggerEntered() function to be called whenever any of the activation triggers publishes a TriggerEntered event.
            foreach (OnTriggerEnteredEventPublisher trigger in ActivationTriggers)
            {                
                trigger.TriggerEntered += OnActivationTriggerEntered;
            }
        }

        // If an activation trigger has been entered by an object with the 'Player' tag, publishes the PlayerEnteredActivationTrigger event.
        private void OnActivationTriggerEntered(string otherTag)
        {
            if (PlayerEnteredActivationTrigger != null && otherTag == "Player")
                PlayerEnteredActivationTrigger.Invoke(this);
        }
    }
}
