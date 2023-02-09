using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{
  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.KitchenObject.KitchenObjectSO))
        {
          player.KitchenObject.KitchenObjectParent = this;
          player.KitchenObject = null;
        }
      }
    }
    else
    {
      if (!player.HasKitchenObject())
      {
        KitchenObject.KitchenObjectParent = player;
        KitchenObject = null;
      }
    }
  }

  public override void InteractAlternate(Player player)
  {
    if (HasKitchenObject() && HasRecipeWithInput(KitchenObject.KitchenObjectSO))
    {
      KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(KitchenObject.KitchenObjectSO);
      KitchenObject.DestroySelf();

      KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
    }
  }

  private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
  {
    foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
    {
      if (cuttingRecipeSO.input == inputKitchenObjectSO)
      {
        return cuttingRecipeSO.output;
      }
    }
    return null;
  }

  private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    return GetOutputForInput(inputKitchenObjectSO) != null;
  }

}

