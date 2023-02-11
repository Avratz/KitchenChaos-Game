using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateKitchenObject : KitchenObject
{

  public event EventHandler<OnIngredientAddedEventArgs> OnIngredientAdded;
  public class OnIngredientAddedEventArgs : EventArgs
  {
    public KitchenObjectSO kitchenObjectSO;
  }

  [SerializeField] private List<KitchenObjectSO> validKitchenObjectSOList;

  private List<KitchenObjectSO> kitchenObjectSOList;
  public List<KitchenObjectSO> KitchenObjectSOList
  {
    get { return kitchenObjectSOList; }
  }
  private void Awake()
  {
    kitchenObjectSOList = new List<KitchenObjectSO>();
  }

  public bool TryAddIngredient(KitchenObjectSO kitchenObjectSO)
  {
    if (!validKitchenObjectSOList.Contains(kitchenObjectSO) || kitchenObjectSOList.Contains(kitchenObjectSO))
    {
      return false;
    }

    kitchenObjectSOList.Add(kitchenObjectSO);
    OnIngredientAdded?.Invoke(this, new OnIngredientAddedEventArgs { kitchenObjectSO = kitchenObjectSO });
    return true;
  }
}
