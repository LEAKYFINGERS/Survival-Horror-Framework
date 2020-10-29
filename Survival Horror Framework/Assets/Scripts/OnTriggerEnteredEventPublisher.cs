////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.10.20
// Date last edited:    29.10.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace SurvivalHorrorFramework
{
    // A script which publishes the TriggerEntered event when the attached trigger is entered.
    [RequireComponent(typeof(Collider))]
    public class OnTriggerEnteredEventPublisher : MonoBehaviour
    {
        public delegate void TriggerEventHandler(string otherTag);
        
        public event TriggerEventHandler TriggerEntered; // An event published when the attached trigger is entered - includes the tag of the other collider.

        private void Awake()
        {
            Assert.IsTrue(GetComponent<Collider>().isTrigger, "The collider attached to " + name + " must be a trigger in order for the OnTriggerEventPublisher component to function.");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (TriggerEntered != null)
                TriggerEntered.Invoke(other.tag);
        }
    }
}
