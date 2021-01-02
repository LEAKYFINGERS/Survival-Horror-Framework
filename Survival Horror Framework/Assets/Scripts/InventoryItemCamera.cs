////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.12.20
// Date last edited:    02.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    // The script for a camera used to display and manipulate items in the player's inventory.
    [RequireComponent(typeof(Camera))]
    public class InventoryItemCamera : MonoBehaviour
    {
        public Image FadeImage; // The UI image placed in front of the camera used to create a 'fade' effect when an inventory item appears/disappears by changing the opacity.
        public Vector3 InventoryItemDisplayCoroutineSpawnRotation = new Vector3(720.0f, 720.0f, 0.0f); // The local rotation of the displayed inventory items which will lerp towards the local rotation value of the 3D model prefab when they move in front of the camera - used to create the classic 'item appear spin effect'.
        public float InventoryItemDisplayCoroutineDuration = 1.5f; // The duration of the spinning 'animations' which play when an inventory item starts/stops being displayed.
        public float InventoryItemDisplayCoroutineSpawnDistanceFromCamera = 10.0f; // The local distance from the inventory item camera at which the inventory item spawns before being displayed/despawns after being displayed.        

        public bool IsDisplayingAnInventoryItem
        {
            get { return currentlyDisplayedInventoryItem != null; }
        }

        // Initialises a coroutine which spawns the 3D model representing the specified inventory item and moves it in front of the inventory item camera to be displayed at the local position and rotation of the 3D model prefab.
        public void DisplayInventoryItem(InventoryItem inventoryItem)
        {
            if(currentlyDisplayedInventoryItem || isItemDisplayHandlingCoroutineRunning)
            {
                throw new System.Exception("Cannot display inventory item " + inventoryItem.name + " because inventory item " + currentlyDisplayedInventoryItem.name + " is already being displayed.");
            }
            else
            {
                StartCoroutine("StartDisplayingItemCoroutine", inventoryItem);
            }
        }

        // Initialises a coroutine which removes the currently displayed 3D model of an inventory item from the view of the inventory item camera and destroys it. 
        public void StopDisplayingInventoryItem()
        {
            if(!currentlyDisplayedInventoryItem)
            {
                Debug.Log("StopDisplayingInventoryItem() has been called on " + name + " but no inventory items are currently being displayed.");
            }
            else if(!isItemDisplayHandlingCoroutineRunning)
            {
                StartCoroutine("StopDisplayingItemCoroutine");
            }
        }


        private Color fadeImageColor; // The color of the fade image at the beginning of the scene.
        private Transform currentlyDisplayedInventoryItem;
        private bool isItemDisplayHandlingCoroutineRunning;

        // A corutine which spawns the 3D model representing the specified inventory item and moves it from a set distance/rotation into the view of the camera offset according to the local position/rotation of the 3D model prefab.
        private IEnumerator StartDisplayingItemCoroutine(InventoryItem inventoryItem)
        {
            isItemDisplayHandlingCoroutineRunning = true;

            currentlyDisplayedInventoryItem = Instantiate(inventoryItem.ModelPrefab, this.transform);

            Vector3 startingLocalPos = new Vector3(0.0f, 0.0f, InventoryItemDisplayCoroutineSpawnDistanceFromCamera);
            Vector3 startingLocalRotation = InventoryItemDisplayCoroutineSpawnRotation;
            Vector3 endingLocalPos = new Vector3(0.0f, 0.0f, currentlyDisplayedInventoryItem.localPosition.z);
            Vector3 endingLocalRotation = currentlyDisplayedInventoryItem.localRotation.eulerAngles;
            float timer = 0.0f;
            while(timer < InventoryItemDisplayCoroutineDuration)
            {
                FadeImage.color = Color.Lerp(fadeImageColor, Color.clear, timer / InventoryItemDisplayCoroutineDuration);

                currentlyDisplayedInventoryItem.transform.localPosition = Vector3.Lerp(startingLocalPos, endingLocalPos, timer / InventoryItemDisplayCoroutineDuration);
                currentlyDisplayedInventoryItem.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingLocalRotation, endingLocalRotation, timer / InventoryItemDisplayCoroutineDuration));

                timer += Time.deltaTime;
                yield return null;
            }
            currentlyDisplayedInventoryItem.localPosition = endingLocalPos;                                  

            isItemDisplayHandlingCoroutineRunning = false;
        }

        // A corutine moves the currently displayed 3D model of an inventory item from in front of the camera to a set distance/rotation before destroying it.
        private IEnumerator StopDisplayingItemCoroutine()
        {
            isItemDisplayHandlingCoroutineRunning = true;

            Vector3 startingLocalPos = currentlyDisplayedInventoryItem.localPosition;
            Vector3 startingLocalRotation = currentlyDisplayedInventoryItem.localRotation.eulerAngles;
            Vector3 endingLocalPos = new Vector3(0.0f, 0.0f, InventoryItemDisplayCoroutineSpawnDistanceFromCamera);
            Vector3 endingLocalRotation = InventoryItemDisplayCoroutineSpawnRotation;
            float timer = 0.0f;
            while (timer < InventoryItemDisplayCoroutineDuration)
            {
                FadeImage.color = Color.Lerp(Color.clear, fadeImageColor, timer / InventoryItemDisplayCoroutineDuration);

                currentlyDisplayedInventoryItem.transform.localPosition = Vector3.Lerp(startingLocalPos, endingLocalPos, timer / InventoryItemDisplayCoroutineDuration);
                currentlyDisplayedInventoryItem.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingLocalRotation, endingLocalRotation, timer / InventoryItemDisplayCoroutineDuration));

                timer += Time.deltaTime;
                yield return null;
            }

            Destroy(currentlyDisplayedInventoryItem.gameObject);

            isItemDisplayHandlingCoroutineRunning = false;
        }

        //private IEnumerator DisplayItemCoroutine(InventoryItem inventoryItem)
        //{
        //    isItemSpinCoroutineRunning = true;

        //    currentlyisplayedInventoryItem = Instantiate(inventoryItem.ModelPrefab, this.transform);

        //    Vector3 startingLocalPos = new Vector3(0.0f, 0.0f, 10.0f);
        //    Vector3 startingLocalRotation = new Vector3(0.0f, 0.0f, 0.0f);
        //    Vector3 endingLocalPos = new Vector3(0.0f, 0.0f, 2.0f);
        //    Vector3 endingLocalRotation = new Vector3(0.0f, 1080.0f, 0.0f);
        //    float timer = 0.0f;
        //    while (timer < ItemSpinDuration)
        //    {
        //        currentlyisplayedInventoryItem.transform.localPosition = Vector3.Lerp(startingLocalPos, endingLocalPos, timer / ItemSpinDuration);
        //        currentlyisplayedInventoryItem.transform.localRotation = Quaternion.Euler(Vector3.Lerp(startingLocalRotation, endingLocalRotation, timer / ItemSpinDuration));

        //        timer += Time.deltaTime;
        //        yield return null;
        //    }
        //    currentlyisplayedInventoryItem.localPosition = endingLocalPos;

        //    isItemSpinCoroutineRunning = false;
        //}

        private void Awake()
        {
            fadeImageColor = FadeImage.color;
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
