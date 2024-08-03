using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Ship : MonoBehaviour
{
    public enum ShipType { Player, Enemy }

    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float maxhealth;
    [SerializeField] protected ShipType type;
    [SerializeField, ShowIf("type", ShipType.Enemy)] protected bool hasAGun;
    [SerializeField, ChildGameObjectsOnly, ShowIf("type", ShipType.Enemy), ShowIf("hasAGun", true)] protected Gun ownGun;
    [SerializeField, ChildGameObjectsOnly, ShowIf("type", ShipType.Player)] protected PlayerGunsManager playerGunsManager;
    [SerializeField, ShowIf("type", ShipType.Enemy)] protected Collider2D enemyShipCollider;

    public event EventHandler<float> onPlayerhealthChanged;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (type == ShipType.Enemy)
        {
            //if (collision.gameObject.TryGetComponent<Bullet>(out Bullet playerBullet) )
            //{
            //    Debug.Break();
            //    //TakeDamage(playerBullet.GetDammage());
            //}
            //take dmg from player
        }
        else if (type == ShipType.Player)
        {
            if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemy))
            {
                //take dng from enemys
                TakeDamage(enemy.GetCollisionDamage());
                Debug.Log(" player Collide with an Enemy ship");
                enemy.ResetAndBackToPool();
            }
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        //taking dmg by seraching the script the collider that attach to it have the data our damag hit

        if (type == ShipType.Player)
        {

            //take dng from enemys bullets

        }
        else if (type == ShipType.Enemy)
        {
            if (collision.attachedRigidbody.TryGetComponent<Bullet>(out Bullet playerBullet))
            {
                PlayGettingHitAnimation();
                TakeDamage(playerBullet.GetDammage());
                playerBullet.ResetBullet();
            }
            //take dmg from player

        }
    }


    protected abstract void Move();
    protected abstract void DestroyShip();

    protected virtual void PlayGettingHitAnimation() { }

    protected virtual void TakeDamage(float damage)
    {
        if (maxhealth > 0f)
        {
            maxhealth -= damage;

            if (type == ShipType.Player)
            {
                if (maxhealth <= 0f)
                {

                    InvokHealthEvent(0f);
                    DestroyShip();


                }
                else
                {
                    InvokHealthEvent(maxhealth);
                }
            }
        }
    }
    protected virtual void InvokHealthEvent(float health)
    {
        if (type == ShipType.Player)
        {
            onPlayerhealthChanged?.Invoke(this, health);
        }
    }
}
