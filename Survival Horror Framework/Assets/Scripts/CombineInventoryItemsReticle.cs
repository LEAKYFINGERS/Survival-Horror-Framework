////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.02.21
// Date last edited:    01.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    // The script for a menu tile used to display one of the two reticles shown when the player is attempting to combine items.
    public class CombineInventoryItemsReticle : MenuTile
    {
        public InventoryTile AttachedInventoryTile; // The inventory tile which this reticle hovers over and reads to see if it contains an inventory item to be combined.        
        [HideInInspector]
        public InventoryTile InitiallySelectedInventoryTile; // The initially-selected inventory tile which contains the item to be combined with the item stored in the attached inventory tile.

        public override void ActivateTile(GameMenu gameMenu)
        {
            gameMenu.SetParentMenuTileGroup(gameMenu.DefaultParentMenuTileGroup, InitiallySelectedInventoryTile);
            
            InitiallySelectedInventoryTile = null;
        }
    }
}