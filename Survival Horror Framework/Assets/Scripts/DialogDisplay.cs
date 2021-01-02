////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        24.11.20
// Date last edited:    02.01.21
// Reference/s:         https://repo.ijs.si/eHeritage/3DInstitute/blob/master/Assets/TextMesh%20Pro/Examples/Scripts/VertexColorCycler.cs
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(Canvas))]
    // The script used to handle the canvas and UI text object which display dialog text to the player.
    public class DialogDisplay : MonoBehaviour
    {
        public delegate void DialogDisplayEventHandler();

        public event DialogDisplayEventHandler OnAllDialogSnippetsDisplayCompleted; // Called when all of the dialog snippets have finished being displayed and the dialog display is awaiting player input.
        public PauseHandler ScenePauseHandler;
        public TextMeshProUGUI UIText;

        public bool IsDialogBeingDisplayed
        {
            get { return isDisplayDialogCoroutineRunning; }
        }

        public void PauseSceneAndDisplayDialog(Dialog dialog)
        {
            if (!isDisplayDialogCoroutineRunning)
            {
                StartCoroutine("PauseSceneAndDisplayDialogCoroutine", dialog);
            }
        }


        private bool isDisplayDialogCoroutineRunning;

        private IEnumerator PauseSceneAndDisplayDialogCoroutine(Dialog dialog)
        {
            isDisplayDialogCoroutineRunning = true;
            ScenePauseHandler.PauseScene();
            UIText.enabled = true;

            // Divides the dialog into individual 'snippets' seperated by a tilde character which are each displayed one at a time in sequence.
            string[] dialogSnippets = dialog.DisplayedText.Split('~'); 
            for(int i = 0; i < dialogSnippets.Length; ++i)
            {
                UIText.text = dialogSnippets[i].Trim(); // Trims the text to remove any whitespace at the beginning or end.
                UIText.ForceMeshUpdate(); // Updates the mesh of the UIText so that all of the textInfo values will be accurate.
                TMP_TextInfo textInfo = UIText.textInfo;

                // If the character reveal speed is greater than zero causes the characters to appear one at a time, else causes them all to appear instantly.
                if (dialog.DefaultCharacterRevealInterval > 0.0f)
                {
                    SetAllUITextCharactersTransparency(textInfo, 0); // Sets all the characters to initially be invisible.

                    // Causes the characters to 'appear' one at a time by setting the alpha value of each subsequent character to 'opaque' each time the loop is iterated.
                    int appearingCharacterIndex = 0;
                    while (appearingCharacterIndex < textInfo.characterCount)
                    {
                        Color32 opaqueColor = new Color32(textInfo.characterInfo[appearingCharacterIndex].color.r, textInfo.characterInfo[appearingCharacterIndex].color.g, textInfo.characterInfo[appearingCharacterIndex].color.b, 255);
                        SetUITextCharacterColor(textInfo, opaqueColor, appearingCharacterIndex);
                        appearingCharacterIndex++;

                        yield return new WaitForSecondsRealtime(Input.GetAxis("Use") == 1.0f || Input.GetAxis("Run") == 1.0f ? dialog.FastCharacterRevealInterval : dialog.DefaultCharacterRevealInterval); // Adjusts the speed of the character reveal according to whether any inputs are being held
                    }
                    SetAllUITextCharactersTransparency(textInfo, 255);
                }

                if(i == dialogSnippets.Length - 1 && OnAllDialogSnippetsDisplayCompleted != null)
                {
                    OnAllDialogSnippetsDisplayCompleted.Invoke();
                }

                // Pauses to wait for the player to press either the 'Use' or 'Run' inputs before the dialog text disappears and the scene unpauses. 
                bool continueInputReleased = false;
                while (true)
                {
                    if ((Input.GetAxis("Use") == 1.0f || Input.GetAxis("Run") == 1.0f) && continueInputReleased)
                    {
                        break;
                    }
                    else
                    {
                        if (Input.GetAxis("Use") == 0.0f && Input.GetAxis("Run") == 0.0f)
                        {
                            continueInputReleased = true;
                        }

                        yield return null;
                    }
                }
            }

            UIText.enabled = false;
            ScenePauseHandler.UnpauseScene();
            isDisplayDialogCoroutineRunning = false;
        }

        private void Awake()
        {
            UIText.enabled = false;
        }

        // Sets the alpha of all the characters currently displayed by the UIText to the specified value while preserving their RGB values.
        private void SetAllUITextCharactersTransparency(TMP_TextInfo textInfo, byte transparencyValue)
        {
            for (int i = 0; i < textInfo.characterCount; ++i)
            {
                Color32 transparentColor = new Color32(textInfo.characterInfo[i].color.r, textInfo.characterInfo[i].color.g, textInfo.characterInfo[i].color.b, transparencyValue);
                SetUITextCharacterColor(textInfo, transparentColor, i);
            }
        }

        // Sets the color of the specified character currently displayed by the UIText.
        private void SetUITextCharacterColor(TMP_TextInfo textInfo, Color32 newColor, int characterIndex)
        {
            if (characterIndex < 0 || characterIndex > textInfo.characterCount)
            {
                throw new System.Exception("The characterIndex parameter is outside the range of any existing characters within the UIText.");
            }

            int materialIndex = textInfo.characterInfo[characterIndex].materialReferenceIndex; // The material of the index used by the current character.            
            Color32[] newVertexColors = textInfo.meshInfo[materialIndex].colors32; // The 4 vertex colors of the mesh used by this text element (character or sprite).            
            int vertexIndex = textInfo.characterInfo[characterIndex].vertexIndex; // The index of the first vertex used by this text element.

            if (textInfo.characterInfo[characterIndex].isVisible)
            {
                newVertexColors[vertexIndex + 0] = newColor;
                newVertexColors[vertexIndex + 1] = newColor;
                newVertexColors[vertexIndex + 2] = newColor;
                newVertexColors[vertexIndex + 3] = newColor;

                UIText.UpdateVertexData(TMP_VertexDataUpdateFlags.Colors32); // Pushes all updated vertex data to the appropriate meshes when using either the Mesh Renderer or CanvasRenderer.
            }
        }
    }
}
