using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCounter : MonoBehaviour, IKitchenObjectParent
{
  [SerializeField] private Transform counterTopPoint;

  private KitchenObject kitchenObject;

  public virtual void Interact(Player player)
  {
    Debug.LogError("BaseCounter.Interact() called");
  }

  public Transform KitchenObjectFollowTransform
  {
    get => counterTopPoint;
  }

  public KitchenObject KitchenObject
  {
    get => kitchenObject;
    set => kitchenObject = value;
  }

  public bool HasKitchenObject()
  {
    return kitchenObject != null;
  }
}