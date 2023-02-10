using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress
{

  public event EventHandler<IHasProgress.OnProgressEventArgs> OnProgressChanged;

  public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
  public class OnStateChangedEventArgs : EventArgs
  {
    public State state;
  }


  public enum State
  {
    Idle,
    Frying,
    Fried,
    Burned
  }

  [SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
  [SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

  private State state;
  private float fryingTimer = 0f;
  private float burningTimer = 0f;
  private FryingRecipeSO fryingRecipeSO;
  private BurningRecipeSO burningRecipeSO;

  private void Start()
  {
    state = State.Idle;
  }

  private void Update()
  {
    if (HasKitchenObject())
    {
      switch (state)
      {
        case State.Idle:
          break;
        case State.Frying:
          fryingTimer += Time.deltaTime;
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
          {
            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
          });
          if (fryingTimer > fryingRecipeSO.fryingTimerMax)
          {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);

            burningRecipeSO = GetBurningRecipeSOWithInput(KitchenObject.KitchenObjectSO);
            state = State.Fried;
            burningTimer = 0f;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
              state = state
            });
          }
          break;
        case State.Fried:
          burningTimer += Time.deltaTime;
          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
          {
            progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
          });
          if (burningTimer > burningRecipeSO.burningTimerMax)
          {
            KitchenObject.DestroySelf();
            KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
            state = State.Burned;
            burningTimer = 0f;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
              state = state
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
            {
              progressNormalized = 0f
            });
          }
          break;
        case State.Burned:
          break;
      }
    }


  }

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
          fryingRecipeSO = GetFryingRecipeSOWithInput(KitchenObject.KitchenObjectSO);
          state = State.Frying;
          fryingTimer = 0f;
          OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
          {
            state = state
          });

          OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
          {
            progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
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
        state = State.Idle;
        OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
        {
          state = state
        });

        OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
        {
          progressNormalized = 0f
        });
      }
      else
      {
        if (player.KitchenObject.TryGetPlate(out PlateKitchenObject plateKitchenObject))
        {
          if (plateKitchenObject.TryAddIngredient(KitchenObject.KitchenObjectSO))
          {
            KitchenObject.DestroySelf();

            state = State.Idle;
            OnStateChanged?.Invoke(this, new OnStateChangedEventArgs
            {
              state = state
            });

            OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressEventArgs
            {
              progressNormalized = 0f
            });
          }
        }
      }
    }
  }

  private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO)
  {
    FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);

    if (fryingRecipeSO != null)
    {
      return fryingRecipeSO.output;
    }
    return null;
  }

  private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
    return fryingRecipeSO != null;
  }

  private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray)
    {
      if (fryingRecipeSO.input == inputKitchenObjectSO)
      {
        return fryingRecipeSO;
      }
    }
    return null;
  }

  private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO)
  {
    foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray)
    {
      if (burningRecipeSO.input == inputKitchenObjectSO)
      {
        return burningRecipeSO;
      }
    }
    return null;
  }
}
