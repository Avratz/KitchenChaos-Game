using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IKitchenObjectParent
{
  public Transform KitchenObjectFollowTransform { get; }
  public KitchenObject KitchenObject { get; set; }
  public bool HasKitchenObject();
}
