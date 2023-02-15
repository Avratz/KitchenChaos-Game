using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{

  public static SoundManager Instance { get; private set; }

  [SerializeField] private AudioClipRefsSO audioClipRefsSO;

  private float volume = 1f;

  private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

  private void Awake()
  {
    Instance = this;
    volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
  }

  private void Start()
  {
    DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnRecipeSuccess;
    DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnRecipeFailed;
    CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
    Player.Instance.OnPickSomething += Player_OnPickSomething;
    BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
    TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
  }

  private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e)
  {
    TrashCounter trashCounter = sender as TrashCounter;
    PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
  }

  private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e)
  {
    BaseCounter baseCounter = sender as BaseCounter;
    PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
  }

  private void Player_OnPickSomething(object sender, EventArgs e)
  {
    PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
  }

  private void CuttingCounter_OnAnyCut(object sender, EventArgs e)
  {
    CuttingCounter cuttingCounter = sender as CuttingCounter;
    PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
  }

  private void DeliveryManager_OnRecipeFailed(object sender, EventArgs e)
  {
    DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
    PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);
  }

  private void DeliveryManager_OnRecipeSuccess(object sender, EventArgs e)
  {
    DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
    PlaySound(audioClipRefsSO.deliverySuccess, deliveryCounter.transform.position);
  }

  private void PlaySound(AudioClip[] audioClipsArray, Vector3 position, float volume = 1f)
  {
    int randomIndex = UnityEngine.Random.Range(0, audioClipsArray.Length);
    PlaySound(audioClipsArray[randomIndex], position, volume);
  }

  private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f)
  {
    AudioSource.PlayClipAtPoint(audioClip, position, volumeMultiplier * volume);
  }

  public void PlayFootstepsSound(Vector3 position, float volume = 1f)
  {
    PlaySound(audioClipRefsSO.footstep, position, volume);
  }

  public void PlayCountdownSound()
  {
    PlaySound(audioClipRefsSO.warning, Vector3.zero);
  }

  public void PlayWarningSound(Vector3 position)
  {
    PlaySound(audioClipRefsSO.warning, position);
  }

  public void ChangeVolume()
  {
    volume += 0.1f;
    if (volume > 1f)
      volume = 0f;

    PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
    PlayerPrefs.Save();
  }

  public float Volume
  {
    get { return volume; }
  }
}
