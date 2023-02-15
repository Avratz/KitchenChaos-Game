using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsUI : MonoBehaviour
{
  public static OptionsUI Instance { get; private set; }

  [SerializeField] private Button soundEffectsButton;
  [SerializeField] private Button musicButton;
  [SerializeField] private Button closeButton;

  [SerializeField] private Button moveUpButton;
  [SerializeField] private Button moveDownButton;
  [SerializeField] private Button moveLeftButton;
  [SerializeField] private Button moveRightButton;
  [SerializeField] private Button interactButton;
  [SerializeField] private Button interactAlternateButton;
  [SerializeField] private Button pauseButton;

  [SerializeField] private TextMeshProUGUI soundEffectsText;
  [SerializeField] private TextMeshProUGUI musicText;

  [SerializeField] private TextMeshProUGUI moveUpText;
  [SerializeField] private TextMeshProUGUI moveDownText;
  [SerializeField] private TextMeshProUGUI moveLeftText;
  [SerializeField] private TextMeshProUGUI moveRightText;
  [SerializeField] private TextMeshProUGUI interactText;
  [SerializeField] private TextMeshProUGUI interactAlternateText;
  [SerializeField] private TextMeshProUGUI pauseText;

  [SerializeField] private Transform pressToRebindTransform;


  private void Awake()
  {
    Instance = this;

    soundEffectsButton.onClick.AddListener(() =>
    {
      SoundManager.Instance.ChangeVolume();
      UpdateVisual();
    });
    musicButton.onClick.AddListener(() =>
    {
      MusicManager.Instance.ChangeVolume();
      UpdateVisual();
    });

    closeButton.onClick.AddListener(() =>
    {
      Hide();
    });

    moveUpButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Up));
    moveDownButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Down));
    moveLeftButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Left));
    moveRightButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Move_Right));
    interactButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact));
    interactAlternateButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Interact_Alternate));
    pauseButton.onClick.AddListener(() => RebindBinding(GameInput.Binding.Pause));
  }

  private void Start()
  {
    GameManager.Instance.OnGameUnpaused += GameManager_OnGameUnpaused;
    UpdateVisual();
    Hide();
    HidePressToRebind();
  }

  private void GameManager_OnGameUnpaused(object sender, EventArgs e)
  {
    Hide();
  }

  private void UpdateVisual()
  {
    float normalizeMultiplier = 10f;
    soundEffectsText.text = "Sound Effects: " + Mathf.Round(SoundManager.Instance.Volume * normalizeMultiplier);
    musicText.text = "Music: " + Mathf.Round(MusicManager.Instance.Volume * normalizeMultiplier);

    moveUpText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Up);
    moveDownText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Down);
    moveLeftText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Left);
    moveRightText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Move_Right);
    interactText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact);
    interactAlternateText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Interact_Alternate);
    pauseText.text = GameInput.Instance.GetBindingText(GameInput.Binding.Pause);
  }

  public void Show()
  {
    gameObject.SetActive(true);
  }

  public void Hide()
  {
    gameObject.SetActive(false);
  }

  public void ShowPressToRebind()
  {
    pressToRebindTransform.gameObject.SetActive(true);
  }

  public void HidePressToRebind()
  {
    pressToRebindTransform.gameObject.SetActive(false);
  }

  private void RebindBinding(GameInput.Binding binding)
  {
    ShowPressToRebind();
    GameInput.Instance.RebindBinding(binding, () =>
    {
      HidePressToRebind();
      UpdateVisual();
    });
  }
}