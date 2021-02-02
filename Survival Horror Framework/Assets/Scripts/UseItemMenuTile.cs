////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.02.21
// Date last edited:    02.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    // The menu tile which allows the player to use/equip the item in the currently selected inventory tile.
    public class UseItemMenuTile : MenuTile
    {
        [HideInInspector]
        //public InventoryItem InventoryItemToUse;
        public InventoryTile InventoryTileToUse;
        public Text VerbUIText; // The UI text element displayed on the tile which changes according to the UseVerb of the currently selected inventory item. 

        public override void ActivateTile(GameMenu gameMenu)
        {
            if(InventoryTileToUse == null)
            {
                throw new System.Exception("In order for the UseItemTile " + name + " to be activated, the value for InventoryItemToUse cannot be null.");
            }

            if (!InventoryTileToUse.IsEmpty)
            {
                InventoryTileToUse.DestroyStoredInventoryItems(1);
                gameMenu.SetParentMenuTileGroup(gameMenu.DefaultParentMenuTileGroup);
            }

            InventoryTileToUse = null;
        }


        protected override void OnEnabled()
        {
            if (InventoryTileToUse == null)
            {
                throw new System.Exception("In order for the UseItemTile " + name + " to be enabled, the value for InventoryItemToUse cannot be null.");
            }

            VerbUIText.text = InventoryTileToUse.StoredInventoryItem.UseVerb;
        }
    }
}
