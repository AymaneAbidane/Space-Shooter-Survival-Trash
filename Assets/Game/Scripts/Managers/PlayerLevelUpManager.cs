using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerLevelUpManager : MonoBehaviour
{
    [SerializeField, SceneObjectsOnly] private GameStateManager gameStateManager;
    [SerializeField, SceneObjectsOnly] private EnemySpawnManager enemySpawnManager;

    [SerializeField] private int currentPlayerLevel;

    [Header("LevelUp Assets and Varriables")]
    [SerializeField] private Slider xpBarSlider;
    [SerializeField] private float xpBarStartingValue;
    [SerializeField] private float xpBarMultiplayerAfterLevelUp;

    public event EventHandler onPlayerLevelUp;
    private float maxXpValue;

    private List<Enemy> smallShipsInPoolList = new();

    private void Awake()
    {
        gameStateManager.onGameStateChanged += GameStateManager_onGameStateChanged;
    }

    private void Start()
    {
        smallShipsInPoolList = enemySpawnManager.GetPoolShips();
        maxXpValue = xpBarStartingValue;
        ResetXpBar();
    }

    private void ResetXpBar()
    {
        xpBarSlider.minValue = 0;
        xpBarSlider.value = 0f;
        xpBarSlider.maxValue = maxXpValue;
        currentPlayerLevel = 1;
    }

    private void OnDestroy()
    {
        gameStateManager.onGameStateChanged -= GameStateManager_onGameStateChanged;
        SubOrDesubFromShips(smallShipsInPoolList, false);
    }

    private void GameStateManager_onGameStateChanged(object sender, GameStateManager.GameState e)
    {
        if (e == GameStateManager.GameState.GamePlay)
        {
            SubOrDesubFromShips(smallShipsInPoolList, true);
            //sub to the ships because they will give u xp for the xpbar
            // and other stuff
        }
        else
        {
            currentPlayerLevel = 1;
            SubOrDesubFromShips(smallShipsInPoolList, false);
            ResetXpBar();
            //desubscribe from every thing
        }
    }

    private void SubOrDesubFromShips(List<Enemy> shipsList, bool sub)
    {
        if (sub == true)
        {
            foreach (Enemy ships in shipsList)
            {
                ships.onEnemyShipDestroyed += Sh_onEnemyShipDestroyed;
            }
        }
        else
        {
            foreach (Enemy ships in shipsList)
            {
                ships.onEnemyShipDestroyed -= Sh_onEnemyShipDestroyed;
            }
        }
    }

    private void Sh_onEnemyShipDestroyed(object sender, float e)
    {
        xpBarSlider.value += e;
        currentPlayerLevel++;
        if (xpBarSlider.value >= xpBarSlider.maxValue)
        {
            onPlayerLevelUp?.Invoke(this, EventArgs.Empty);
            maxXpValue *= xpBarMultiplayerAfterLevelUp;
            ResetXpBar();
        }
    }
}
