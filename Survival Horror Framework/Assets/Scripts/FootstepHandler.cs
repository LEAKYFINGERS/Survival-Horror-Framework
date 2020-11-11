//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS
// Reference/s:         https://github.com/boaheck/TheFirstPerson/tree/master/Assets/TheFirstPerson/Code/ExampleExtensions/FootstepSystem
// Date created:        06.11.20
// Date last edited:    11.11.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// TODO - add support for arrays of walk + run sounds which are randomly picked for each subsequent footfall.
// TODO - are two seperate foot audio sources required - maybe just use one instead?
// TODO - add vector offset for footstep test ray origin from transform position?

namespace SurvivalHorrorFramework
{
    public class FootstepHandler : MonoBehaviour
    {
        public AudioClip DefaultFootstepWalkSound; // The footstep sound effect played when the player/NPC is walking and isn't standing on one of the specified FootstepFloorTextures.
        public AudioClip DefaultFootstepRunSound; // The footstep sound effect played when the player/NPC is running and isn't standing on one of the specified FootstepFloorTextures.
        public AudioSource LeftFoot;
        public AudioSource RightFoot;
        public FootstepFloorTextures[] FootstepFloorTextures; // An array of FootstepFloorTextures used to specify which footstep sound effect should be played depending on what texture the player/NPC is currently standing on.

        // Plays the appropriate walking footstep sound from the left foot AudioSource depending on the texture on which the player/NPC is currently standing.
        public void PlayLeftFootstepWalk()
        {
            if (!LeftFoot.isPlaying)
            {
                LeftFoot.Stop();               
            }
            LeftFoot.clip = GetFootstepSound(false);
            LeftFoot.Play();
        }

        // Plays the appropriate walking footstep sound from the right foot AudioSource depending on the texture on which the player/NPC is currently standing.
        public void PlayRightFootstepWalk()
        {
            if (!RightFoot.isPlaying)
            {
                RightFoot.Stop();
            }
            RightFoot.clip = GetFootstepSound(false);
            RightFoot.Play();
        }

        // Plays the appropriate running footstep sound from the left foot AudioSource depending on the texture on which the player/NPC is currently standing.
        public void PlayLeftFootstepRun()
        {
            if (!LeftFoot.isPlaying)
            {
                LeftFoot.Stop();
            }
            LeftFoot.clip = GetFootstepSound(true);
            LeftFoot.Play();
        }

        // Plays the appropriate running footstep sound from the right foot AudioSource depending on the texture on which the player/NPC is currently standing.
        public void PlayRightFootstepRun()
        {
            if (!RightFoot.isPlaying)
            {
                RightFoot.Stop();
            }
            RightFoot.clip = GetFootstepSound(true);
            RightFoot.Play();
        }


        // Gets the appropriate footstep sound according to the texture on which the player/NPC is currently standing as well as whether it is running or walking.
        private AudioClip GetFootstepSound(bool isRunning)
        {
            // Casts a ray from the player/NPC position directly down to the floor collider to see if the floor texture matches that of any of the specified FootstepFloorTextures - if so, returns the associated walk/run footstep sound.
            RaycastHit raycastHit;
            Ray ray = new Ray(transform.position, Vector3.down); 
            if (Physics.Raycast(ray, out raycastHit))
            {
                MeshRenderer meshRenderer = raycastHit.collider.GetComponent<MeshRenderer>();
                if(meshRenderer)
                {
                    foreach (FootstepFloorTextures footstepFloorMaterial in FootstepFloorTextures)
                    {
                        foreach (Texture2D footstepFloorMaterialTexture in footstepFloorMaterial.FloorTextures)
                        {
                            if (meshRenderer.material.mainTexture == footstepFloorMaterialTexture)
                            {
                                return isRunning ? footstepFloorMaterial.FootstepRunSound : footstepFloorMaterial.FootstepWalkSound; 
                            }
                        }
                    }
                }                              
            }

            return isRunning ? DefaultFootstepRunSound : DefaultFootstepWalkSound;
        }
    }
}
