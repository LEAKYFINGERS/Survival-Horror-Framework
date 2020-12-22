////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    22.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    // A script for an interactive object which plays an animated vignette before loading another scene when interacted with.
    public class SceneTransitionObject : InteractiveObject
    {
        public Transform AnimatedVignetteObjectPrefab; // The prefab used to spawn the animated vignette object which indicates the player is transitioning scenes e.g. a set of doors opening, a ladder, etc.
        public VignetteCamera SceneVignetteCamera;
        public int DestinationSceneIndex;
        public string DestinationSceneEntrancePointName; // The name of the SceneEntrancePoint instance in the destination scene at which the player will be placed when it is loaded.

        public override void Interact()
        {
            SceneTransferrableData.NextSceneEntrancePointName = DestinationSceneEntrancePointName; // Updates the transferrable scene entrance point name value so that the specifed entrance point will be used to position the player once the destination scene is loaded.
            SceneVignetteCamera.PlayVignetteAndLoadScene(AnimatedVignetteObjectPrefab, DestinationSceneIndex);
        }
    }
}
