////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        16.11.20
// Date last edited:    17.11.20
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
        public PauseHandler ScenePauseHandler;
        public float ForwardOffsetFromCamera = 0.31f; // The offset value used to position the menu canvas in front of the active camera.
        public AudioClip DeactivationSound;
        public Image ActivationFadeImage; // The UI image (usually solid black) which fades in and out each time the menu transitions between activated and deactivated.
        public float ActivationFadeDuration = 0.25f;
        public Image BackgroundImage;


        private AudioSource audioSourceComponent;
        private bool isMenuActive;
        private bool wasMenuInputDownDuringPreviousUpdate;
        private Color activationFadeImageColor; // The initial color tint of the activation fade image - stored so the image can transition between this color and completely transparent.
        private bool isActivationFadeCoroutineRunning;

        private void Awake()
        {
            audioSourceComponent = GetComponent<AudioSource>();

            activationFadeImageColor = ActivationFadeImage.color;
            ActivationFadeImage.color = Color.clear;

            BackgroundImage.gameObject.SetActive(false);

            isMenuActive = false;
        }

        private void Update()
        {
            // Toggles the active state of the menu if the 'Menu' input is pressed.
            if (Input.GetAxis("Menu") == 1.0f && !wasMenuInputDownDuringPreviousUpdate)
            {
                SetMenuActiveState(!isMenuActive);
            }
        }

        private void LateUpdate()
        {
            wasMenuInputDownDuringPreviousUpdate = Input.GetAxis("Menu") == 1.0f;
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
                audioSourceComponent.PlayOneShot(DeactivationSound);
            }

            // Fades the activation fade image from clear to opaque.
            float timer = 0.0f;
            while(timer < ActivationFadeDuration / 2.0f)
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
    }
}
