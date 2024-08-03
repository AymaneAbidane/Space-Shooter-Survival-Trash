using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunsManager : MonoBehaviour
{
    [SerializeField, ChildGameObjectsOnly] private Gun[] playerGuns;

    private int playerGunsUpgradeIndex = 1;

    [Button]
    private void TESTUPGRADE_ActivateNextGun()
    {
        playerGunsUpgradeIndex++;
        StartShooting();
    }


    public void StartShooting()
    {
        for (int i = 0; i < playerGunsUpgradeIndex; i++)
        {
            Gun gun = playerGuns[i];
            gun.StartShooting();
        }
    }

    public void AddBulletsToAllGuns()
    {
        foreach (Gun gun in playerGuns) { gun.AddToPool(); }
    }

    public void ResetPlayerGuns()
    {
        foreach (Gun gun in playerGuns) { gun.ResetGun(); }
    }

}
