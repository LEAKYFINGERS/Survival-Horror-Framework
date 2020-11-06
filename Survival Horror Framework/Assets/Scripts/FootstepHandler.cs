//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        06.11.20
// Date last edited:    06.11.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    public class FootstepHandler : MonoBehaviour
    {
        public AudioSource LeftFoot;
        public AudioSource RightFoot;
        public AudioClip TestFootstepWalkSound; // DEBUG
        public AudioClip TestFootstepRunSound; // DEBUG

        public void PlayLeftFootstepWalk()
        {
            if (!LeftFoot.isPlaying)
            {
                LeftFoot.clip = TestFootstepWalkSound;
                LeftFoot.Play();
            }
        }

        public void PlayRightFootstepWalk()
        {
            if (!RightFoot.isPlaying)
            {
                RightFoot.clip = TestFootstepWalkSound;
                RightFoot.Play();
            }
        }

        public void PlayLeftFootstepRun()
        {
            if (!LeftFoot.isPlaying)
            {
                LeftFoot.clip = TestFootstepRunSound;
                LeftFoot.Play();
            }
        }

        public void PlayRightFootstepRun()
        {
            if (!RightFoot.isPlaying)
            {
                RightFoot.clip = TestFootstepRunSound;
                RightFoot.Play();
            }
        }


        //private Material GetFloorMaterial()
        //{

        //}
    }
}
