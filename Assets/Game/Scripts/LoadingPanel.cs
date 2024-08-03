using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingPanel : MonoBehaviour
{
    [SerializeField, ChildGameObjectsOnly] private Image loadingImage;
    [SerializeField, AssetsOnly] private Sprite[] loadingBarSpritesColor;
    [Space]
    [SerializeField, InfoBox("it should be always equal to the time on the game state manager")] private float delayForIncreassing;

    private void OnEnable()
    {
        LoadBar();
    }

    private void LoadBar()
    {
        float barProgres = 0f;
        SetLoadingBarValue(0f);
        loadingImage.sprite = loadingBarSpritesColor[UnityEngine.Random.Range(0, loadingBarSpritesColor.Length)];
        loadingImage.SetNativeSize();

        DOTween.To(() => barProgres, x => barProgres = x, 1f, delayForIncreassing).OnUpdate(() =>
        {
            SetLoadingBarValue(barProgres);
        });
    }

    private void SetLoadingBarValue(float f)
    {
        loadingImage.fillAmount = f;
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }
}
