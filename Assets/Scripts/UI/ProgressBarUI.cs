using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarUI : MonoBehaviour
{

  [SerializeField] private GameObject hasProgressGameObject;
  [SerializeField] private Image barImage;
  private IHasProgress hasProgress;

  private void Start()
  {
    hasProgress = hasProgressGameObject.GetComponent<IHasProgress>();
    if (hasProgress == null)
    {
      Debug.LogError("hasProgressGameObject does not implements IHasProgress");
    }

    hasProgress.OnProgressChanged += HasProgress_OnProgressChanged;
    barImage.fillAmount = 0f;
    Hide();
  }

  private void HasProgress_OnProgressChanged(object sender, IHasProgress.OnProgressEventArgs e)
  {
    barImage.fillAmount = e.progressNormalized;
    if (e.progressNormalized == 1f || e.progressNormalized == 0f)
    {
      Hide();
    }
    else
    {
      Show();
    }
  }

  private void Show()
  {
    gameObject.SetActive(true);
  }

  private void Hide()
  {
    gameObject.SetActive(false);
  }
}
