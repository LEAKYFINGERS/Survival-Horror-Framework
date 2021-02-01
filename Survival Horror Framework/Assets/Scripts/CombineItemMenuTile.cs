////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        01.02.21
// Date last edited:    01.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    // The menu tile which allows the player combine two inventory items currently within the inventory.
    public class CombineItemMenuTile : MenuTile
    {
        [HideInInspector]
        public InventoryTile InitiallySelectedInventoryTile; // The inventory tile which was selected when the combine item tile was activated - contains the first inventory item of the two to be combined.
        public List<CombineInventoryItemsReticle> ChildCombineReticleMenuTiles; // The group of menu tiles which are pushed onto the menu stack and enabled when this tile is activated, used to choose the second of the two inventory items to be combined (excludes the currently selected inventory tile on activation).

        public override void ActivateTile(GameMenu gameMenu)
        {
            if (InitiallySelectedInventoryTile == null)
            {
                throw new System.Exception("In order for the CombineItemMenuTile " + name + " to be activated, the value for InitiallySelectedInventoryTile cannot be null.");
            }

            // Creates and activates a list of menu tile reticles to be pushed onto the game menu stack which doesn't include the reticle over the initially-selected inventory tile.
            List<MenuTile> reticleTiles = new List<MenuTile>();
            foreach (CombineInventoryItemsReticle reticle in ChildCombineReticleMenuTiles)
            {
                // Creates a 'non-functional' reticle to display over the initially-selected inventory tile.
                if (reticle.AttachedInventoryTile == InitiallySelectedInventoryTile)
                {
                    intiallySelectedItemDisplayReticle = Instantiate(reticle, reticle.transform.parent).transform;
                    intiallySelectedItemDisplayReticle.GetComponent<Image>().enabled = true;
                    intiallySelectedItemDisplayReticle.GetComponent<CombineInventoryItemsReticle>().IsEnabled = true;
                    intiallySelectedItemDisplayReticle.GetComponent<CombineInventoryItemsReticle>().IsSelected = true;
                    intiallySelectedItemDisplayReticle.GetComponent<CombineInventoryItemsReticle>().enabled = false;
                }
                else
                {
                    reticle.gameObject.SetActive(true);
                    reticle.InitiallySelectedInventoryTile = InitiallySelectedInventoryTile;
                    reticleTiles.Add((MenuTile)reticle);
                }
            }
            gameMenu.PushMenuTileGroup(reticleTiles);

            if (!destroyInitiallySelectedItemDisplayReticleAddedToEvent)
            {
                gameMenu.OnPopMenuTileGroup += DestroyInitiallySelectedItemDisplayReticle;
                destroyInitiallySelectedItemDisplayReticleAddedToEvent = true;
            }
        }


        private Transform intiallySelectedItemDisplayReticle; // The non-functional reticle used to display the initially-selected menu item to be combined with the second item chosen by the player.
        private bool destroyInitiallySelectedItemDisplayReticleAddedToEvent;

        // The function to be called each time a menu group is popped to ensure the initially-selected inventory item reticle is destroyed.
        private void DestroyInitiallySelectedItemDisplayReticle()
        {
            if (intiallySelectedItemDisplayReticle != null)
            {
                Destroy(intiallySelectedItemDisplayReticle.gameObject);
            }
        }
    }
}
