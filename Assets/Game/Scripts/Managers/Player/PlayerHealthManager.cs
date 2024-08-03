using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    [SerializeField, SceneObjectsOnly] private GameStateManager gameStateManager;
    [SerializeField, SceneObjectsOnly] private Player player;
    [SerializeField, SceneObjectsOnly] private Slider healthSlider;
    [SerializeField, SceneObjectsOnly] private TextMeshProUGUI healtTextValue;
    [SerializeField, SceneObjectsOnly] private AnimationsEventsController playerEventsController;
    public event EventHandler onPlayerhealthReachZero;

    private void Awake()
    {
        player.onPlayerhealthChanged += Player_onPlayerhealthChanged;
        gameStateManager.onGameStateChanged += GameStateManager_onGameStateChanged;
        playerEventsController.onPlayerDeathAnimationFinished += PlayerEventsController_onPlayerDeathAnimationFinished;
    }

    private void PlayerEventsController_onPlayerDeathAnimationFinished(object sender, EventArgs e)
    {
        onPlayerhealthReachZero?.Invoke(this, EventArgs.Empty);
    }

    private void GameStateManager_onGameStateChanged(object sender, GameStateManager.GameState e)
    {
        if (e == GameStateManager.GameState.GamePlay)
        {
            SetMinAndMaxValueToSlider(player.GetHealthValue());
        }
    }


    private void OnDestroy()
    {
        player.onPlayerhealthChanged -= Player_onPlayerhealthChanged;
        gameStateManager.onGameStateChanged -= GameStateManager_onGameStateChanged;
        playerEventsController.onPlayerDeathAnimationFinished -= PlayerEventsController_onPlayerDeathAnimationFinished;

    }

    private void Player_onPlayerhealthChanged(object sender, float e)
    {
        SetHealthValueToSlider(e);


    }

    private void SetHealthValueToSlider(float e)
    {
        healthSlider.value = e;
        healtTextValue.text = healthSlider.value.ToString() + " / " + healthSlider.maxValue.ToString();
    }

    private void SetMinAndMaxValueToSlider(float v)
    {
        healthSlider.minValue = 0f;
        healthSlider.maxValue = v;
        SetHealthValueToSlider(v);
    }
}
