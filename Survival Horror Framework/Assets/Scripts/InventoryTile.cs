////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        28.11.20
// Date last edited:    12.01.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    // The script for a menu tile used to store items in the inventory of the player.
    public class InventoryTile : ParentMenuTile
    {
        public Image StoredInventoryItemImage; // The image used to display the sprite of the currently stored inventory item.
        //public TextMeshProUGUI ItemCountText; // The UI text used to display the number of inventory items currently being stored within this tile.

        public bool IsEmpty
        {
            get { return storedInventoryItem == null; }
        }

        // The property used to get the name of the currently stored inventory item - if no item is currently being stored, returns an empty string.
        public string StoredInventoryItemName
        {
            get
            {
                if(storedInventoryItem)
                {
                    return storedInventoryItem.Name;
                }
                else
                {
                    return "";
                }
            }
        }

        // Pushes the item interaction menu tile group onto the stack if this inventory tile currently contains an inventory item.
        public override void ActivateTile(GameMenu gameMenu)
        {
            if (!IsEmpty)
            {
                foreach(MenuTile childTile in ChildMenuTiles)
                {
                    if(childTile.GetComponent<CheckitemMenuTile>())
                    {
                        childTile.GetComponent<CheckitemMenuTile>().InventoryItemToCheck = storedInventoryItem;
                        base.ActivateTile(gameMenu);
                        return;
                    }                    
                }

                throw new System.Exception("For the InventoryTile " + name + " to function correctly, one of the assigned child tiles must have a CheckitemMenuTile component attached.");
            }
        }

        // If empty, stores the specified inventory item in this inventory tile.
        public void StoreInventoryItem(InventoryItem item)
        {
            if(!IsEmpty)
            {
                throw new System.Exception("The inventory item " + item.Name + " cannot be stored in the inventory tile " + this.name + " because it already contains inventory item " + storedInventoryItem.name);
            }

            storedInventoryItem = item;

            StoredInventoryItemImage.enabled = true;
            StoredInventoryItemImage.sprite = item.InventoryTileSprite;
            PlayMenuActivationSoundOnActivateTile = true;
        }


        protected override void Awake()
        {
            base.Awake();
            PlayMenuActivationSoundOnActivateTile = false;

            StoredInventoryItemImage.enabled = false;
        }


        private InventoryItem storedInventoryItem;        
    }
}
