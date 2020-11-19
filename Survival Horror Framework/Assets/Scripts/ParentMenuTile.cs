////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        19.11.20
// Date last edited:    19.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    public class ParentMenuTile : MenuTile
    {
        public List<MenuTile> ChildMenuTiles;

        public override void ActivateTile(GameMenu gameMenu)
        {

        }


        protected void Start()
        {
            foreach(MenuTile childTile in ChildMenuTiles)
            {
                childTile.gameObject.SetActive(false);
            }
        }        
    }
}
