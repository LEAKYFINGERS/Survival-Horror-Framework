////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    19.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SurvivalHorrorFramework
{
    public class SceneTransitionObject : InteractiveObject
    {
        public ColorTintPostProcessHandler FadeHandler;
        public PauseHandler ScenePauseHandler;        
        public float FadeDuration;
        public int DestinationSceneIndex;

        public override void Interact()
        {
            ScenePauseHandler.PauseScene();

            SceneManager.LoadScene(DestinationSceneIndex);

            //FadeHandler.FadeToColor(Color.black, FadeDuration);
        }
    }
}
