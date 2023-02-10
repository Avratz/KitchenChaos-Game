using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : BaseCounter
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        player.KitchenObject.KitchenObjectParent = this;
        player.KitchenObject = null;
      }
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        KitchenObject.KitchenObjectParent = player;
        KitchenObject = null;
      }
      else
      {
        if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
          if (plateKitchenObject.TryAddIngredient(KitchenObject.KitchenObjectSO))
          {
            KitchenObject.DestroySelf();
          }
        }
        else
        {
          if (KitchenObject.TryGetPlate(out plateKitchenObject))
          {
            if (plateKitchenObject.TryAddIngredient(player.KitchenObject.KitchenObjectSO))
            {
              player.KitchenObject.DestroySelf();
            }
          }
        }
      }
    }
  }
}
