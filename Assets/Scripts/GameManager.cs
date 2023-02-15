using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

  public static GameManager Instance { get; private set; }

  public event EventHandler OnStateChanged;
  public event EventHandler OnGamePaused;
  public event EventHandler OnGameUnpaused;

  private enum State
  {
    WaitingToStart,
    CountdownToStart,
    GamePlaying,
    GameOver
  }

  private State state;
  private float countdownToStartTimer = 3f;
  private float gamePlayingTimer;
  private float gamePlayingTimerMax = 60f;
  private bool isGamePaused = false;

  private void Awake()
  {
    Instance = this;
    state = State.WaitingToStart;
  }

  private void Start()
  {
    GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;
    GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
  }

  private void GameInput_OnInteractAction(object sender, EventArgs e)
  {
    if (state == State.WaitingToStart)
    {
      state = State.CountdownToStart;
      OnStateChanged?.Invoke(this, System.EventArgs.Empty);
    }
  }

  private void GameInput_OnPauseAction(object sender, EventArgs e)
  {
    TogglePauseGame();
  }

  private void Update()
  {
    switch (state)
    {
      case State.WaitingToStart:
        break;
      case State.CountdownToStart:
        countdownToStartTimer -= Time.deltaTime;
        if (countdownToStartTimer <= 0f)
        {
          state = State.GamePlaying;
          gamePlayingTimer = gamePlayingTimerMax;
          OnStateChanged?.Invoke(this, System.EventArgs.Empty);
        }
        break;
      case State.GamePlaying:
        gamePlayingTimer -= Time.deltaTime;
        if (gamePlayingTimer <= 0f)
        {
          state = State.GameOver;
          OnStateChanged?.Invoke(this, System.EventArgs.Empty);
        }
        break;
      case State.GameOver:
        break;
    }
  }

  public void TogglePauseGame()
  {
    isGamePaused = !isGamePaused;
    if (isGamePaused)
    {
      Time.timeScale = 0f;
      OnGamePaused?.Invoke(this, System.EventArgs.Empty);
    }
    else
    {
      Time.timeScale = 1f;
      OnGameUnpaused?.Invoke(this, System.EventArgs.Empty);
    }

  }

  public bool IsGamePlaying
  {
    get => state == State.GamePlaying;
  }

  public bool IsCountdownToStartActive
  {
    get => state == State.CountdownToStart;
  }

  public float CountdownToStartTimer
  {
    get => countdownToStartTimer;
  }

  public bool IsGameOver
  {
    get => state == State.GameOver;
  }

  public float GamePlayingTimerNormalized
  {
    get => 1 - (gamePlayingTimer / gamePlayingTimerMax);
  }
}
