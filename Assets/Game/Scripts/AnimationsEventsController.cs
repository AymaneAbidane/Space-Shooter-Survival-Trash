using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsEventsController : MonoBehaviour
{
    [SerializeField] private Ship.ShipType shipType;

    public event EventHandler onPlayerDeathAnimationFinished;
    public event EventHandler onEnemyDeathAnimationFinished;

    public void KEYFRAM_PlayerDeathAnimationFinished()
    {
        if (shipType == Ship.ShipType.Player)
        {
            onPlayerDeathAnimationFinished?.Invoke(this, EventArgs.Empty);
        }
        else if (shipType == Ship.ShipType.Enemy)
        {
            onEnemyDeathAnimationFinished?.Invoke(this, EventArgs.Empty);
        }
    }

}
