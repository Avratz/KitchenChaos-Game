using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  private float movementSpeed = 5f;
  [SerializeField]
  private GameInput gameInput;

  private bool isWalking = false;

  private void Update()
  {
    Vector2 inputVector = gameInput.GetMovementVectorNormalized();

    Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    transform.position += movementDirection * movementSpeed * Time.deltaTime;

    isWalking = movementDirection != Vector3.zero;

    float rotateSpeed = 10f;
    transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotateSpeed);
  }

  public bool IsWalking
  {
    get => isWalking;
  }
}
