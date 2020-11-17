////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    18.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(AudioSource))]
    [RequireComponent(typeof(Canvas))]
    public class GameMenu : MonoBehaviour
    {
        public AudioClip ChangeSelectedSound;
        public AudioClip DeactivationSound;
        public Image ActivationFadeImage; // The UI image (usually solid black) which fades in and out each time the menu transitions between activated and deactivated.
        public Image BackgroundImage;
        public List<MenuTile> MenuTiles; // TEST
        public PauseHandler ScenePauseHandler;
        public float ForwardOffsetFromCamera = 0.31f; // The offset value used to position the menu canvas in front of the active camera.   
        public float ActivationFadeDuration = 0.25f;


        private AudioSource audioSourceComponent;
        private Color activationFadeImageColor; // The initial color tint of the activation fade image - stored so the image can transition between this color and completely transparent.
        private bool isMenuActive;
        private bool wasMenuInputDownDuringPreviousUpdate;
        private bool wasHorizontalInputDownDuringPreviousUpdate;
        private bool wasVerticalInputDownDuringPreviousUpdate;
        private bool isActivationFadeCoroutineRunning;
        private int currentlySelectedMenuTileIndex;

        // A coroutine which fades the activation fade image from clear to opaque and back again while the menu itself is activated or deactivated.
        private IEnumerator MenuActivationCoroutine(bool isBeingActivated)
        {
            isActivationFadeCoroutineRunning = true;

            if (isBeingActivated)
            {
                ScenePauseHandler.PauseScene();
                SetSelectedMenuTile(MenuTiles[0]); // Resets the currently selected menu tile to the first in the list. 
            }
            else
            {
                audioSourceComponent.PlayOneShot(DeactivationSound);
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
            SetSelectedMenuTile(MenuTiles[0]);
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
        }

        private void UpdateMenuTiles()
        {
            // Updates which is the currently selected menu tile according to the player selection inputs.
            if (Input.GetAxis("Horizontal") == -1.0f && !wasHorizontalInputDownDuringPreviousUpdate && MenuTiles[currentlySelectedMenuTileIndex].TileToLeft)
            {
                SetSelectedMenuTile(MenuTiles[currentlySelectedMenuTileIndex].TileToLeft, true);
            }
            else if (Input.GetAxis("Horizontal") == 1.0f && !wasHorizontalInputDownDuringPreviousUpdate && MenuTiles[currentlySelectedMenuTileIndex].TileToRight)
            {
                SetSelectedMenuTile(MenuTiles[currentlySelectedMenuTileIndex].TileToRight, true);
            }
            else if (Input.GetAxis("Vertical") == 1.0f && !wasVerticalInputDownDuringPreviousUpdate && MenuTiles[currentlySelectedMenuTileIndex].TileAbove)
            {
                SetSelectedMenuTile(MenuTiles[currentlySelectedMenuTileIndex].TileAbove, true);
            }
            else if (Input.GetAxis("Vertical") == -1.0f && !wasVerticalInputDownDuringPreviousUpdate && MenuTiles[currentlySelectedMenuTileIndex].TileBelow)
            {
                SetSelectedMenuTile(MenuTiles[currentlySelectedMenuTileIndex].TileBelow, true);
            }
        }

        // Activates or deactivates the menu if the scene isn't already currently paused (e.g. for camera transition stutter effect).
        private void SetMenuActiveState(bool activeState)
        {
            if (!ScenePauseHandler.IsScenePaused || ScenePauseHandler.IsScenePaused && isMenuActive && !activeState)
            {
                if (!isActivationFadeCoroutineRunning)
                {
                    StartCoroutine("MenuActivationCoroutine", activeState);
                }
            }
        }

        // If the specified menu tile belongs to the MenuTiles list updates the currently selected menu tile index so that the specified tile is the only one with the 'Selected' status.
        private void SetSelectedMenuTile(MenuTile menuTile, bool playSoundEffect = false)
        {
            for (int i = 0; i < MenuTiles.Count; ++i)
            {
                if (MenuTiles[i] == menuTile)
                {
                    MenuTiles[currentlySelectedMenuTileIndex].IsSelected = false;

                    MenuTiles[i].IsSelected = true;
                    currentlySelectedMenuTileIndex = i;

                    if (playSoundEffect)
                    {
                        audioSourceComponent.PlayOneShot(ChangeSelectedSound);
                    }

                    break;
                }
            }
        }
    }
}
