using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimationsController : MonoBehaviour
{
    [SerializeField] private Ship.ShipType shipType;

    [SerializeField, ChildGameObjectsOnly] private Animator animator;

    private const string BOOL_DEATH_ANIMATION_STRING = "IsDead";

    private void Enemy_onEnemyShipDestroyed(object sender, System.EventArgs e)
    {
        PlayDeathAnimation();
    }

    public void PlayDeathAnimation()
    {
        animator.SetBool(BOOL_DEATH_ANIMATION_STRING, true);
    }

    public void PlayIdlAnimation()
    {
        animator.SetBool(BOOL_DEATH_ANIMATION_STRING, false);
    }
}
