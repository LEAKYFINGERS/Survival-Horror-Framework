////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        19.11.20
// Date last edited:    22.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // A script derived from MenuTile which pushes the specified 'child' menu tile group onto the menu groups stack of the game menu when activated.
    public class ParentMenuTile : MenuTile
    {
        public List<MenuTile> ChildMenuTiles; // The group of menu tiles which are pushed onto the stack and enabled when this tile is activated.

        public override void ActivateTile(GameMenu gameMenu)
        {
            foreach (MenuTile childTile in ChildMenuTiles)
            {
                childTile.gameObject.SetActive(true);
            }

            gameMenu.PushMenuTileGroup(ChildMenuTiles);
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
