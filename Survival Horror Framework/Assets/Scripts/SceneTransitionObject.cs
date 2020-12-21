////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    21.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    public class SceneTransitionObject : InteractiveObject
    {
        //public ColorTintPostProcessHandler FadeHandler;
        //public PauseHandler ScenePauseHandler;        
        //public float FadeDuration;
        public Transform AnimatedVignetteObjectPrefab;
        public VignetteCamera SceneVignetteCamera;
        public int DestinationSceneIndex;
        public string DestinationSceneEntrancePointName;

        public override void Interact()
        {
            StartCoroutine("SceneTranstionCoroutine");
        }


        private IEnumerator SceneTranstionCoroutine()
        {
            SceneVignetteCamera.PlayVignette(AnimatedVignetteObjectPrefab);
            yield return new WaitForSecondsRealtime(SceneVignetteCamera.FadeDuration + 4.0f); // TODO - create VignetteCamera coroutine variant which just loads the specified scene when completed?

            SceneTransferrableData.NextSceneEntrancePointName = DestinationSceneEntrancePointName;
            SceneManager.LoadScene(DestinationSceneIndex);
        }
    }
}
