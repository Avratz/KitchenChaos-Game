using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingCounter : BaseCounter
{

  public event EventHandler<OnProgressEventArgs> OnProgressChanged;
  public class OnProgressEventArgs : EventArgs
  {
    public float progressNormalized;
  }
  public event EventHandler OnCut;


  [SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

  private int cuttingProgress;

  public override void Interact(Player player)
  {
    if (!HasKitchenObject())
    {
      if (player.HasKitchenObject())
      {
        if (HasRecipeWithInput(player.KitchenObject.KitchenObjectSO))
        {
          cuttingProgress = 0;
          player.KitchenObject.KitchenObjectParent = this;
          player.KitchenObject = null;

          CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.KitchenObjectSO);
          OnProgressChanged?.Invoke(this, new OnProgressEventArgs
          {
            progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
          });
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
      cuttingProgress++;
      CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(KitchenObject.KitchenObjectSO);

      OnCut?.Invoke(this, EventArgs.Empty);
      OnProgressChanged?.Invoke(this, new OnProgressEventArgs
      {
        progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
      });

      if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax)
      {
        KitchenObjectSO outputKitchenObjectSO = GetOutputForInput(KitchenObject.KitchenObjectSO);
        KitchenObject.DestroySelf();
        KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
      }

    }
  }

  private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
  {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);

    if (cuttingRecipeSO != null)
    {
      return cuttingRecipeSO.output;
    }
    return null;
  }

  private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
    return cuttingRecipeSO != null;
  }

  private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray)
    {
      if (cuttingRecipeSO.input == inputKitchenObjectSO)
      {
        return cuttingRecipeSO;
      }
    }
    return null;
  }

}

