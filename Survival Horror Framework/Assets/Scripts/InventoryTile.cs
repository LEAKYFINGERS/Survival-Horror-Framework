////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        28.11.20
// Date last edited:    28.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    // The script for a menu tile used to store items in the inventory of the player.
    public class InventoryTile : ParentMenuTile
    {
        public Image StoredInventoryItemImage; // The image used to display the sprite of the currently stored inventory item.

        public bool IsEmpty
        {
            get { return storedInventoryItem == null; }
        }

        public override void ActivateTile(GameMenu gameMenu)
        {
            base.ActivateTile(gameMenu);
        }

        public void StoreInventoryItem(InventoryItem item)
        {
            if(!IsEmpty)
            {
                throw new System.Exception("The inventory item " + item.Name + " cannot be stored in the inventory tile " + this.name + " because it already contains inventory item " + storedInventoryItem.name);
            }

            storedInventoryItem = item;

            StoredInventoryItemImage.enabled = true;
            StoredInventoryItemImage.sprite = item.InventoryTileSprite;
        }


        protected override void Awake()
        {
            base.Awake();

            StoredInventoryItemImage.enabled = false;
        }


        private InventoryItem storedInventoryItem;

        
    }
}
