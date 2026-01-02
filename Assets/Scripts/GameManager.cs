using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum State
    {
        WaitingToStart,
        CountdownToStart,
        GamePlaying,
        GamePaused,
        GameOver
    }

    State m_State;

    State state
    {
        get => m_State;
        set
        {
            if (m_State == value)
                return;

            m_State = value;
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    float waitingToStartTimer = 1f;
    float countdownToStartTimer = 3f;
    float gamePlayingTimer = 120f;
    const float k_GamePlayingTimerMax = 120f;

    public EventHandler OnStateChanged;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
    }

    void Update()
    {
        switch (state)
        {
            case State.WaitingToStart:
                waitingToStartTimer -= Time.deltaTime;
                if (waitingToStartTimer < 0)
                {
                    state = State.CountdownToStart;
                }
                break;
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 1)
                {
                    state = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer -= Time.deltaTime;
                if (gamePlayingTimer < 0)
                {
                    state = State.GameOver;
                }
                break;
            case State.GameOver or State.GamePaused:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        } 
    }

    public bool IsGamePlaying()
    {
        return state == State.GamePlaying;
    }

    public bool IsGameStartCountDownActive()
    {
        return state == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state == State.GameOver;
    }

    public bool IsGamePaused()
    {
        return state == State.GamePaused;
    }

    public int GetGameStartCountdown()
    {
        return (int) countdownToStartTimer;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - gamePlayingTimer/k_GamePlayingTimerMax;
    }

    void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    public void TogglePauseGame()
    {
        if (state == State.GamePlaying)
        {
            state = State.GamePaused;
            Time.timeScale = 0;
        }
        else if (state == State.GamePaused)
        {
            state = State.GamePlaying;
            Time.timeScale = 1;
        }
    }
}
