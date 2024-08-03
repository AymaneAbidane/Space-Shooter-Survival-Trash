using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameStateManager : SingletonPersistent<GameStateManager>
{
    public enum GameState { MainMenu, GamePlay, GameOver, Quitte }

    [Header("Game Scenes")]
    [SerializeField, SceneObjectsOnly] private GameObject mainMenu;
    [SerializeField, SceneObjectsOnly] private GameObject gamePlay;
    [SerializeField, SceneObjectsOnly] private GameObject gameOverMenu;

    [Space]

    [SerializeField, SceneObjectsOnly] private PlayerHealthManager playerHealthManager;

    [Space]

    [Header("Loading Panel"), SerializeField, SceneObjectsOnly] private GameObject loadingPanel;

    [Space]

    [Header("Transition Between Scenes buttons")]
    [SerializeField, SceneObjectsOnly] private Button playButton;
    [SerializeField, SceneObjectsOnly] private Button backToMainScene;
    [SerializeField, SceneObjectsOnly] private Button quitteButton;

    [Space]

    [SerializeField] private float delayToActivateScene;

    private GameState currentStat;

    public event EventHandler<GameState> onGameStateChanged;

    protected override void Awake()
    {
        base.Awake();

        playButton.onClick.AddListener(BTN_GameplaySceneActivated);
        backToMainScene.onClick.AddListener(BTN_MainSceneActivated);

        playerHealthManager.onPlayerhealthReachZero += PlayerHealthManager_onPlayerhealthReachZero;
    }


    private void Start()
    {
        SetCurrentState(GameState.MainMenu);
    }

    private void OnDestroy()
    {
        playerHealthManager.onPlayerhealthReachZero -= PlayerHealthManager_onPlayerhealthReachZero;
    }

    private void PlayerHealthManager_onPlayerhealthReachZero(object sender, EventArgs e)
    {
        GameOverSceneActivated();
    }

    public void SetCurrentState(GameState newState)
    {
        currentStat = newState;
        if (currentStat != GameState.Quitte)
        {
            SetScensActivation(newState);
        }
        else
        {
            Application.Quit();
        }
    }

    private void SetScensActivation(GameState newState)
    {
        mainMenu.SetActive(false);
        gamePlay.SetActive(false);
        gameOverMenu.SetActive(false);

        loadingPanel.SetActive(true);

        Utils.Delay(this, delayToActivateScene, () =>
        {
            loadingPanel.SetActive(false);
            mainMenu.SetActive(newState == GameState.MainMenu);
            gamePlay.SetActive(newState == GameState.GamePlay);
            gameOverMenu.SetActive(newState == GameState.GameOver);
            onGameStateChanged?.Invoke(this, currentStat);
        });

    }

    private void BTN_GameplaySceneActivated()
    {
        SetCurrentState(GameState.GamePlay);
    }

    private void BTN_MainSceneActivated()
    {
        SetCurrentState(GameState.MainMenu);
        mainMenu.SetActive(false);
    }

    private void GameOverSceneActivated()
    {
        SetCurrentState(GameState.GameOver);
        gameOverMenu.SetActive(false);
    }

    private void BTN_QuitteGame()
    {
        SetCurrentState(GameState.Quitte);
    }
}
