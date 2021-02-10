////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    10.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    // A script for an interactive object which plays an animated vignette before loading another scene when interacted with - must have the same name within the scene as the associated SceneEntrancePoint to allow the SceneTransferrableData to store if this transition object has been unlocked in a previous scene.
    public class SceneTransitionObject : InteractiveObject
    {
        public GameMenu SceneGameMenu; // The menu used by the player in the scene to store inventory items which can be transferred between scenes.
        public Transform AnimatedVignetteObjectPrefab; // The prefab used to spawn the animated vignette object which indicates the player is transitioning scenes e.g. a set of doors opening, a ladder, etc.
        public VignetteCamera SceneVignetteCamera;
        public bool IsLockedOnGameStart; // Whether the transition object begins 'locked' (i.e. the player cannot use it to transition to another scene) when the game starts - will be overridden on scene load if the name of this object appears on the SceneTransferrableData UnlockedSceneTransitionObjects list of strings.
        [HideInInspector] public bool IsLocked;
        public int DestinationSceneIndex;
        public string DestinationSceneEntrancePointName; // The name of the SceneEntrancePoint instance in the destination scene at which the player will be placed when it is loaded.
        
        public override void Interact()
        {
            if (!IsLocked)
            {
                SceneGameMenu.UpdateSceneTransferrableDataWithInventoryData();

                SceneTransferrableData.NextSceneEntrancePointName = DestinationSceneEntrancePointName; // Updates the transferrable scene entrance point name value so that the specifed entrance point will be used to position the player once the destination scene is loaded.
                SceneVignetteCamera.PlayVignetteAndLoadScene(AnimatedVignetteObjectPrefab, DestinationSceneIndex);
            }
            else
            {
                Debug.Log(name + " is currently locked.");
            }
        }
                

        private void Start()
        {
            // If the transition object is flagged to be locked on game start, checks to see whether it is present within the SceneTransferrableData UnlockedSceneTransitionObjects list - if that is the case sets it as 'unlocked'.
            if (IsLockedOnGameStart)
            {
                IsLocked = true;
                foreach (string unlockedTransitionObjectName in SceneTransferrableData.UnlockedSceneTransitionObjects)
                {
                    if (this.name == unlockedTransitionObjectName)
                    {
                        IsLocked = false;
                        Debug.Log(name + " has been unlocked due to being recorded as unlocked in a previous scene load.");
                        break;
                    }
                }
            }
            else
            {
                IsLocked = false;
            }
        }
        
        private void OnDestroy()
        {
            // If unlocked, checks to see whether this transition object or its destination object already exist within the static 'unlocked objects' list and adds either if they don't.
            if (!IsLocked)
            {
                bool existsInUnlockedTransitionObjectsList = false;
                bool destinationObjectExistsInUnlockedTransitionObjectsList = false;
                foreach (string unlockedObjectName in SceneTransferrableData.UnlockedSceneTransitionObjects)
                {
                    if (this.name == unlockedObjectName)
                    {
                        existsInUnlockedTransitionObjectsList = true;
                    }
                    else if (DestinationSceneEntrancePointName == unlockedObjectName)
                    {
                        destinationObjectExistsInUnlockedTransitionObjectsList = true;
                    }
                }
                if(!existsInUnlockedTransitionObjectsList)
                {
                    SceneTransferrableData.UnlockedSceneTransitionObjects.Add(name);
                    Debug.Log("Transition object " + name + " added to static list of unlocked transition objects.");
                }
                if(!destinationObjectExistsInUnlockedTransitionObjectsList)
                {
                    SceneTransferrableData.UnlockedSceneTransitionObjects.Add(DestinationSceneEntrancePointName);
                    Debug.Log("Transition object " + DestinationSceneEntrancePointName + " added to static list of unlocked transition objects.");
                }
            }
        }
    }
}
