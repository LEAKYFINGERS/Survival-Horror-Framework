////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.12.20
// Date last edited:    29.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    // The script for a camera used to display and manipulate items in the player's inventory.
    [RequireComponent(typeof(Camera))]
    public class InventoryItemCamera : MonoBehaviour
    {
        public InventoryItem TestInventoryItem;
        public float ItemSpinDuration = 1.5f;

        public void StartDisplayingInventoryItem(InventoryItem inventoryItem)
        {
            if(currentlyisplayedInventoryItem || isItemSpinCoroutineRunning)
            {
                throw new System.Exception("Cannot display inventory item " + inventoryItem.name + " because inventory item " + currentlyisplayedInventoryItem.name + " is already being displayed.");
            }
            else
            {
                StartCoroutine("ItemAppearSpinCoroutine", inventoryItem);
            }
        }


        private Transform currentlyisplayedInventoryItem;
        private bool isItemSpinCoroutineRunning;

        private IEnumerator ItemAppearSpinCoroutine(InventoryItem inventoryItem)
        {
            isItemSpinCoroutineRunning = true;

            currentlyisplayedInventoryItem = Instantiate(inventoryItem.ModelPrefab, this.transform);

            Vector3 startingLocalPos = new Vector3(0.0f, 0.0f, 10.0f);
            Vector3 startingLocalRotation = new Vector3(0.0f, 0.0f, 0.0f);
            Vector3 endingLocalPos = new Vector3(0.0f, 0.0f, 2.0f);
            Vector3 endingLocalRotation = new Vector3(0.0f, 1080.0f, 0.0f);
            float timer = 0.0f;
            while(timer < ItemSpinDuration)
            {
                currentlyisplayedInventoryItem.transform.localPosition = Vector3.Lerp(startingLocalPos, endingLocalPos, timer / ItemSpinDuration);
                currentlyisplayedInventoryItem.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingLocalRotation, endingLocalRotation, timer / ItemSpinDuration));

                timer += Time.deltaTime;
                yield return null;
            }
            currentlyisplayedInventoryItem.localPosition = endingLocalPos;                                  

            isItemSpinCoroutineRunning = false;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.I) && !currentlyisplayedInventoryItem) // DEBUG
            {
                StartDisplayingInventoryItem(TestInventoryItem);
            }
        }

        //public ColorTintPostProcessHandler FadeHandler;
        ////public FixedCameraHandler SceneFixedCameraHandler;
        ////public PauseHandler ScenePauseHandler;
        //public float FadeDuration = 0.5f; // The duration of each fade to/from black when a vignette is being played.

        //// Initialises a coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed.
        //public void PlayVignette(Transform animatedVignetteObjectPrefab)
        //{
        //    if (!isPlayVignetteCoroutineRunning)
        //    {
        //        coroutineSceneToLoadIndex = -1; // A scene will only be loaded if the index is greater than or equal to zero.
        //        StartCoroutine("PlayVignetteCoroutine", animatedVignetteObjectPrefab);
        //    }
        //    else
        //    {
        //        Debug.Log("The PlayVignette() function cannot be executed because the PlayVignetteCoroutine() is already running.");
        //    }
        //}

        //// Initialises a coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed and then loads the specified scene.
        //public void PlayVignetteAndLoadScene(Transform animatedVignetteObjectPrefab, int sceneToLoadIndex)
        //{
        //    if (!isPlayVignetteCoroutineRunning)
        //    {
        //        coroutineSceneToLoadIndex = sceneToLoadIndex;
        //        StartCoroutine("PlayVignetteCoroutine", animatedVignetteObjectPrefab);
        //    }
        //    else
        //    {
        //        Debug.Log("The PlayVignette() function cannot be executed because the PlayVignetteCoroutine() is already running.");
        //    }
        //}


        //private AudioListener audioListenerComponent;
        //private Camera cameraComponent;        
        //private bool isPlayVignetteCoroutineRunning;
        //private int coroutineSceneToLoadIndex; // Stores the index for the scene to load when the play vignette coroutine is completed - if the value is less than zero, disables the vignette camera and returns back to the current scene instead.

        //// The property used to get and set whether the audio listener and camera components of the vignette camera are currently active.
        //private bool CameraComponentsAreActive
        //{
        //    get { return audioListenerComponent.enabled == true && cameraComponent.enabled == true; }
        //    set
        //    {
        //        audioListenerComponent.enabled = value;
        //        cameraComponent.enabled = value;
        //    }
        //}

        //private void Awake()
        //{
        //    audioListenerComponent = GetComponent<AudioListener>();
        //    cameraComponent = GetComponent<Camera>();
        //    CameraComponentsAreActive = false;
        //}

        //// A coroutine which pauses the scene and disables the fixed camera handler before spawning the animated vignette object from a prefab to view through the vignette camera until the animation is completed.
        //private IEnumerator PlayVignetteCoroutine(Transform animatedVignetteObjectPrefab)
        //{
        //    isPlayVignetteCoroutineRunning = true;

        //    ScenePauseHandler.PauseScene();
        //    FadeHandler.FadeToColor(Color.black, FadeDuration);
        //    yield return new WaitForSecondsRealtime(FadeDuration);

        //    SceneFixedCameraHandler.SetAllFixedCamerasActiveState(false);
        //    CameraComponentsAreActive = true;
        //    Transform animatedVignetteObject = GameObject.Instantiate(animatedVignetteObjectPrefab, this.transform);
        //    FadeHandler.FadeToColor(Color.clear, FadeDuration);

        //    yield return new WaitForSecondsRealtime(animatedVignetteObject.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length - FadeDuration);

        //    FadeHandler.FadeToColor(Color.black, FadeDuration);
        //    yield return new WaitForSecondsRealtime(FadeDuration);

        //    Destroy(animatedVignetteObject.gameObject);

        //    if (coroutineSceneToLoadIndex < 0)
        //    {
        //        CameraComponentsAreActive = false;
        //        SceneFixedCameraHandler.SetAllFixedCamerasActiveState(true);
        //        FadeHandler.FadeToColor(Color.clear, FadeDuration);
        //        yield return new WaitForSecondsRealtime(FadeDuration);

        //        ScenePauseHandler.UnpauseScene();

        //        isPlayVignetteCoroutineRunning = false;
        //    }
        //    else
        //    {
        //        SceneManager.LoadScene(coroutineSceneToLoadIndex);
        //    }            
        //}
    }
}
