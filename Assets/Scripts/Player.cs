using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
  public static Player Instance { get; private set; }

  public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
  public class OnSelectedCounterChangedEventArgs : EventArgs
  {
    public ClearCounter selectedCounter;
  }

  [SerializeField] private float movementSpeed = 5f;
  [SerializeField] private GameInput gameInput;
  [SerializeField] private LayerMask layerMask;
  [SerializeField] private Transform kitchenObjectHoldPoint;

  private bool isWalking = false;
  private Vector3 lastInteractDirection;
  private ClearCounter selectedCounter;
  private KitchenObject kitchenObject;

  private void Awake()
  {
    if (Instance != null)
    {
      Debug.LogError("More than one instance of Player found!");
    }
    Instance = this;
  }

  private void Start()
  {
    gameInput.OnInteractAction += GameInput_OnInteractAction;
  }

  private void Update()
  {
    HandleInteractions();
    HandleMovement();
  }

  public bool IsWalking
  {
    get => isWalking;
  }

  private void GameInput_OnInteractAction(object sender, System.EventArgs e)
  {
    if (selectedCounter != null)
    {
      selectedCounter.Interact(this);
    }
  }

  private void HandleInteractions()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

    if (moveDirection != Vector3.zero)
    {
      lastInteractDirection = moveDirection;
    }

    float interactDistance = 2f;
    if (Physics.Raycast(
      transform.position,
      lastInteractDirection,
      out RaycastHit raycastHit,
      interactDistance,
      layerMask
    ))
    {
      if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter))
      {
        if (clearCounter != selectedCounter)
        {
          SetSelectedCounter(clearCounter);
        }
      }
      else
      {
        SetSelectedCounter(null);
      }
    }
    else
    {
      SetSelectedCounter(null);
    }
  }

  private void SetSelectedCounter(ClearCounter clearCounter)
  {
    selectedCounter = clearCounter;
    OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
  }

  private void HandleMovement()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();
    Vector3 moveDirection = new Vector3(inputVector.x, 0f, inputVector.y);

    float moveDistance = movementSpeed * Time.deltaTime;
    float playerRadius = 0.7f;
    float playerHeight = 2f;

    bool canMove = !Physics.CapsuleCast(
      transform.position,
      transform.position + Vector3.up * playerHeight,
      playerRadius,
      moveDirection,
      moveDistance
    );

    if (!canMove)
    {
      Vector3 moveDirX = new Vector3(moveDirection.x, 0f, 0f).normalized;
      canMove = !Physics.CapsuleCast(
        transform.position,
        transform.position + Vector3.up * playerHeight,
        playerRadius,
        moveDirX,
        moveDistance
      );

      if (canMove)
      {
        moveDirection = moveDirX;
      }
      else
      {
        Vector3 moveDirZ = new Vector3(0f, 0f, moveDirection.z).normalized;
        canMove = !Physics.CapsuleCast(
          transform.position,
          transform.position + Vector3.up * playerHeight,
          playerRadius,
          moveDirZ,
          moveDistance
        );

        if (canMove)
        {
          moveDirection = moveDirZ;
        }
      }

    }

    if (canMove)
    {
      transform.position += moveDirection * moveDistance;
    }

    isWalking = moveDirection != Vector3.zero;

    float rotateSpeed = 10f;
    transform.forward = Vector3.Slerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
  }

  public Transform KitchenObjectFollowTransform
  {
    get => kitchenObjectHoldPoint;
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
