////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        23.11.20
// Date last edited:    23.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(Collider))]
    // The base script for an object which the player can interact with via the 'Use' input.
    public abstract class InteractiveObject : MonoBehaviour
    {
        public abstract void Interact();
    }
}
