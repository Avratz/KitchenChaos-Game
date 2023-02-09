using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;
  [SerializeField] private Transform counterTopPoint;

  private KitchenObject kitchenObject;

  public void Interact(Player player)
  {
    if (kitchenObject == null)
    {
      Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab, counterTopPoint);
      kitchenObjectTransform.GetComponent<KitchenObject>().KitchenObjectParent = this;
    }
    else
    {
      kitchenObject.KitchenObjectParent = player;
    }
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
