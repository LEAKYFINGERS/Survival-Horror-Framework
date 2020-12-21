////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        21.12.20
// Date last edited:    21.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for a camera used to play short animated vignettes which convey interactions such as entering a door or going up stairs using an animated object. 
    [RequireComponent(typeof(Camera))]
    public class VignetteCamera : MonoBehaviour
    {
        public ColorTintPostProcessHandler FadeHandler;
        public FixedCameraHandler SceneFixedCameraHandler;
        public PauseHandler ScenePauseHandler;
        public float FadeDuration = 0.5f;
        //public Transform TestVignetteObjectPrefab; // DEBUG

        public void PlayVignette(Transform animatedVignetteObjectPrefab, bool transitionBackToSceneOnceComplete = false)
        {
            if(!isPlayVignetteCoroutineRunning)
            {
                animatedVignetteObjectPrefabForCoroutine = animatedVignetteObjectPrefab;
                StartCoroutine("PlayVignetteCoroutine", transitionBackToSceneOnceComplete);
            }
            else
            {
                Debug.Log("The PlayVignette() function cannot be executed because the PlayVignetteCoroutine() is already running.");
            }
        }


        private AudioListener audioListenerComponent;
        private Camera cameraComponent;
        private Transform animatedVignetteObjectPrefabForCoroutine;
        private bool isPlayVignetteCoroutineRunning;

        private bool CameraComponentsAreActive
        {
            get { return audioListenerComponent.enabled == true && cameraComponent.enabled == true; }
            set
            {
                audioListenerComponent.enabled = value;
                cameraComponent.enabled = value;
            }
        }

        private void Awake()
        {
            audioListenerComponent = GetComponent<AudioListener>();            
            cameraComponent = GetComponent<Camera>();
            CameraComponentsAreActive = false;
        }

        //// DEBUG
        //private void Update()
        //{
        //    if(Input.GetKeyDown(KeyCode.Space))
        //    {
        //        PlayVignette();
        //    }
        //}


        private IEnumerator PlayVignetteCoroutine(bool transitionBackToSceneOnceComplete)
        {
            isPlayVignetteCoroutineRunning = true;

            if (!ScenePauseHandler.IsScenePaused)
            {
                ScenePauseHandler.PauseScene();
            }
            FadeHandler.FadeToColor(Color.black, FadeDuration);
            yield return new WaitForSecondsRealtime(FadeDuration);

            SceneFixedCameraHandler.SetAllFixedCamerasActiveState(false);
            CameraComponentsAreActive = true;

            Transform animatedVignetteObject = GameObject.Instantiate(animatedVignetteObjectPrefabForCoroutine, this.transform);
            

            FadeHandler.FadeToColor(Color.clear, FadeDuration);            

            Debug.Log(animatedVignetteObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
            yield return new WaitForSecondsRealtime(animatedVignetteObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - FadeDuration);

            FadeHandler.FadeToColor(Color.black, FadeDuration);
            yield return new WaitForSecondsRealtime(FadeDuration);

            Destroy(animatedVignetteObject.gameObject);


            if (transitionBackToSceneOnceComplete)
            {
                CameraComponentsAreActive = false;
                SceneFixedCameraHandler.SetAllFixedCamerasActiveState(true);

                FadeHandler.FadeToColor(Color.clear, FadeDuration);
                yield return new WaitForSecondsRealtime(FadeDuration);

                if (ScenePauseHandler.IsScenePaused)
                {
                    ScenePauseHandler.UnpauseScene();
                }
            }



            isPlayVignetteCoroutineRunning = false;
        }
    }
}
