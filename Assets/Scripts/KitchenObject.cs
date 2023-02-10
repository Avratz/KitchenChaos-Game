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

  public void DestroySelf()
  {
    kitchenObjectParent.KitchenObject = null;
    Destroy(gameObject);
  }

  public static KitchenObject SpawnKitchenObject(KitchenObjectSO kitchenObjectSO, IKitchenObjectParent kitchenObjectParent)
  {
    Transform kitchenObjectTransform = Instantiate(kitchenObjectSO.prefab);
    KitchenObject kitchenObject = kitchenObjectTransform.GetComponent<KitchenObject>();
    kitchenObject.KitchenObjectParent = kitchenObjectParent;
    return kitchenObject;
  }

  public bool TryGetPlate(out PlateKitchenObject plateKitchenObject)
  {
    plateKitchenObject = null;
    if (this is PlateKitchenObject)
    {
      plateKitchenObject = this as PlateKitchenObject;
      return true;
    }

    return false;
  }
}
