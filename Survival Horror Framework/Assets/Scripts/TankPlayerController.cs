////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        25.10.20
// Date last edited:    25.10.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // Handles player movement using tank controls - rather than strafing, horizontal movement inputs rotate the player while vertical movement inputs move the player forward and back according to their current orientation.
    [RequireComponent(typeof(CharacterController))]
    public class TankPlayerController : MonoBehaviour
    {
        public float WalkSpeed = 2.0f; // The movement speed of the player in units-per-second when walking forwards or backwards.
        public float RunSpeed = 4.0f; // The movement speed of the player in units-per-second when running forwards - cannot run backwards.
        public float StationaryRotateSpeed = 270.0f; // The rotation speed of the player in degrees-per-second when standing in place.
        public float MovingRotateSpeed = 90.0f; // The rotation speed of the player in degrees-per-second when walking or running.

        private CharacterController characterController;

        private void Awake()
        {
            characterController = GetComponent<CharacterController>();
        }

        private void Update()
        {           
            transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * (Input.GetAxis("Vertical") != 0.0f ? MovingRotateSpeed : StationaryRotateSpeed) * Time.deltaTime);

            characterController.Move(transform.forward * Input.GetAxis("Vertical") * (Input.GetAxis("Run") == 1.0f && Input.GetAxis("Vertical") > 0.0f ? RunSpeed : WalkSpeed) * Time.deltaTime);
        }
    }
}
