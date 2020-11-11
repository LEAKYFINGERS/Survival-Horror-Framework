//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Reference/s:         https://github.com/boaheck/TheFirstPerson/tree/master/Assets/TheFirstPerson/Code/ExampleExtensions/FootstepSystem
// Date created:        09.11.20
// Date last edited:    11.11.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Footstep Floor Material")]
    public class FootstepFloorTextures : ScriptableObject
    {
        public AudioClip FootstepWalkSound;
        public AudioClip FootstepRunSound;
        public Texture2D[] FloorTextures;
    }
}
