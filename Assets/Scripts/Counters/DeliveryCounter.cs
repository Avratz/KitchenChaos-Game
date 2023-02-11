using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
  public override void Interact(Player player)
  {
    if (player.KitchenObject)
    {
      if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
      {
        player.KitchenObject.DestroySelf();
      }
    }
  }
}
