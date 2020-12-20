////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        25.10.20
// Date last edited:    20.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SurvivalHorrorFramework
{
    // Handles player movement using tank controls - rather than strafing, horizontal movement inputs rotate the player while vertical movement inputs move the player forward and back according to their current orientation.
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(CharacterController))]
    public class TankControlPlayer : MonoBehaviour
    {
        public PlayerInteractionTrigger InteractionTrigger; // The trigger collider used to determine which interactive objects within the scene are currently within reach of the player.        
        public float WalkSpeed = 2.0f; // The movement speed of the player in units-per-second when walking forwards.
        public float RetreatSpeed = 1.5f; // The movement speed of the player in units-per-second when walking backwards.
        public float RunSpeed = 4.0f; // The movement speed of the player in units-per-second when running forwards - cannot run backwards.
        public float StationaryRotateSpeed = 270.0f; // The rotation speed of the player in degrees-per-second when standing in place.
        public float MovingRotateSpeed = 90.0f; // The rotation speed of the player in degrees-per-second when walking or running.
        public float AnimationBlendDuration = 0.15f; // The duration of the animtion crossfades when the player transitions between clips.


        private Animator animatorComponent;
        private CharacterController characterControllerComponent;        
        private bool isPaused;
        private bool wasUseInputDownDuringPreviousUpdate;

        private void Awake()
        {
            animatorComponent = GetComponent<Animator>();
            characterControllerComponent = GetComponent<CharacterController>();
        }

        private void Start()
        {
            SceneEntrancePoint[] sceneEntrancePoints = GameObject.FindObjectsOfType<SceneEntrancePoint>(); // An array of the entrance points in the scene which can be used to orient the player character when the scene is loaded.
            if(sceneEntrancePoints.Length > 0)
            {
                // If the name of any of the scene entrance points matches the entrance point name specified by the exit point of the previous scene, positions and rotates the player to line up with that specific entrance point.
                foreach(SceneEntrancePoint sceneEntrancePoint in sceneEntrancePoints)
                {
                    if(sceneEntrancePoint.name == SceneTransferrableData.NextSceneEntrancePointName)
                    {
                        characterControllerComponent.enabled = false;
                        transform.position = sceneEntrancePoint.transform.position;
                        transform.rotation = Quaternion.Euler(0.0f, sceneEntrancePoint.transform.rotation.eulerAngles.y, 0.0f);
                        characterControllerComponent.enabled = true;

                        break;
                    }
                }                
            }
        }

        private void Update()
        {
            if (!isPaused)
            {
                UpdateInteraction();
                UpdateMovement();
                UpdateAnimation();
            }
        }

        private void LateUpdate()
        {
            wasUseInputDownDuringPreviousUpdate = Input.GetAxis("Use") == 1.0f;
        }

        private void UpdateInteraction()
        {
            // If the 'Use' input has been pressed, activates any interactive object which is within the bounds of the trigger and closest to the player.
            if(Input.GetAxis("Use") == 1.0f && !wasUseInputDownDuringPreviousUpdate)
            {
                InteractiveObject interactiveObjectInRange = InteractionTrigger.GetClosestInteractiveObjectInsideBounds(transform.position);
                if(interactiveObjectInRange != null)
                {
                    interactiveObjectInRange.Interact();
                }
            }
        }

        private void UpdateMovement()
        {
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * (Input.GetAxis("Vertical") != 0.0f ? MovingRotateSpeed : StationaryRotateSpeed) * Time.deltaTime); 

            float moveSpeed = 0.0f;
            if (Input.GetAxis("Vertical") == 1.0f)
            {
                moveSpeed = (Input.GetAxis("Run") == 1.0f ? RunSpeed : WalkSpeed);
            }
            else if (Input.GetAxis("Vertical") == -1.0f)
            {
                moveSpeed = -RetreatSpeed;
            }
            characterControllerComponent.Move(transform.forward * moveSpeed * Time.deltaTime);

            // Ensures the player remains grounded at all times.
            RaycastHit raycastHit;
            Ray ray = new Ray(transform.position, Vector3.down);
            if(Physics.Raycast(ray, out raycastHit))
            {
                if ((transform.position - raycastHit.point).magnitude > characterControllerComponent.skinWidth)
                {
                    characterControllerComponent.Move(Vector3.down * raycastHit.distance);
                }
            }
        }

        private void UpdateAnimation()
        {
            // The animator component will return a clip info count of 0 if animation crossfading is currently occuring - if not, the animator is in a valid state to potentially start crossfading to a new animation.
            if (animatorComponent.GetCurrentAnimatorClipInfoCount(0) > 0)
            {
                // If the player is walking forwards and pressing the run input, plays the 'Run' animation.
                if ((Input.GetAxis("Vertical") > 0.0f && Input.GetAxis("Run") == 1.0f) && animatorComponent.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Run")
                {
                    animatorComponent.CrossFadeInFixedTime("Run", AnimationBlendDuration);
                }
                // Else if the player is walking forwards or turning on the spot, plays the 'Walk' animation.
                else if ((Input.GetAxis("Run") == 0.0f && Input.GetAxis("Vertical") > 0.0f || (Input.GetAxis("Vertical") == 0.0f && Input.GetAxis("Horizontal") != 0.0f)) && animatorComponent.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Walk")
                {
                    animatorComponent.CrossFadeInFixedTime("Walk", AnimationBlendDuration);
                }
                // Else if the player is walking backwards plays the 'Retreat' animation.
                else if (Input.GetAxis("Vertical") < 0.0f && animatorComponent.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Retreat")
                {
                    animatorComponent.CrossFadeInFixedTime("Retreat", AnimationBlendDuration);
                }
                // Else plays the 'Idle' animation.
                else if ((Input.GetAxis("Horizontal") == 0.0f && Input.GetAxis("Vertical") == 0.0f) && animatorComponent.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Idle")
                {
                    animatorComponent.CrossFadeInFixedTime("Idle", AnimationBlendDuration);
                }
            }
        }

        private void Pause()
        {
            animatorComponent.speed = 0.0f;

            isPaused = true;
        }

        private void Unpause()
        {
            animatorComponent.speed = 1.0f;

            isPaused = false;
        }
    }
}
