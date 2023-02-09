using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitchenObject : MonoBehaviour
{
  [SerializeField] private KitchenObjectSO kitchenObjectSO;

  private IKitchenObjectParent kitchenObjectParent;
  public IKitchenObjectParent KitchenObjectParent
  {
    get => kitchenObjectParent;
    set
    {
      if (kitchenObjectParent != null)
      {
        kitchenObjectParent = null;
      }

      kitchenObjectParent = value;

      if (value.HasKitchenObject())
      {
        Debug.LogError("kitchenObjectParent already has a KitchenObject");
      }

      value.KitchenObject = this;

      transform.parent = kitchenObjectParent.KitchenObjectFollowTransform;
      transform.localPosition = Vector3.zero;
    }
  }

  public KitchenObjectSO KitchenObjectSO
  {
    get => kitchenObjectSO;
  }
}
