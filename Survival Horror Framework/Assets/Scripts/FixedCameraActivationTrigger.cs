////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    30.10.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SurvivalHorrorFramework
{
    // The script for a trigger used to inform a FixedCamera whether an object tagged "Player" has entered, stayed, or exited its bounds so the camera can decide whether to be active or not.
    [RequireComponent(typeof(BoxCollider))]
    public class FixedCameraActivationTrigger : MonoBehaviour
    {
        public delegate void TriggerEventHandler();

        public Color GizmoDrawColor = new Color(0.0f, 0.0f, 1.0f, 0.25f); // The color used to draw a filled box gizmo so it's easier to see the bounds of the trigger and how it intesects with others in the scene.
        public event TriggerEventHandler TriggerEnteredByPlayer; 
        public event TriggerEventHandler PlayerStayedInTrigger; 
        public event TriggerEventHandler TriggerExitedByPlayer; 

        public float DistanceFromTriggerCenterToPlayer
        {
            get { return (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position + GetComponent<BoxCollider>().center).magnitude; }
        }        


        private void Awake()
        {
            Assert.IsTrue(GetComponent<BoxCollider>().isTrigger, "The box collider attached to " + name + " must be a trigger in order for the FixedCameraActivationTrigger component to function.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TriggerEnteredByPlayer != null && other.tag == "Player")
                TriggerEnteredByPlayer.Invoke();
        }

        private void OnTriggerStay(Collider other)
        {
            if (PlayerStayedInTrigger != null && other.tag == "Player")
                PlayerStayedInTrigger.Invoke();
        }

        private void OnTriggerExit(Collider other)
        {
            if (TriggerExitedByPlayer != null && other.tag == "Player")
                TriggerExitedByPlayer.Invoke();
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = GizmoDrawColor;
            Gizmos.DrawCube(transform.position + GetComponent<BoxCollider>().center, Vector3.Scale(transform.localScale, GetComponent<BoxCollider>().size));
        }
    }
}
