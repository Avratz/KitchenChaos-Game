using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatesCounterVisual : MonoBehaviour
{
  [SerializeField] private PlatesCounter platesCounter;
  [SerializeField] private Transform counterTopPoint;
  [SerializeField] private Transform plateVisualPrefab;

  private List<GameObject> platesVisualsGameObjectList;

  private void Awake()
  {
    platesVisualsGameObjectList = new List<GameObject>();
  }

  private void Start()
  {
    platesCounter.OnPlateSpawned += PlatesCounter_OnPlateSpawned;
    platesCounter.OnPlateRemoved += PlatesCounter_OnPlateRemoved;
  }

  private void PlatesCounter_OnPlateRemoved(object sender, EventArgs e)
  {
    if (platesVisualsGameObjectList.Count > 0)
    {
      GameObject lastPlateVisualGameObject = platesVisualsGameObjectList[platesVisualsGameObjectList.Count - 1];
      platesVisualsGameObjectList.Remove(lastPlateVisualGameObject);
      Destroy(lastPlateVisualGameObject);
    }
  }

  private void PlatesCounter_OnPlateSpawned(object sender, EventArgs e)
  {
    Transform plateVisualTransform = Instantiate(plateVisualPrefab, counterTopPoint);

    float plateOffsetY = 0.1f;
    plateVisualTransform.localPosition = new Vector3(0f, plateOffsetY * platesVisualsGameObjectList.Count, 0f);
    platesVisualsGameObjectList.Add(plateVisualTransform.gameObject);
  }
}
