////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        04.02.21
// Date last edited:    04.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for an object which once destroyed (in a manner other than the scene exiting) will remain permanently destroyed even when the scene is reloaded - requires the object to always remain in exactly the same world position to function.
    public class PermanentlyDestructibleObject : MonoBehaviour
    {
        // The ID which combines the name and world position of the object into a string so that it can be correctly identified as the 'same object' each time the scene is reloaded.
        public string UniqueID
        {
            get
            {
                return name + transform.position.ToString();
            }
        }


        private bool isBeingDestroyedDueToBeingInPermanentlyDestroyedObjectsList; // The flag used to differentiate whether the object is being destroyed due to being within the SceneTransferrableData.PermanentlyDestroyedObjectUniqueIDs list or if is being destroyed as part of normal gameplay (e.g. destroying a gameobject because the player picks it up, etc.)

        private void Awake()
        {
            // If the object is referenced within the 'permanently destroyed objects list' transferred between scenes which means it has been destroyed previously during gameplay, destroys it when the scene loads.
            foreach (string destroyedObjectID in SceneTransferrableData.PermanentlyDestroyedObjectUniqueIDs)
            {
                if (destroyedObjectID == UniqueID)
                {
                    isBeingDestroyedDueToBeingInPermanentlyDestroyedObjectsList = true;
                    Destroy(gameObject);
                }
            }
        }

        private void OnDestroy()
        {
            // If the scene isn't currently exiting and the object is being destroyed for a reason other than that it was already on the 'permanently destroyed' list, adds it to the list.
            if (gameObject.scene.isLoaded && !isBeingDestroyedDueToBeingInPermanentlyDestroyedObjectsList)
            {
                foreach (string destroyedObjectID in SceneTransferrableData.PermanentlyDestroyedObjectUniqueIDs)
                {
                    if (destroyedObjectID == UniqueID)
                    {
                        throw new System.Exception("There is already a unique ID within the SceneTransferrableData.PermanentlyDestroyedObjectUniqueIDs list which matches the ID of " + name + " which is trying to be added to the list.");
                    }
                }

                SceneTransferrableData.PermanentlyDestroyedObjectUniqueIDs.Add(UniqueID);
            }
        }
    }
}
