////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        17.11.20
// Date last edited:    17.11.20
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
        public Sprite DefaultSprite;
        public Sprite HoverSprite;
        public MenuTile TileLeft;
        public MenuTile TileRight;
        public MenuTile TileAbove;
        public MenuTile TileBelow;
    }
}
