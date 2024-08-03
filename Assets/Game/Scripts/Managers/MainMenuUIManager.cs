using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainMenuUIManager : MonoBehaviour
{

    [SerializeField, SceneObjectsOnly] private GameStateManager gameStateManager;

    [Header("Parallax Backgrounds"), SerializeField, SceneObjectsOnly] private Parallax[] bgsParallax;

    [SerializeField, SceneObjectsOnly] private RectTransform gameTitle;
    [SerializeField, SceneObjectsOnly] private RectTransform menuVisual;

    private void OnEnable()
    {
        gameStateManager.onGameStateChanged += GameStateManager_onGameStateChanged;
        gameTitle.localScale = Vector3.zero;
        menuVisual.localScale = Vector3.zero;
    }

    private void OnDisable()
    {
        gameStateManager.onGameStateChanged -= GameStateManager_onGameStateChanged;

    }

    private void GameStateManager_onGameStateChanged(object sender, GameStateManager.GameState e)
    {
        if (e == GameStateManager.GameState.MainMenu)
        {
            SelectrandomParallaxBg();

            gameTitle.DOScale(Vector3.one, 1f).From(Vector3.zero).OnComplete(() =>
            {
                menuVisual.DOScale(Vector3.one, 1f).From(Vector3.zero);
            });
        }
    }

    private void SelectrandomParallaxBg()
    {
        bgsParallax[UnityEngine.Random.Range(0, bgsParallax.Length)].gameObject.SetActive(true);
    }
}

