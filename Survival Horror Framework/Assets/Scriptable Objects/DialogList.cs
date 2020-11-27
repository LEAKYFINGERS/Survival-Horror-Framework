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
    // A scriptable object used to store a list of dialog snippets that can be displayed using the DialogDisplay script.
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Dialog List")]
    public class DialogList : ScriptableObject
    {
        public List<Dialog> Dialogs;
    }
}
