using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
  [SerializeField]
  private float movementSpeed = 5f;

  private void Update()
  {
    Vector2 inputVector = new Vector2(0, 0);
    if (Input.GetKey(KeyCode.W))
    {
      inputVector.y = 1;
    }
    if (Input.GetKey(KeyCode.S))
    {
      inputVector.y = -1;
    }

    if (Input.GetKey(KeyCode.A))
    {
      inputVector.x = -1;
    }

    if (Input.GetKey(KeyCode.D))
    {
      inputVector.x = 1;
    }

    inputVector = inputVector.normalized;
    Vector3 movementDirection = new Vector3(inputVector.x, 0f, inputVector.y);
    transform.position += movementDirection * movementSpeed * Time.deltaTime;

    float rotateSpeed = 10f;
    transform.forward = Vector3.Slerp(transform.forward, movementDirection, Time.deltaTime * rotateSpeed);
  }
}
