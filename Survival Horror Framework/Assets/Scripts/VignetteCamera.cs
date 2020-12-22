////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        21.12.20
// Date last edited:    22.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    // The script for a camera used to play short animated vignettes used to convey interactions such as entering a door or going up stairs by spawning and viewing an animated object. 
    [RequireComponent(typeof(Camera))]
    public class VignetteCamera : MonoBehaviour
    {
        public ColorTintPostProcessHandler FadeHandler;
        public FixedCameraHandler SceneFixedCameraHandler;
        public PauseHandler ScenePauseHandler;
        public float FadeDuration = 0.5f; // The duration of each fade to/from black when a vignette is being played.

        // Initialises a coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed.
        public void PlayVignette(Transform animatedVignetteObjectPrefab)
        {
            if (!isPlayVignetteCoroutineRunning)
            {
                coroutineSceneToLoadIndex = -1; // A scene will only be loaded if the index is greater than or equal to zero.
                StartCoroutine("PlayVignetteCoroutine", animatedVignetteObjectPrefab);
            }
            else
            {
                Debug.Log("The PlayVignette() function cannot be executed because the PlayVignetteCoroutine() is already running.");
            }
        }

        // Initialises a coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed and then loads the specified scene.
        public void PlayVignetteAndLoadScene(Transform animatedVignetteObjectPrefab, int sceneToLoadIndex)
        {
            if (!isPlayVignetteCoroutineRunning)
            {
                coroutineSceneToLoadIndex = sceneToLoadIndex;
                StartCoroutine("PlayVignetteCoroutine", animatedVignetteObjectPrefab);
            }
            else
            {
                Debug.Log("The PlayVignette() function cannot be executed because the PlayVignetteCoroutine() is already running.");
            }
        }


        private AudioListener audioListenerComponent;
        private Camera cameraComponent;        
        private bool isPlayVignetteCoroutineRunning;
        private int coroutineSceneToLoadIndex; // Stores the index for the scene to load when the play vignette coroutine is completed - if the value is less than zero, disables the vignette camera and returns back to the current scene instead.

        // The property used to get and set whether the audio listener and camera components of the vignette camera are currently active.
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

        // A coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed.
        private IEnumerator PlayVignetteCoroutine(Transform animatedVignetteObjectPrefab)
        {
            isPlayVignetteCoroutineRunning = true;
            
            ScenePauseHandler.PauseScene();
            FadeHandler.FadeToColor(Color.black, FadeDuration);
            yield return new WaitForSecondsRealtime(FadeDuration);

            SceneFixedCameraHandler.SetAllFixedCamerasActiveState(false);
            CameraComponentsAreActive = true;
            Transform animatedVignetteObject = GameObject.Instantiate(animatedVignetteObjectPrefab, this.transform);
            FadeHandler.FadeToColor(Color.clear, FadeDuration);

            yield return new WaitForSecondsRealtime(animatedVignetteObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - FadeDuration);

            FadeHandler.FadeToColor(Color.black, FadeDuration);
            yield return new WaitForSecondsRealtime(FadeDuration);

            Destroy(animatedVignetteObject.gameObject);

            if (coroutineSceneToLoadIndex < 0)
            {
                CameraComponentsAreActive = false;
                SceneFixedCameraHandler.SetAllFixedCamerasActiveState(true);
                FadeHandler.FadeToColor(Color.clear, FadeDuration);
                yield return new WaitForSecondsRealtime(FadeDuration);

                ScenePauseHandler.UnpauseScene();

                isPlayVignetteCoroutineRunning = false;
            }
            else
            {
                SceneManager.LoadScene(coroutineSceneToLoadIndex);
            }            
        }
    }
}
