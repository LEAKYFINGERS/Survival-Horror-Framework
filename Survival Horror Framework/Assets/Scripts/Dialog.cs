////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        27.11.20
// Date last edited:    27.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A scriptable object used to store a dialog snippet that can be displayed using the DialogDisplay script.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Dialog")]
    public class Dialog : ScriptableObject
    {
        public float DefaultCharacterRevealInterval = 0.1f; // The duration of the pause between each character of the displayed dialog text being revealed if the 'Use' or 'Run' inputs aren't being held.
        public float FastCharacterRevealInterval = 0.05f; // The duration of the pause between each character of the displayed dialog text being revealed if the 'Use' or 'Run' inputs are being held.
        [TextArea(15, 20)]
        public string DisplayedText = "DIALOG TEXT~dialog text~DIALOG TEXT";
    }
}
