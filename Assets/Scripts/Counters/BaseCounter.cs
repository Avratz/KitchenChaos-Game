using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{

  public static event EventHandler OnAnyObjectPlacedHere;
  [SerializeField] private Transform counterTopPoint;

  private KitchenObject kitchenObject;

  public virtual void Interact(Player player)
  {
    Debug.LogError("BaseCounter.Interact() called");
  }

  public virtual void InteractAlternate(Player player)
  {
    //Debug.LogError("BaseCounter.Interact() called");
  }

  public Transform KitchenObjectFollowTransform
  {
    get => counterTopPoint;
  }

  public KitchenObject KitchenObject
  {
    get
    {
      return kitchenObject;
    }
    set
    {
      kitchenObject = value;
      if (kitchenObject != null)
      {
        OnAnyObjectPlacedHere?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  public bool HasKitchenObject()
  {
    return kitchenObject != null;
  }
}
