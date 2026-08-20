using UnityEngine;
using static BetterLanternsAndLights.Configs;

namespace BetterLanternsAndLights
{
    internal class InventoryLantern : MonoBehaviour
    {
        internal GPButtonInventorySlot InventorySlot { get; set; }

        private void LateUpdate()
        {
            if (!enableInventoryLantern.Value || !InventorySlot.IsLookedAt())
                return; 

            var currentItem = InventorySlot.currentItem;

            if (currentItem is ShipItemLight && (GameInput.GetKeyDown(InputName.Activate) || Input.GetMouseButtonDown(1)))
                currentItem.OnAltActivate();
        }
    }
}
