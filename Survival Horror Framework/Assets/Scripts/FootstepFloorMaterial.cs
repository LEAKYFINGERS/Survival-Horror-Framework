//////////////////////////////////////////////////
// Author/s:            LEAKYFINGERS   
// Date created:        09.11.20
// Date last edited:    09.11.20
//////////////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Footstep Floor Material")]
public class FootstepFloorMaterial : ScriptableObject
{
    public AudioClip FootstepWalkSound;
    public AudioClip FootstepRunSound;
    public Material FloorMaterial;
}
