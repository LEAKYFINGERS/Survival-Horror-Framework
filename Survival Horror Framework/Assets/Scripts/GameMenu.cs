////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    22.11.20
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
        public AudioClip ActivateSelectedTileSound;
        public AudioClip ChangeSelectedTileSound;
        public AudioClip GoBackInMenuSound;
        public Image ActivationFadeImage; // The UI image (usually solid black) which fades in and out each time the menu transitions between activated and deactivated.
        public Image BackgroundImage;
        public List<MenuTile> ParentMenuTileGroup; // The initial group of menu tiles which are pushed onto the stack of menu tile groups and thus form the first interactive 'layer' of the menu.
        public PauseHandler ScenePauseHandler;        
        public float ActivationFadeDuration = 0.25f;

        // Activates or deactivates the menu if the scene isn't already currently paused (e.g. for camera transition stutter effect).
        public void SetMenuActiveState(bool activeState)
        {
            if (!ScenePauseHandler.IsScenePaused || ScenePauseHandler.IsScenePaused && isMenuActive && !activeState)
            {
                if (!isActivationFadeCoroutineRunning)
                {
                    StartCoroutine("MenuActivationCoroutine", activeState);
                }
            }
        }

        // Pushes the specified menu tile group onto the stack and sets it as the current interactive 'layer' of the menu.
        public void PushMenuTileGroup(List<MenuTile> menuTileGroup)
        {
            menuTileGroups.Push(menuTileGroup);
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

        // A coroutine which fades the activation fade image from clear to opaque and back again while the menu itself is activated or deactivated.
        private IEnumerator MenuActivationCoroutine(bool isBeingActivated)
        {
            isActivationFadeCoroutineRunning = true;

            if (isBeingActivated)
            {
                ScenePauseHandler.PauseScene();              
            }
            else
            {
                audioSourceComponent.PlayOneShot(GoBackInMenuSound);
            }

            // Fades the activation fade image from clear to opaque.
            float timer = 0.0f;
            while (timer < ActivationFadeDuration / 2.0f)
            {
                ActivationFadeImage.color = Color.Lerp(Color.clear, activationFadeImageColor, timer / (ActivationFadeDuration / 2.0f));

                timer += Time.deltaTime;
                yield return null;
            }
            ActivationFadeImage.color = activationFadeImageColor;

            BackgroundImage.gameObject.SetActive(isBeingActivated);

            // Fades the activation fade image back to clear.
            timer = 0.0f;
            while (timer < ActivationFadeDuration / 2.0f)
            {
                ActivationFadeImage.color = Color.Lerp(activationFadeImageColor, Color.clear, timer / (ActivationFadeDuration / 2.0f));

                timer += Time.deltaTime;
                yield return null;
            }
            ActivationFadeImage.color = Color.clear;

            if (!isBeingActivated)
            {
                ResetMenuToDefaultState();
                ScenePauseHandler.UnpauseScene();
            }

            isMenuActive = isBeingActivated;

            isActivationFadeCoroutineRunning = false;
        }

        private void Awake()
        {
            audioSourceComponent = GetComponent<AudioSource>();

            activationFadeImageColor = ActivationFadeImage.color;
            ActivationFadeImage.color = Color.clear;

            BackgroundImage.gameObject.SetActive(false);

            isMenuActive = false;
        }

        private void Start()
        {
            menuTileGroups = new Stack<List<MenuTile>>();
            menuTileGroups.Push(ParentMenuTileGroup);
            SetSelectedMenuTile(menuTileGroups.Peek()[0]);
        }

        private void Update()
        {
            // Toggles the active state of the menu if the 'Menu' input is pressed.
            if (Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
            {
                SetMenuActiveState(!isMenuActive);
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
            if (Input.GetAxis("Horizontal") == -1.0f && !wasHorizontalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToLeft)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToLeft, true);
            }
            else if (Input.GetAxis("Horizontal") == 1.0f && !wasHorizontalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToRight)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileToRight, true);
            }
            else if (Input.GetAxis("Vertical") == 1.0f && !wasVerticalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileAbove)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileAbove, true);
            }
            else if (Input.GetAxis("Vertical") == -1.0f && !wasVerticalInputDownDuringPreviousUpdate && menuTileGroups.Peek()[currentlySelectedMenuTileIndex].TileBelow)
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

                if(menuTileGroups.Count > 1)
                {
                    PopMenuTileGroup();
                }
                else
                {
                    SetMenuActiveState(!isMenuActive);
                }                
            }
        }

        // Pops all the menu tile groups other than the one on the bottom of the stack and selects the first tile in that list.
        private void ResetMenuToDefaultState()
        {
            if (menuTileGroups.Count > 1)
            {
                for (int i = 0; i < menuTileGroups.Count - 1; ++i)
                {
                    PopMenuTileGroup();
                }
            }

            if(currentlySelectedMenuTileIndex != 0)
            {
                SetSelectedMenuTile(menuTileGroups.Peek()[0]);
            }
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
                menuTile.gameObject.SetActive(false);
            }
            menuTileGroups.Pop();

            SetSelectedMenuTile(menuTileGroups.Peek()[0]);
        }
    }
}
