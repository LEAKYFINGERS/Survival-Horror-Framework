////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        17.11.20
// Date last edited:    22.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(Image))]
    // The script for an individual menu tile which can be selected and activated by the player in the game menu as well as inherited from to extend functionality.
    public class MenuTile : MonoBehaviour
    {
        public MenuTile TileToLeft; // The tile which will become selected after the player presses 'Left' while this tile is selected.
        public MenuTile TileToRight; // The tile which will become selected after the player presses 'Right' while this tile is selected.
        public MenuTile TileAbove; // The tile which will become selected after the player presses 'Up' while this tile is selected.
        public MenuTile TileBelow; // The tile which will become selected after the player presses 'Down' while this tile is selected.
        public Sprite DefaultSprite;
        public Sprite SelectedSprite;
        public bool PlayMenuActivationSoundOnActivateTile = false;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (value)
                {
                    imageComponent.sprite = SelectedSprite;
                }
                else
                {
                    imageComponent.sprite = DefaultSprite;
                }

                isSelected = value;
            }
        }

        // Called when the player presses the 'Use' input while this tile is selected in the menu.
        public virtual void ActivateTile(GameMenu gameMenu) { }


        protected Image imageComponent;
        protected bool isSelected;

        protected void Awake()
        {
            imageComponent = GetComponent<Image>();
            IsSelected = false;

            gameObject.SetActive(false);
        }
    }
}
