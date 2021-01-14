////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        29.12.20
// Date last edited:    14.01.21
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
        public float DisplayedItemRotateSpeedInDegrees = 90.0f;        

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

        // If an item is currently being displayed, rotates it according to the DisplayedItemRotateSpeedInDegrees member value and the normalized direction vector to dicate the horizontal and vertical spin directions.
        public void RotateDisplayedItem(Vector2 direction)
        {
            if (!IsDisplayingAnInventoryItem)
            {
                throw new System.Exception("The InventoryItemCamera " + name + " cannot rotate the currently displayed item because no item is being displayed.");
            }
            
            if(direction == Vector2.zero)
            {
                return;
            }
            else
            {
                direction.Normalize();

                currentlyDisplayedInventoryItem.RotateAround(currentlyDisplayedInventoryItem.position, Vector3.down, direction.x * DisplayedItemRotateSpeedInDegrees * Time.deltaTime);
                currentlyDisplayedInventoryItem.RotateAround(currentlyDisplayedInventoryItem.position, Vector3.left, direction.y * DisplayedItemRotateSpeedInDegrees * Time.deltaTime);
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
        
        private void Awake()
        {
            fadeImageColor = FadeImage.color;
        }
    }
}
