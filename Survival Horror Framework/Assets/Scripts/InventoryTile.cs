////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        28.11.20
// Date last edited:    05.02.21
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
        public TextMeshProUGUI ItemCountText; // The UI text used to display the number of inventory items currently being stored within this tile.

        // The property used to get the currently stored inventory item.
        public InventoryItem StoredInventoryItem
        {
            get { return storedInventoryItem; }
        }

        // The property used to get whether no item/s are currently being stored in this inventory tile.
        public bool IsEmpty
        {
            get { return storedInventoryItem == null || storedInventoryItemCount == 0; }
        }

        // The property used to get whether the maximum count of the currently stored inventory item is being stored within this tile.
        public bool IsFull
        {
            get { return storedInventoryItem != null && storedInventoryItemCount == storedInventoryItem.MaxStackCount; }
        }

        // The property used to get the count of the stack of items currently stored in this inventory tile.
        public int StoredInventoryItemCount
        {
            get { return storedInventoryItemCount; }
        }

        // The property used to get the displayed name of the currently stored inventory item - if no item is currently being stored, returns an empty string.
        public string StoredInventoryItemDisplayName
        {
            get
            {
                if (storedInventoryItem)
                {
                    return storedInventoryItem.DisplayName;
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
                bool useItemTilePrepared = false;
                bool checkItemTilePrepared = false;
                bool combineItemTilePrepared = false;
                foreach (MenuTile childTile in ChildMenuTiles)
                {
                    if (childTile.GetComponent<UseItemMenuTile>())
                    {
                        childTile.GetComponent<UseItemMenuTile>().InventoryTileToUse = this;
                        //childTile.GetComponent<UseItemMenuTile>().InventoryItemToUse = storedInventoryItem;
                        useItemTilePrepared = true;
                    }
                    else if (childTile.GetComponent<CheckItemMenuTile>())
                    {
                        childTile.GetComponent<CheckItemMenuTile>().InventoryItemToCheck = storedInventoryItem;
                        checkItemTilePrepared = true;
                    }
                    else if (childTile.GetComponent<CombineItemMenuTile>())
                    {
                        childTile.GetComponent<CombineItemMenuTile>().InitiallySelectedInventoryTile = this;
                        combineItemTilePrepared = true;
                    }
                }

                if (!useItemTilePrepared || !checkItemTilePrepared || !combineItemTilePrepared)
                {
                    throw new System.Exception("For the InventoryTile " + name + " to function correctly, the child tiles must have at least one UseItemTile, one CheckitemMenuTile, and one CombineItemMenuTile component present.");
                }

                base.ActivateTile(gameMenu);
            }
        }

        // If the tile is currently empty or storing the same type of item as the specified item to add and not full, stores the specified inventory item in this inventory tile.
        public void StoreInventoryItem(InventoryItem item)
        {
            if (IsFull)
            {
                throw new System.Exception("The inventory item " + item.DisplayName + " cannot be stored in the inventory tile " + this.name + " because it already contains the maximum number of inventory item " + storedInventoryItem.name + " which can be stored within a single inventory tile.");
            }
            else if (!IsEmpty && item.DisplayName != StoredInventoryItemDisplayName)
            {
                throw new System.Exception("The display name of the specified inventory item must match that of the already stored item within this inventory tile.");
            }

            if (!storedInventoryItem)
            {
                storedInventoryItem = item;
            }
            storedInventoryItemCount++;

            StoredInventoryItemImage.enabled = true;
            StoredInventoryItemImage.sprite = item.InventoryTileSprite;
            UpdateItemCountText();
            PlayMenuActivationSoundOnActivateTile = true;
        }

        // Removes all stored inventory items from this tile.
        public void DestroyStoredInventoryItems()
        {
            if (IsEmpty)
            {
                throw new System.Exception("The DestroyStoredInventoryItems() function of InventoryTile " + name + " cannot be called as it is already empty.");
            }

            storedInventoryItemCount = 0;
            storedInventoryItem = null;
            StoredInventoryItemImage.enabled = false;
            UpdateItemCountText();
        }
        // Removes the specified count of the currently stored inventory items from this tile.
        public void DestroyStoredInventoryItems(uint toRemoveCount)
        {
            if (IsEmpty)
            {
                throw new System.Exception("The DestroyStoredInventoryItems() function of InventoryTile " + name + " cannot be called as it is already empty.");
            }
            if (storedInventoryItemCount - toRemoveCount < 0)
            {
                throw new System.Exception("The toRemoveCount parameter (" + toRemoveCount + ") of DestroyStoredInventoryItems() function of InventoryTile " + name + " must leave at least a remaining count of zero as a minimum after being subtracted from the current storedInventoryItemCount (" + storedInventoryItem + ".");
            }

            storedInventoryItemCount -= (int)toRemoveCount;
            if (IsEmpty)
            {
                storedInventoryItem = null;
                StoredInventoryItemImage.enabled = false;
            }
            UpdateItemCountText();
        }


        protected override void Awake()
        {
            base.Awake();
            PlayMenuActivationSoundOnActivateTile = false;

            StoredInventoryItemImage.enabled = false;
            UpdateItemCountText();
        }


        private InventoryItem storedInventoryItem;
        private int storedInventoryItemCount;

        private void UpdateItemCountText()
        {
            if (IsEmpty || storedInventoryItem.MaxStackCount == 1)
            {
                ItemCountText.enabled = false;
            }
            else
            {
                if (!ItemCountText.enabled)
                {
                    ItemCountText.enabled = true;
                }
                ItemCountText.text = storedInventoryItemCount.ToString();
            }
        }
    }
}
