////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        24.11.20
// Date last edited:    24.11.20
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
        public PauseHandler ScenePauseHandler;
        public TextMeshProUGUI UIText; 

        public bool IsDialogBeingDisplayed
        {
            get { return isDisplayDialogCoroutineRunning; }
        }

        public void PauseSceneAndDisplayDialog()
        {            
            if(!isDisplayDialogCoroutineRunning)
            {
                StartCoroutine("PauseSceneAndDisplayDialogCoroutine");
            }
        }


        //private bool wasUseInputDownDuringPreviousUpdate;
        //private bool wasRunInputDownDuringPreviousUpdate;
        private bool isDisplayDialogCoroutineRunning;

        private IEnumerator PauseSceneAndDisplayDialogCoroutine()
        {
            isDisplayDialogCoroutineRunning = true;
            ScenePauseHandler.PauseScene();
            UIText.enabled = true;

            bool useInputInputReleased = false;

            TMP_CharacterInfo[] characters = UIText.GetTextInfo(UIText.text).characterInfo;
            for (int i = 0; i < UIText.GetTextInfo(UIText.text).characterCount; ++i)
            {
                characters[i].isVisible = false;
            }
            UIText.ForceMeshUpdate();

            //TMP_CharacterInfo[] characters = UIText.GetTextInfo(UIText.text).characterInfo;
            //for (int i = 0; i < UIText.GetTextInfo(UIText.text).characterCount; ++i)
            //{
            //    int meshIndex = UIText.textInfo.characterInfo[i].materialReferenceIndex;
            //    int vertexIndex = UIText.textInfo.characterInfo[i].vertexIndex;
            //    Color32[] vertexColors = UIText.textInfo.meshInfo[meshIndex].colors32;
            //    vertexColors[vertexIndex + 0] = Color.clear;
            //    vertexColors[vertexIndex + 1] = Color.clear; 
            //    vertexColors[vertexIndex + 2] = Color.clear; 
            //    vertexColors[vertexIndex + 3] = Color.clear; 
            //}
            //UIText.ForceMeshUpdate();


            //for (int i = 0; i < myText.textInfo.wordCount; i++)
            //{
            //    TMP_WordInfo wInfo = myText.textInfo.wordInfo;
            //    for (int j = 0; j < wInfo.characterCount; j++)
            //    {
            //        yield return StartCoroutine(waitWithDuration(.2f));
            //        int characterIndex = wInfo.firstCharacterIndex + j;
            //        int meshIndex = myText.textInfo.characterInfo[characterIndex].materialReferenceIndex;
            //        int vertexIndex = myText.textInfo.characterInfo[characterIndex].vertexIndex;
            //        Color32[] vertexColors = myText.textInfo.meshInfo[meshIndex].colors32;
            //        vertexColors[vertexIndex + 0] = BLUE;
            //        vertexColors[vertexIndex + 1] = BLUE;
            //        vertexColors[vertexIndex + 2] = BLUE;
            //        vertexColors[vertexIndex + 3] = BLUE;

            //    }
            //    myText.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
            //}




            while (true)
            {
                if((Input.GetAxis("Use") == 1.0f && useInputInputReleased ))// || (Input.GetAxis("Run") == 1.0f && !wasRunInputDownDuringPreviousUpdate))
                {
                    break;
                }
                else
                {
                    if(Input.GetAxis("Use") == 0.0f)
                    {
                        useInputInputReleased = true;
                    }

                    Debug.Log(UIText.GetTextInfo(UIText.text).characterInfo[0].color);

                    yield return null;
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

        //private void LateUpdate()
        //{
        //    wasUseInputDownDuringPreviousUpdate = Input.GetAxis("Use") == 1.0f;
        //    //wasRunInputDownDuringPreviousUpdate = Input.GetAxis("Run") == 1.0f;

        //    if (isDisplayDialogCoroutineRunning && Input.GetAxis("Use") == 0.0f)
        //    {
        //        hasUseInputBeenReleasedSinceDialogDisplayCoroutineStarted = true;
        //    }
        //    else
        //    {
        //        hasUseInputBeenReleasedSinceDialogDisplayCoroutineStarted = false;
        //    }
        //}
    }
}
