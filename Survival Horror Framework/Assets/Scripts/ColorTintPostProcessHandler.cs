////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    05.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace SurvivalHorrorFramework
{
    // The script used to control the color tint post-processing effect for uses such as fades.
    [RequireComponent(typeof(PostProcessVolume))]
    public class ColorTintPostProcessHandler : MonoBehaviour
    {
        public Color SceneLoadFadeInColor = Color.black; // The initial color which will fade into clear when the scene is loaded.
        public float SceneLoadFadeInDuration = 1.0f; // The duration of the fade into clear when the scene is loaded.

        // Fades the post-process color tint from the current color to the specified goal color.
        public void FadeToColor(Color newColor, float duration)
        {
            coroutineInitialColor = colorTintPostProcessEffect.Color.value;
            coroutineGoalColor = newColor;
            coroutineDuration = duration;
            StartCoroutine("FadeColorCoroutine");
        }
        // Fades the post-process color tint between the two specified colors.
        public void FadeToColor(Color initialColor, Color goalColor, float duration)
        {
            coroutineInitialColor = initialColor;
            coroutineGoalColor = goalColor;
            coroutineDuration = duration;
            StartCoroutine("FadeColorCoroutine");
        }

        private IEnumerator FadeColorCoroutine()
        {
            if (coroutineDuration > 0.0f)
            {
                SetColor(coroutineInitialColor);

                float timer = 0.0f;
                while (timer < coroutineDuration)
                {
                    SetColor(Color.Lerp(coroutineInitialColor, coroutineGoalColor, timer / coroutineDuration));

                    timer += Time.deltaTime;
                    yield return null;
                }
            }

            SetColor(coroutineGoalColor);
        }

        private void Awake()
        {
            PostProcessVolume postProcessVolumeComponent = GetComponent<PostProcessVolume>();
            postProcessVolumeComponent.profile.TryGetSettings(out colorTintPostProcessEffect);
        }

        private void Start()
        {
            FadeToColor(SceneLoadFadeInColor, Color.clear, SceneLoadFadeInDuration);
        }

        private void SetColor(Color newColor)
        {
            colorTintPostProcessEffect.Color.value = newColor;
            colorTintPostProcessEffect.Blend.value = newColor.a; // Sets the alpha value as the blend value because the 'Color' parameter of the shader doesn't take opacity into account.
        }


        private ColorTintPostProcessEffect colorTintPostProcessEffect;
        private Color coroutineInitialColor;
        private Color coroutineGoalColor;
        private float coroutineDuration;
    }
}
