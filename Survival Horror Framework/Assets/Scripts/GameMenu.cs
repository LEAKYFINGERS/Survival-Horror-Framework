////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    01.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Canvas))]
    // The script used to handle the collection of objects which make up the in-game menu.
    public class GameMenu : MonoBehaviour
    {
        public delegate void GameMenuEventHandler();
        public enum MenuMode
        {
            Default,
            PickUpItem,
            InventoryFull,
            CheckInventoryItem
        }

        public AudioClip ActivateSelectedTileSound;
        public AudioClip ChangeSelectedTileSound;
        public AudioClip GoBackInMenuSound;
        public ColorTintPostProcessHandler FadeHandler;
        public Dialog PickUpItemDialogTemplate; // The dialog asking the player whether they wish to add a item they've 'picked up' to their inventory - the PickUpItem's InventoryItem name + a question mark will be appended onto the end of the dialog template.
        public Dialog InventoryFullDialog;
        public DialogDisplay MenuDialogDisplay;
        public event GameMenuEventHandler OnPopMenuTileGroup;
        public Image BackgroundImage;
        public InventoryItemCamera SceneInventoryItemCamera; // The camera used to render the 3D models which represent inventory items within the menu.
        public InventoryTile[] InventoryTiles; // An array pointing to the inventory tiles used to store the inventory items currently possessed by the player.
        public List<MenuTile> DefaultParentMenuTileGroup; // The initial group of menu tiles which are pushed onto the stack of menu tile groups when the menu is activated normally.
        public List<MenuTile> AddItemParentMenuTileGroup; // The initial group of menu tiles which are pushed onto the stack of menu tile groups when an item is to be added to the menu inventory.
        public PauseHandler ScenePauseHandler;
        public float ActivationFadeDuration = 0.25f;

        // The property used to get the currently selected menu tile within the menu.
        public MenuTile SelectedMenuTile
        {
            get { return menuTileGroups.Peek()[currentlySelectedMenuTileIndex]; }
        }
        
        // Returns whether the specified inventory item can be added to either an empty inventory tile or one which contains other instances of the same inventory item and isn't full.
        public bool CanItemBeAddedToInventory(InventoryItem itemToAdd)
        {
            foreach (InventoryTile inventoryTile in InventoryTiles)
            {
                if (inventoryTile.IsEmpty || inventoryTile.StoredInventoryItemDisplayName == itemToAdd.DisplayName && !inventoryTile.IsFull)
                {
                    return true;
                }
            }

            return false;
        }        

        // Attempts to add the specified item to one of the inventory tiles in the menu - returns 'true' if successful, else if all available inventory slots are full returns 'false'.
        public bool TryAddItemToPlayerInventory(InventoryItem itemToAdd)
        {
            bool itemAdded = false;
            foreach (InventoryTile inventoryTile in InventoryTiles)
            {
                if (inventoryTile.IsEmpty || inventoryTile.StoredInventoryItemDisplayName == itemToAdd.DisplayName && !inventoryTile.IsFull) //(inventoryTile.IsEmpty || inventoryTile.StoredInventoryItemName == itemToAdd.name && !inventoryTile.IsFull)
                {
                    inventoryTile.StoreInventoryItem(itemToAdd);
                    itemAdded = true;
                    break;
                }
            }

            return itemAdded;
        }

        // Activates the menu in the default mode if the scene isn't already currently paused.
        public void ActivateMenuInDefaultMode()
        {
            if (!isMenuActive && !ScenePauseHandler.IsScenePaused && !isMenuProcessCoroutineRunning)
            {
                currentMenuMode = MenuMode.Default;
                StartCoroutine("ActivateMenuInDefaultModeCoroutine");
            }
        }

        // If at least one inventory tile is empty activates the menu and allows the player to choose to add the specified PickUpItem to their inventory, else if the inventory is full activates it in a manner that tells the player it is full before exiting.
        public void ActivateMenuAndTryToAddItem(PickupItem pickUpItem)
        {
            if (!isMenuActive && !ScenePauseHandler.IsScenePaused && !isMenuProcessCoroutineRunning)
            {
                if (CanItemBeAddedToInventory(pickUpItem.InventoryRepresentation))
                {
                    currentMenuMode = MenuMode.PickUpItem;
                    StartCoroutine("ActivateMenuInPickUpItemModeCoroutine", pickUpItem);
                }
                else
                {
                    currentMenuMode = MenuMode.InventoryFull;
                    StartCoroutine("ActivateMenuInInventoryFullModeCoroutine", pickUpItem);
                }
            }
        }

        // If the menu is currently in 'Default' mode, begins the 'check item' process in which the specified inventory item is displayed to be viewed and examined.
        public void BeginCheckInventoryItemProcess(InventoryItem inventoryItem)
        {
            if (isMenuActive && currentMenuMode == MenuMode.Default && !isMenuProcessCoroutineRunning)
            {
                currentMenuMode = MenuMode.CheckInventoryItem;
                StartCoroutine("CheckInventoryItemCoroutine", inventoryItem);
            }
        }

        public void DeactivateMenu()
        {
            if (isMenuActive && ScenePauseHandler.IsScenePaused && !isMenuProcessCoroutineRunning)
            {
                StartCoroutine("DeactivateMenuCoroutine");
            }
        }

        // Pops all existing menu tile groups and pushes the specified menu tile group so that it becomes the new 'base' layer of the menu.
        public void SetParentMenuTileGroup(List<MenuTile> parentTileGroup)
        {
            if (parentTileGroup.Count == 0)
            {
                throw new System.Exception("The parent tile group must contain at least one MenuTile instance.");
            }

            while(menuTileGroups.Count > 0)
            {
                PopMenuTileGroup();
            }

            PushMenuTileGroup(parentTileGroup);
        }
        // Pops all existing menu tile groups and pushes the specified menu tile group so that it becomes the new 'base' layer of the menu as well as setting the specified menu tile as the selected tile.
        public void SetParentMenuTileGroup(List<MenuTile> parentTileGroup, MenuTile selectedTile)
        {
            if (parentTileGroup.Count == 0)
            {
                throw new System.Exception("The parent tile group must contain at least one MenuTile instance.");
            }

            while (menuTileGroups.Count > 0)
            {
                PopMenuTileGroup();
            }

            PushMenuTileGroup(parentTileGroup);
            SetSelectedMenuTile(selectedTile);
        }

        // Pushes the specified menu tile group onto the stack and sets it as the current interactive 'layer' of the menu and selects the first tile in the list.
        public void PushMenuTileGroup(List<MenuTile> menuTileGroup)
        {
            menuTileGroups.Push(menuTileGroup);
            foreach (MenuTile menuTile in menuTileGroups.Peek())
            {
                menuTile.IsEnabled = true;
            }
            SetSelectedMenuTile(menuTileGroups.Peek()[0]);
        }
        
        // If the specified menu tile belongs to the MenuTiles list updates the currently selected menu tile index so that the specified tile is the only one with the 'Selected' status.
        public void SetSelectedMenuTile(MenuTile menuTile, bool playSoundEffect = false)
        {
            for (int i = 0; i < menuTileGroups.Peek().Count; ++i)
            {
                if (menuTileGroups.Peek()[i] == menuTile)
                {
                    menuTileGroups.Peek()[i].IsSelected = true;
                    currentlySelectedMenuTileIndex = i;

                    // If the currently selected tile is an inventory tile updates the menu dialog display to show it's name, else clears it.
                    InventoryTile selectedInventoryTile = menuTileGroups.Peek()[i].GetComponent<InventoryTile>();
                    if (selectedInventoryTile != null)
                    {
                        if (!selectedInventoryTile.IsEmpty)
                        {
                            MenuDialogDisplay.DisplayBasicText(selectedInventoryTile.StoredInventoryItemDisplayName);
                        }
                        else
                        {
                            MenuDialogDisplay.DisplayBasicText("");
                        }
                    }

                    if (playSoundEffect)
                    {
                        audioSourceComponent.PlayOneShot(ChangeSelectedTileSound);
                    }
                }
                else
                {
                    menuTileGroups.Peek()[i].IsSelected = false;
                }
            }
        }


        private AudioSource audioSourceComponent;
        private Color activationFadeImageColor; // The initial color tint of the activation fade image - stored so the image can transition between this color and completely transparent.
        private Stack<List<MenuTile>> menuTileGroups; // The stack which contains each of the menu tile groups that form the different interactive 'layers' of the menu.
        private MenuMode currentMenuMode;
        private bool isMenuActive;
        private bool wasMenuInputDownDuringPreviousUpdate;
        private bool wasHorizontalInputDownDuringPreviousUpdate;
        private bool wasVerticalInputDownDuringPreviousUpdate;
        private bool wasUseInputDownDuringPreviousUpdate;
        private bool wasRunInputDownDuringPreviousUpdate;        
        private bool isMenuProcessCoroutineRunning; // A flag used to specify whether the menu is currently running in it's default state or is going through some sequential process handled by a coroutine (activating, viewing an item, deactivation, etc.)
        private bool hasDialogDisplayFinishedDisplayingAllSnippets; // Whether the menu dialog display has finished displaying all the current snippets and is awaiting user input to proceed.
        private bool hasDialogDisplayExitedDialog; // Whether the menu dialog display has finished displaying all the current snippets and the player has pressed an input to 'exit' the dialog process.
        private int currentlySelectedMenuTileIndex; // The index of the currently selected menu tile within the group that is currently on the top of the menu groups stack.

        private IEnumerator ActivateMenuInDefaultModeCoroutine()
        {
            isMenuProcessCoroutineRunning = true;

            ScenePauseHandler.PauseScene();

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            BackgroundImage.gameObject.SetActive(true);

            SetParentMenuTileGroup(DefaultParentMenuTileGroup);

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            MenuDialogDisplay.UIText.enabled = true;
            isMenuActive = true;
            isMenuProcessCoroutineRunning = false;
        }

        private IEnumerator ActivateMenuInPickUpItemModeCoroutine(PickupItem pickUpItem)
        {
            isMenuProcessCoroutineRunning = true;

            ScenePauseHandler.PauseScene();

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            BackgroundImage.gameObject.SetActive(true);

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            // Displays the 3D model which represents the item to be added.
            SceneInventoryItemCamera.DisplayInventoryItem(pickUpItem.InventoryRepresentation);
            yield return new WaitForSecondsRealtime(SceneInventoryItemCamera.InventoryItemDisplayCoroutineDuration);

            // Displays dialog asking the player whether they want to add the item to their inventory or not.
            Dialog PickUpItemDialog = Instantiate(PickUpItemDialogTemplate);
            PickUpItemDialog.DisplayedText += pickUpItem.InventoryRepresentation.DisplayName + '?';
            MenuDialogDisplay.DisplayDialog(PickUpItemDialog, false);
            hasDialogDisplayFinishedDisplayingAllSnippets = false;
            while (!hasDialogDisplayFinishedDisplayingAllSnippets)
            {
                yield return new WaitForEndOfFrame();
            }

            // Activates the 'yes' and 'no' menu tiles so the player can choose whether to add the item to their inventory.
            SetParentMenuTileGroup(AddItemParentMenuTileGroup);
            // Stores the PickUpItem to be added in the 'confirm/yes' menu tile so that it can call TryAddItemToPlayerInventory() when activated.
            foreach (MenuTile menuTile in menuTileGroups.Peek())
            {
                if (menuTile.gameObject.GetComponent<ConfirmAddItemToInventoryMenuTile>())
                {
                    menuTile.gameObject.GetComponent<ConfirmAddItemToInventoryMenuTile>().ItemToAdd = pickUpItem;
                    break;
                }
            }

            MenuDialogDisplay.UIText.enabled = true;
            isMenuActive = true;
            isMenuProcessCoroutineRunning = false;
        }

        private IEnumerator ActivateMenuInInventoryFullModeCoroutine(PickupItem pickUpItem)
        {
            isMenuProcessCoroutineRunning = true;

            ScenePauseHandler.PauseScene();

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            BackgroundImage.gameObject.SetActive(true);

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            // Displays the 3D model which represents the item the player is attempting to add to their inventory.
            SceneInventoryItemCamera.DisplayInventoryItem(pickUpItem.InventoryRepresentation);
            yield return new WaitForSecondsRealtime(SceneInventoryItemCamera.InventoryItemDisplayCoroutineDuration);

            // Displays dialog telling the player that their inventory is already full.
            MenuDialogDisplay.DisplayDialog(InventoryFullDialog, false);
            hasDialogDisplayFinishedDisplayingAllSnippets = false;
            while (!hasDialogDisplayFinishedDisplayingAllSnippets)
            {
                yield return new WaitForEndOfFrame();
            }

            MenuDialogDisplay.UIText.enabled = true;
            isMenuActive = true;
            isMenuProcessCoroutineRunning = false;
        }

        private IEnumerator CheckInventoryItemCoroutine(InventoryItem inventoryItem)
        {
            isMenuProcessCoroutineRunning = true;

            // Pops the 'item interaction' menu tiles so they're no longer visible/interactable and stores them to be re-pushed when the coroutine is completed.
            List<MenuTile> itemInteractionMenuTileGroup = menuTileGroups.Peek();
            PopMenuTileGroup();

            // Displays the 3D model which represents the item the player is checking.
            SceneInventoryItemCamera.DisplayInventoryItem(inventoryItem);
            yield return new WaitForSecondsRealtime(SceneInventoryItemCamera.InventoryItemDisplayCoroutineDuration);

            while (true)
            {
                // If the 'Use' input is pressed, pauses the examine process and displays the examine dialog of the inventory item.
                if (Input.GetAxis("Use") == 1.0f && !wasUseInputDownDuringPreviousUpdate)
                {
                    MenuDialogDisplay.DisplayDialog(inventoryItem.ExamineDialog, false);
                    hasDialogDisplayExitedDialog = false;
                    while(!hasDialogDisplayExitedDialog)
                    {
                        yield return null;
                    }
                    MenuDialogDisplay.DisplayBasicText(inventoryItem.DisplayName);
                }

                // Allows the player to spin the displayed inventory item.
                if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
                {
                    SceneInventoryItemCamera.RotateDisplayedItem(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
                }

                // If the 'Menu' input is pressed, exits the menu - the DeactivateMenu coroutine is directly called because it doesn't check if isMenuProcessCoroutineRunning is already true.
                if (Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
                {
                    StartCoroutine("DeactivateMenuCoroutine");
                    yield break;
                }
                // Else if the 'Run' input is pressed, breaks the 'check item' loop.
                else if (Input.GetAxis("Run") == 1.0f && !wasRunInputDownDuringPreviousUpdate)
                {
                    break;
                }

                yield return null;
            }

            SceneInventoryItemCamera.StopDisplayingInventoryItem();
            yield return new WaitForSecondsRealtime(SceneInventoryItemCamera.InventoryItemDisplayCoroutineDuration);

            // Pushes the item interaction tiles back as the current menu tile layer and re-selects the 'Check Item' tile.
            PushMenuTileGroup(itemInteractionMenuTileGroup);
            foreach (MenuTile itemInteractionMenuTile in itemInteractionMenuTileGroup)
            {
                if (itemInteractionMenuTile.GetComponent<CheckItemMenuTile>())
                {
                    SetSelectedMenuTile(itemInteractionMenuTile);
                    break;
                }
            }

            isMenuProcessCoroutineRunning = false;
            currentMenuMode = MenuMode.Default;
        }

        private IEnumerator DeactivateMenuCoroutine()
        {
            isMenuProcessCoroutineRunning = true;

            audioSourceComponent.PlayOneShot(GoBackInMenuSound);

            // If the 'pick up item' dialog is currently being displayed, removes the 'yes' and 'no' tiles before continuing to deactivate the menu.
            if (currentMenuMode == MenuMode.PickUpItem)
            {
                PopMenuTileGroup();
            }
            // If an inventory item is currently being displayed in the menu, stops displaying it.
            if (SceneInventoryItemCamera.IsDisplayingAnInventoryItem)
            {
                SceneInventoryItemCamera.StopDisplayingInventoryItem();
                yield return new WaitForSecondsRealtime(SceneInventoryItemCamera.InventoryItemDisplayCoroutineDuration);
            }

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            PopAndDeselectAllMenuTileGroups();
            BackgroundImage.gameObject.SetActive(false);

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            ScenePauseHandler.UnpauseScene();

            MenuDialogDisplay.UIText.enabled = false;
            isMenuActive = false;
            isMenuProcessCoroutineRunning = false;
        }

        private void Awake()
        {
            audioSourceComponent = GetComponent<AudioSource>();

            BackgroundImage.gameObject.SetActive(false);

            MenuDialogDisplay.enabled = false;
            isMenuActive = false;

            MenuDialogDisplay.OnAllDialogSnippetsDisplayCompleted += SetDialogDisplayFinishedDisplayingAllSnippetsFlagToTrue;
            MenuDialogDisplay.OnDialogExited += SetDialogDisplayExitedFlagToTrue;
        }

        private void Start()
        {
            menuTileGroups = new Stack<List<MenuTile>>();
        }

        private void Update()
        {
            // Toggles the active state of the default menu if the 'Menu' input is pressed.
            if (Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
            {
                if (!isMenuActive)
                {
                    ActivateMenuInDefaultMode();
                }
                else if (currentMenuMode == MenuMode.Default)
                {
                    DeactivateMenu();
                }
            }
            // Else if the menu is displaying that the inventory is full, exits the menu if any of the menu interaction keys are pressed.
            else if (isMenuActive && currentMenuMode == MenuMode.InventoryFull)
            {
                if (Input.GetAxis("Use") == 1.0f && !wasUseInputDownDuringPreviousUpdate || Input.GetAxis("Run") == 1.0f && !wasRunInputDownDuringPreviousUpdate || Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
                {
                    DeactivateMenu();
                }
            }

            if (isMenuActive && !isMenuProcessCoroutineRunning && currentMenuMode != MenuMode.CheckInventoryItem)
            {
                UpdateMenuTiles();
            }
        }

        private void LateUpdate()
        {
            wasMenuInputDownDuringPreviousUpdate = Input.GetAxis("Menu") == 1.0f;
            wasHorizontalInputDownDuringPreviousUpdate = Input.GetAxis("Horizontal") != 0.0f;
            wasVerticalInputDownDuringPreviousUpdate = Input.GetAxis("Vertical") != 0.0f;
            wasUseInputDownDuringPreviousUpdate = Input.GetAxis("Use") == 1.0f;
            wasRunInputDownDuringPreviousUpdate = Input.GetAxis("Run") == 1.0f;
        }

        private void UpdateMenuTiles()
        {
            if (menuTileGroups.Count == 0)
            {
                return;
            }

            // Updates which is the currently selected menu tile according to the player selection inputs.
            if (Input.GetAxis("Horizontal") == -1.0f && !wasHorizontalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToLeft && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToLeft.IsEnabled)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToLeft, true);
            }
            else if (Input.GetAxis("Horizontal") == 1.0f && !wasHorizontalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToRight && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToRight.IsEnabled)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToRight, true);
            }
            else if (Input.GetAxis("Vertical") == 1.0f && !wasVerticalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileAbove && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileAbove.IsEnabled)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileAbove, true);
            }
            else if (Input.GetAxis("Vertical") == -1.0f && !wasVerticalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileBelow && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileBelow.IsEnabled)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileBelow, true);
            }

            // If the 'Use' input has been pressed activates the currently selected menu tile.
            if (Input.GetAxis("Use") == 1.0f && !wasUseInputDownDuringPreviousUpdate)
            {
                if (menuTileGroups.Peek()[currentlySelectedMenuTileIndex].PlayMenuActivationSoundOnActivateTile)
                {
                    audioSourceComponent.PlayOneShot(ActivateSelectedTileSound);
                }

                menuTileGroups.Peek()[currentlySelectedMenuTileIndex].ActivateTile(this);
            }
            // Else if the 'Run' input has been pressed, pops the current menu group or deactivates the menu if there are is only one group remaining in the stack and the menu mode is 'default'.
            else if (Input.GetAxis("Run") == 1.0f && !wasRunInputDownDuringPreviousUpdate)
            {
                audioSourceComponent.PlayOneShot(GoBackInMenuSound);

                if (menuTileGroups.Count > 1)
                {
                    PopMenuTileGroup();
                }
                else
                {
                    DeactivateMenu();
                }
            }
        }               
                
        private void PopAndDeselectAllMenuTileGroups()
        {
            while (menuTileGroups.Count > 0)
            {
                foreach (MenuTile menuTile in menuTileGroups.Peek())
                {
                    menuTile.IsSelected = false;
                }
                PopMenuTileGroup();
            }
        }

        // Pops the menu tile group which forms the current interactive 'layer' of the menu off the stack so that the tiles in the next 'layer' down become active and sets the currently selected tile to that which had been selected previously in that layer.
        private void PopMenuTileGroup()
        {
            if (menuTileGroups.Count == 0)
            {
                return;
            }

            // Disables each of the menu tiles in the current layer and pops it.
            foreach (MenuTile menuTile in menuTileGroups.Peek())
            {
                menuTile.IsEnabled = false;
            }
            menuTileGroups.Pop();

            // Searches through the menu tiles of the new layer and updates the currently selected tile index to match that of the tile which had been selected previously.
            if (menuTileGroups.Count > 0)
            {
                for (int i = 0; i < menuTileGroups.Peek().Count; ++i)
                {
                    if (menuTileGroups.Peek()[i].IsSelected)
                    {
                        currentlySelectedMenuTileIndex = i;
                        break;
                    }
                }
            }

            if (OnPopMenuTileGroup != null)
            {
                OnPopMenuTileGroup.Invoke();
            }
        }

        private void SetDialogDisplayFinishedDisplayingAllSnippetsFlagToTrue()
        {
            hasDialogDisplayFinishedDisplayingAllSnippets = true;
        }

        private void SetDialogDisplayExitedFlagToTrue()
        {
            hasDialogDisplayExitedDialog = true;
        }
    }
}
