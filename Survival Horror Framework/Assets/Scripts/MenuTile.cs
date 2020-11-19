////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        17.11.20
// Date last edited:    19.11.20
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SurvivalHorrorFramework
{
    [RequireComponent(typeof(Image))]
    public class MenuTile : MonoBehaviour
    {
        public MenuTile TileToLeft;
        public MenuTile TileToRight;
        public MenuTile TileAbove;
        public MenuTile TileBelow;
        public Sprite DefaultSprite;
        public Sprite SelectedSprite;

        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if(value)
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

        public virtual void ActivateTile(GameMenu gameMenu) { }        


        protected Image imageComponent;
        protected bool isSelected;

        protected void Awake()
        {
            imageComponent = GetComponent<Image>();
            IsSelected = false;
        }
    }
}
