////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        22.11.20
// Date last edited:    22.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A script derived from MenuTile which deactivates the menu when activated.
    public class ExitMenuTile : MenuTile
    {
        public override void ActivateTile(GameMenu gameMenu)
        {
            gameMenu.SetMenuActiveState(false);
        }
    }
}
