using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GamePlayStatsManager : MonoBehaviour
{
    public enum GamePlayStats { Gameplay, PauseMenu, Upgrade }

    [SerializeField, SceneObjectsOnly] private GameStateManager gameStateManager;


    private GamePlayStats currentGameplayStat;

    public event EventHandler<GamePlayStats> onGameplayStatChanged;

    private void Awake()
    {
        gameStateManager.onGameStateChanged += GameStateManager_onGameStateChanged;

    }
    private void OnDestroy()
    {
        gameStateManager.onGameStateChanged -= GameStateManager_onGameStateChanged;

    }

    private void GameStateManager_onGameStateChanged(object sender, GameStateManager.GameState e)
    {
        if (e == GameStateManager.GameState.GamePlay)
        {
            SetCurrentGameplayStat(GamePlayStats.Gameplay);
        }
    }

    private void SetCurrentGameplayStat(GamePlayStats stat)
    {
        currentGameplayStat = stat;

        if (stat == GamePlayStats.Upgrade || stat == GamePlayStats.PauseMenu)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        onGameplayStatChanged?.Invoke(this, currentGameplayStat);
    }

    public UnityAction ReturnToGamePlayPhase()
    {
        return () =>
        {
            SetCurrentGameplayStat(GamePlayStats.Gameplay);
        };
    }
}
