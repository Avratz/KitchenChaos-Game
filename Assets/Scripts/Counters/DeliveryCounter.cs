using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCounter : BaseCounter
{
  public static DeliveryCounter Instance { get; private set; }

  private void Awake()
  {
    Instance = this;
  }

  public override void Interact(Player player)
  {
    if (player.KitchenObject)
    {
      if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
      {
        DeliveryManager.Instance.DeliverRecipe(plateKitchenObject);
        player.KitchenObject.DestroySelf();
      }
    }
  }
}
