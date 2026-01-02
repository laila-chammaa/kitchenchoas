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
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    void Update()
    {
        switch (state)
        {
            case State.CountdownToStart:
                countdownToStartTimer -= Time.deltaTime;
                if (countdownToStartTimer < 0)
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
            case State.GameOver or State.GamePaused or State.WaitingToStart:
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
        return (int)Math.Ceiling(countdownToStartTimer);
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - gamePlayingTimer/k_GamePlayingTimerMax;
    }

    void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (state == State.WaitingToStart)
        {
            // Tutorial is showing, and we just received the interact action, hide tutorial and start game
            state = State.CountdownToStart;
        }
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
