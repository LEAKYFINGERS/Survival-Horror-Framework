////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        23.11.20
// Date last edited:    11.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(BoxCollider))]
    // The script for the trigger collider used to determine which interactive objects within the scene are currently within reach of the player.
    public class PlayerInteractionTrigger : MonoBehaviour
    {
        // Returns the InteractiveObject closest to the player position if there are any currently within the trigger bounds, else returns null.
        public InteractiveObject GetClosestInteractiveObjectInsideBounds(Vector3 playerWorldPosition)
        {
            if(interactiveObjectsWithinBounds.Count == 0)
            {
                return null;
            }
            else if (interactiveObjectsWithinBounds.Count == 1)
            {
                return interactiveObjectsWithinBounds[0];
            }
            else
            {
                InteractiveObject closestToPlayer = interactiveObjectsWithinBounds[0];
                foreach(InteractiveObject interactiveObject in interactiveObjectsWithinBounds)
                {
                    if((interactiveObject.transform.position - playerWorldPosition).magnitude < (closestToPlayer.transform.position - playerWorldPosition).magnitude)
                    {
                        closestToPlayer = interactiveObject;
                    }
                }
                return closestToPlayer;
            }
        }


        private List<InteractiveObject> interactiveObjectsWithinBounds;

        private void Awake()
        {
            interactiveObjectsWithinBounds = new List<InteractiveObject>();
        }

        private void Update()
        {
            // Checks to ensure any interactive objects which have been destroyed are removed from the list.
            for (int i = 0; i < interactiveObjectsWithinBounds.Count; ++i)
            {
                if (interactiveObjectsWithinBounds[i] == null)
                {
                    interactiveObjectsWithinBounds.RemoveAt(i);
                    break;
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            // If an InteractiveObject enters the bounds of the trigger, adds it to the list of interactive objects.
            if(other.GetComponent<InteractiveObject>())
            {
                interactiveObjectsWithinBounds.Add(other.GetComponent<InteractiveObject>());
            }
        }

        private void OnTriggerExit(Collider other)
        {
            // If an InteractiveObject exits the bounds of the trigger, removes it from the list of interactive objects.
            if (other.GetComponent<InteractiveObject>())
            {
                for(int i = 0; i < interactiveObjectsWithinBounds.Count; ++i)
                {
                    if(interactiveObjectsWithinBounds[i] == other.GetComponent<InteractiveObject>())
                    {
                        interactiveObjectsWithinBounds.RemoveAt(i);
                        break;
                    }
                }                
            }
        }
    }
}
