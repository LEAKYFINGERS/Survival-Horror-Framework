////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        05.12.20
// Date last edited:    05.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    public class SceneTransitionObject : InteractiveObject
    {
        public ColorTintPostProcessHandler FadeHandler;
        public PauseHandler ScenePauseHandler;
        public float FadeDuration;
        
        public override void Interact()
        {
            ScenePauseHandler.PauseScene();
            FadeHandler.FadeToColor(Color.black, FadeDuration);
        }
    }
}
