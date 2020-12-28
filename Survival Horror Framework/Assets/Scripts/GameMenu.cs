////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    28.12.20
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
        public enum MenuActivationMode
        {
            Default,
            AddItem
        }

        public AudioClip ActivateSelectedTileSound;
        public AudioClip ChangeSelectedTileSound;
        public AudioClip GoBackInMenuSound;
        public ColorTintPostProcessHandler FadeHandler;
        public Image BackgroundImage;
        public List<MenuTile> DefaultParentMenuTileGroup; // The initial group of menu tiles which are pushed onto the stack of menu tile groups when the menu is activated normally.
        public List<MenuTile> AddItemParentMenuTileGroup; // The initial group of menu tiles which are pushed onto the stack of menu tile groups when an item is to be added to the menu inventory.
        public PauseHandler ScenePauseHandler;
        public float ActivationFadeDuration = 0.25f;

        // Activates the menu if the scene isn't already currently paused (e.g. for camera transition stutter effect).
        public void ActivateMenu(MenuActivationMode activationMode = MenuActivationMode.Default)
        {
            if (!isMenuActive && !ScenePauseHandler.IsScenePaused && !isActivationFadeCoroutineRunning)
            {
                StartCoroutine("ActivateMenuCoroutine", activationMode);
            }
        }

        public void DeactivateMenu()
        {
            if (isMenuActive && ScenePauseHandler.IsScenePaused && !isActivationFadeCoroutineRunning)
            {
                StartCoroutine("DeactivateMenuCoroutine");
            }
        }

        // Pushes the specified menu tile group onto the stack and sets it as the current interactive 'layer' of the menu.
        public void PushMenuTileGroup(List<MenuTile> menuTileGroup)
        {
            menuTileGroups.Push(menuTileGroup);
            foreach (MenuTile menuTile in menuTileGroups.Peek())
            {
                menuTile.IsEnabled = true;
            }
            SetSelectedMenuTile(menuTileGroups.Peek()[0]);
        }


        private AudioSource audioSourceComponent;
        private Color activationFadeImageColor; // The initial color tint of the activation fade image - stored so the image can transition between this color and completely transparent.
        private Stack<List<MenuTile>> menuTileGroups; // The stack which contains each of the menu tile groups that form the different interactive 'layers' of the menu.
        private bool isMenuActive;
        private bool wasMenuInputDownDuringPreviousUpdate;
        private bool wasHorizontalInputDownDuringPreviousUpdate;
        private bool wasVerticalInputDownDuringPreviousUpdate;
        private bool wasUseInputDownDuringPreviousUpdate;
        private bool wasRunInputDownDuringPreviousUpdate;
        private bool isActivationFadeCoroutineRunning;
        private int currentlySelectedMenuTileIndex; // The index of the currently selected menu tile within the group that is currently on the top of the menu groups stack.

        private IEnumerator ActivateMenuCoroutine(MenuActivationMode activationMode)
        {
            isActivationFadeCoroutineRunning = true;

            ScenePauseHandler.PauseScene();            

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            BackgroundImage.gameObject.SetActive(true);

            if (activationMode == MenuActivationMode.Default)
            {
                SetParentMenuTileGroup(DefaultParentMenuTileGroup);
            }
            else if (activationMode == MenuActivationMode.AddItem)
            {
                SetParentMenuTileGroup(AddItemParentMenuTileGroup);
            }

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            isMenuActive = true;
            isActivationFadeCoroutineRunning = false;
        }

        private IEnumerator DeactivateMenuCoroutine()
        {
            isActivationFadeCoroutineRunning = true;

            audioSourceComponent.PlayOneShot(GoBackInMenuSound);

            // Fades the activation fade image from clear to opaque.           
            FadeHandler.FadeToColor(Color.black, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            BackgroundImage.gameObject.SetActive(false);

            // Fades the activation fade image back to clear.            
            FadeHandler.FadeToColor(Color.clear, ActivationFadeDuration / 2.0f);
            yield return new WaitForSecondsRealtime(ActivationFadeDuration / 2.0f);

            ScenePauseHandler.UnpauseScene();

            isMenuActive = false;
            isActivationFadeCoroutineRunning = false;
        }

        private void Awake()
        {
            audioSourceComponent = GetComponent<AudioSource>();

            BackgroundImage.gameObject.SetActive(false);

            isMenuActive = false;
        }

        private void Start()
        {
            menuTileGroups = new Stack<List<MenuTile>>();
        }

        private void Update()
        {
            // Toggles the active state of the menu if the 'Menu' input is pressed.
            if (Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
            {
                if (!isMenuActive)
                {
                    ActivateMenu(MenuActivationMode.Default);
                }
                else
                {
                    DeactivateMenu();
                }
            }

            if (isMenuActive)
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
            // Else if the 'Run' input has been pressed, pops the current menu group or deactivates the menu if there are is only one group remaining in the stack.
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

        // Pops all existing menu tile groups and pushes the specified menu tile group so that it becomes the new 'base' layer of the menu.
        private void SetParentMenuTileGroup(List<MenuTile> parentTileGroup)
        {
            if (parentTileGroup.Count == 0)
            {
                throw new System.Exception("The parent tile group must contain at least one MenuTile instance.");
            }

            for (int i = 0; i < menuTileGroups.Count; ++i)
            {
                PopMenuTileGroup();
            }            

            PushMenuTileGroup(parentTileGroup);
        }

        // If the specified menu tile belongs to the MenuTiles list updates the currently selected menu tile index so that the specified tile is the only one with the 'Selected' status.
        private void SetSelectedMenuTile(MenuTile menuTile, bool playSoundEffect = false)
        {
            for (int i = 0; i < menuTileGroups.Peek().Count; ++i)
            {
                if (menuTileGroups.Peek()[i] == menuTile)
                {
                    menuTileGroups.Peek()[currentlySelectedMenuTileIndex].IsSelected = false;

                    menuTileGroups.Peek()[i].IsSelected = true;
                    currentlySelectedMenuTileIndex = i;

                    if (playSoundEffect)
                    {
                        audioSourceComponent.PlayOneShot(ChangeSelectedTileSound);
                    }

                    break;
                }
            }
        }

        // Pops the menu tile group which forms the current interactive 'layer' of the menu off the stack so that the tiles in the next 'layer' down become active.
        private void PopMenuTileGroup()
        {
            SetSelectedMenuTile(menuTileGroups.Peek()[0]);
            foreach (MenuTile menuTile in menuTileGroups.Peek())
            {
                menuTile.IsEnabled = false;
            }
            menuTileGroups.Pop();

            if (menuTileGroups.Count > 0)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[0]);
            }
        }
    }
}
