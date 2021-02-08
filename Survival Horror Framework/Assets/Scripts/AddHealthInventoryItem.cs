////////////////////////////////////////
// Author:              LEAKYFINGERS
// Date created:        08.02.21
// Date last edited:    08.02.21
////////////////////////////////////////
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SurvivalHorrorFramework
{
    [CreateAssetMenu(fileName = "Data", menuName = "Scriptable Objects/Inventory Items/Add Health")]
    public class AddHealthInventoryItem : InventoryItem
    {
        public uint HealthToAddWhenUsed = 25;

        public override void Use()
        {
            TankControlPlayer player = GameObject.FindGameObjectWithTag("Player").GetComponent<TankControlPlayer>();
            player.SetHealth((uint)Mathf.Clamp(player.CurrentHealth + HealthToAddWhenUsed, 0, player.MaxHealth));
        }
    }
}
