using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour
{

  [SerializeField] private BaseCounter baseCounter;
  [SerializeField] private GameObject[] visualGameObjectArray;

  private void Start()
  {
    Player.Instance.OnSelectedCounterChanged += Player_OnSelectedCounterChanged;
  }

  private void Player_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e)
  {
    if (e.selectedCounter == baseCounter)
    {
      ShowVisual();
    }
    else
    {
      HideVisual();
    }
  }

  private void ShowVisual()
  {
    foreach (GameObject visualGameObject in visualGameObjectArray)
    {
      visualGameObject.SetActive(true);
    }
  }

  private void HideVisual()
  {
    foreach (GameObject visualGameObject in visualGameObjectArray)
    {
      visualGameObject.SetActive(false);
    }
  }
}
