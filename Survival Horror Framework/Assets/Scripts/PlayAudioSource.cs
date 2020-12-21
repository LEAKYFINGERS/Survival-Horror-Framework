////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        21.12.20
// Date last edited:    21.12.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A basic script which allows animations to play the AudioClip currently assigned to an attached AudioSource.
    [RequireComponent(typeof(AudioSource))]
    public class PlayAudioSource : MonoBehaviour
    {
        public void PlayAudioClip()
        {
            GetComponent<AudioSource>().Play();
        }
    }
}
