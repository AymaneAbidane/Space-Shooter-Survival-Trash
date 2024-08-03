using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField, SceneObjectsOnly] private GamePlayStatsManager gamePlayStatsManager;
    [SerializeField] private Button TEST_Button;
    private void Awake()
    {
        gamePlayStatsManager.onGameplayStatChanged += GamePlayStatsManager_onGameplayStatChanged;
    }

    private void Start()
    {
        TEST_Button.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        gamePlayStatsManager.onGameplayStatChanged -= GamePlayStatsManager_onGameplayStatChanged;
    }

    private void GamePlayStatsManager_onGameplayStatChanged(object sender, GamePlayStatsManager.GamePlayStats e)
    {
        if (e == GamePlayStatsManager.GamePlayStats.Upgrade)
        {
            TEST_Button.gameObject.SetActive(true);
            TEST_Button.onClick.AddListener(gamePlayStatsManager.ReturnToGamePlayPhase());
            //activate upgrade ui and add to the button the listner to change the gameplaystat if the user press on it 
        }
        else
        {
            TEST_Button.gameObject.SetActive(false);
            TEST_Button.onClick.RemoveAllListeners();
            //remove all listners from buttons
        }
    }
}
