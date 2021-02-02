////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        17.11.20
// Date last edited:    02.02.21
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
        public MenuTile TileToLeft;
        public MenuTile TileToRight;
        public MenuTile TileAbove;
        public MenuTile TileBelow;
        public Sprite DefaultSprite;
        public Sprite SelectedSprite;
        public bool isVisibleWhenNotEnabled = false;
        public bool PlayMenuActivationSoundOnActivateTile = false;

        // The property used to get and set whether the tile is 'enabled', which controls whether or not it can be selected.
        public bool IsEnabled
        {
            get { return isEnabled; }
            set
            {
                if (value)
                {
                    imageComponent.enabled = true;
                    for (int i = 0; i < transform.childCount; ++i)
                    {
                        transform.GetChild(i).gameObject.SetActive(true);
                    }

                    OnEnabled();
                }
                else if (!isVisibleWhenNotEnabled)
                {
                    imageComponent.enabled = false;
                    for (int i = 0; i < transform.childCount; ++i)
                    {
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }

                IsSelected = false;
                isEnabled = value;
            }
        }

        // The property used to get and set whether the tile is 'selected', which controls whether or not it can be activated and can only occur while it is also enabled.
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isEnabled)
                {
                    if (value && isEnabled)
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
        }

        // Called when the player presses the 'Use' input while this tile is selected in the menu.
        public virtual void ActivateTile(GameMenu gameMenu) { }


        protected Image imageComponent;
        protected bool isEnabled;
        protected bool isSelected;

        protected virtual void Awake()
        {
            imageComponent = GetComponent<Image>();

            IsSelected = false;
            IsEnabled = false;
        }

        // A base function which is called when the MenuTile has been enabled.
        protected virtual void OnEnabled() { } 
    }
}
