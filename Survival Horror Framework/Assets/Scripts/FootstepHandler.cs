//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        06.11.20
// Date last edited:    09.11.20
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
        public List<FootstepFloorMaterial> FootstepFloorMaterials;
        // TODO - add default footstep walk and run sounds
        // TODO - add support for arrays of walk + run sounds and randomise which is picked

        public void PlayLeftFootstepWalk()
        {
            if (!LeftFoot.isPlaying)
            {                
                LeftFoot.clip = GetFootstepFloorMaterialBelowPlayer().FootstepWalkSound;
                LeftFoot.Play();
            }
        }

        public void PlayRightFootstepWalk()
        {
            if (!RightFoot.isPlaying)
            {
                RightFoot.clip = GetFootstepFloorMaterialBelowPlayer().FootstepWalkSound;
                RightFoot.Play();
            }
        }

        public void PlayLeftFootstepRun()
        {
            if (!LeftFoot.isPlaying)
            {
                LeftFoot.clip = GetFootstepFloorMaterialBelowPlayer().FootstepRunSound;
                LeftFoot.Play();
            }
        }

        public void PlayRightFootstepRun()
        {
            if (!RightFoot.isPlaying)
            {               
                RightFoot.clip = GetFootstepFloorMaterialBelowPlayer().FootstepRunSound; 
                RightFoot.Play();
            }
        }


        private FootstepFloorMaterial GetFootstepFloorMaterialBelowPlayer()
        {
            RaycastHit raycastHit;
            Ray ray = new Ray(transform.position, Vector3.down); // TODO - add vector offset for ray origin from transform position.
            if (Physics.Raycast(ray, out raycastHit))
            {
                Material floorMaterial = raycastHit.collider.GetComponent<Renderer>().material;
                foreach(FootstepFloorMaterial footstepFloorMaterial in FootstepFloorMaterials)
                {
                    if(floorMaterial == footstepFloorMaterial.FloorMaterial)
                    {
                        return footstepFloorMaterial;
                    }
                }

                throw new System.Exception("The FootstepFloorMaterials list of the FootstepHandler script does not contain any FootstepFloorMaterial objects with the same material as assigned to the Renderer of the floor on which the attached character is standing.");
            }
            else
            {
                throw new System.Exception("There is no collider currently below the object which the FootstepHandler is assigned to.");
            }
        }
    }
}
