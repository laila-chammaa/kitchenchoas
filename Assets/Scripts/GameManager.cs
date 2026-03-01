using System;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
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

    NetworkVariable<State> state = new NetworkVariable<State>(State.WaitingToStart);
    NetworkVariable<float> gamePlayingTimer = new NetworkVariable<float>(100f);
    NetworkVariable<float> countdownToStartTimer = new NetworkVariable<float>(3f);
    public bool localPlayerReady { get; private set; }
    const float k_GamePlayingTimerMax = 100f;

    Dictionary<ulong, bool> playersReadyDict;

    public EventHandler OnStateChanged;
    public EventHandler OnLocalPlayerReady;

    void Awake()
    {
        Instance = this;
        playersReadyDict = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();
        state.OnValueChanged += (value, newValue) =>
        {
            OnStateChanged?.Invoke(this, EventArgs.Empty);
        };
    }

    void Start()
    {
        GameInput.Instance.OnPauseAction += GameInputOnPauseAction;
        GameInput.Instance.OnInteractAction += GameInputOnInteractAction;
    }

    void Update()
    {
        if (!IsServer)
            return;
        
        switch (state.Value)
        {
            case State.CountdownToStart:
                countdownToStartTimer.Value -= Time.deltaTime;
                if (countdownToStartTimer.Value < 0)
                {
                    state.Value = State.GamePlaying;
                }
                break;
            case State.GamePlaying:
                gamePlayingTimer.Value -= Time.deltaTime;
                if (gamePlayingTimer.Value < 0)
                {
                    state.Value = State.GameOver;
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
        return state.Value == State.GamePlaying;
    }

    public bool IsGameStartCountDownActive()
    {
        return state.Value == State.CountdownToStart;
    }

    public bool IsGameOver()
    {
        return state.Value == State.GameOver;
    }

    public bool IsGamePaused()
    {
        return state.Value == State.GamePaused;
    }

    public float GetGameStartCountdown()
    {
        return countdownToStartTimer.Value;
    }

    public float GetGamePlayingTimerNormalized()
    {
        return 1 - gamePlayingTimer.Value/k_GamePlayingTimerMax;
    }

    void GameInputOnPauseAction(object sender, EventArgs e)
    {
        TogglePauseGame();
    }

    void GameInputOnInteractAction(object sender, EventArgs e)
    {
        if (state.Value == State.WaitingToStart)
        {
            localPlayerReady = true;
            OnLocalPlayerReady?.Invoke(this, EventArgs.Empty);
            SetPlayerReadyServerRpc();
        }
    }

    [Rpc(SendTo.Server, InvokePermission = RpcInvokePermission.Everyone)]
    void SetPlayerReadyServerRpc(RpcParams rpcParams = default)
    {
        playersReadyDict[rpcParams.Receive.SenderClientId] = true;

        var allPlayersReady = true;
        foreach (var clientsId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            if (!playersReadyDict.ContainsKey(clientsId) || !playersReadyDict[clientsId])
            {
                allPlayersReady = false;
                break;
            }
        }

        if (allPlayersReady)
            state.Value = State.CountdownToStart;
    }

    public void TogglePauseGame()
    {
        if (state.Value == State.GamePlaying)
        {
            state.Value = State.GamePaused;
            Time.timeScale = 0;
        }
        else if (state.Value == State.GamePaused)
        {
            state.Value = State.GamePlaying;
            Time.timeScale = 1;
        }
    }
}
