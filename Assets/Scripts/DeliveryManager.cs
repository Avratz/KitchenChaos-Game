using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour
{

  public event EventHandler OnRecipeSpawned;
  public event EventHandler OnRecipeCompleted;
  public event EventHandler OnRecipeSuccess;
  public event EventHandler OnRecipeFailed;


  public static DeliveryManager Instance { get; private set; }

  [SerializeField] private RecipeListSO recipeListSO;
  private List<RecipeSO> waitingRecipeSOList;
  private float spawnRecipeTimer;
  private float spawnRecipeTimerMax = 4f;
  private int waitingRecipesMax = 4;
  private int successfulRecipesAmount = 0;

  private void Awake()
  {
    Instance = this;
    waitingRecipeSOList = new List<RecipeSO>();
  }

  private void Update()
  {
    spawnRecipeTimer -= Time.deltaTime;
    if (spawnRecipeTimer <= 0f)
    {
      spawnRecipeTimer += spawnRecipeTimerMax;
      if (waitingRecipeSOList.Count < waitingRecipesMax)
      {
        RecipeSO recipeSO = recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)];
        waitingRecipeSOList.Add(recipeSO);
        OnRecipeSpawned?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  public void DeliverRecipe(PlateKitchenObject plateKitchenObject)
  {
    for (int i = 0; i < waitingRecipeSOList.Count; i++)
    {
      RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
      if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.KitchenObjectSOList.Count)
      {
        bool plateContentsMatchedRecipe = true;

        foreach (KitchenObjectSO recipeKitchenObjectSO in waitingRecipeSO.kitchenObjectSOList)
        {
          bool ingredientFound = false;
          foreach (KitchenObjectSO plateKitchenObjectSO in plateKitchenObject.KitchenObjectSOList)
          {
            if (plateKitchenObjectSO == recipeKitchenObjectSO)
            {
              ingredientFound = true;
              break;
            }
          }
          if (!ingredientFound)
          {
            plateContentsMatchedRecipe = false;
            break;
          }
        }

        if (plateContentsMatchedRecipe)
        {
          waitingRecipeSOList.RemoveAt(i);
          successfulRecipesAmount++;
          OnRecipeCompleted?.Invoke(this, EventArgs.Empty);
          OnRecipeSuccess?.Invoke(this, EventArgs.Empty);
          return;
        }
      }
    }
    OnRecipeFailed?.Invoke(this, EventArgs.Empty);
  }

  public List<RecipeSO> WaitingRecipeSOList
  {
    get { return waitingRecipeSOList; }
  }

  public int SuccessfulRecipesAmount
  {
    get { return successfulRecipesAmount; }
  }

}
