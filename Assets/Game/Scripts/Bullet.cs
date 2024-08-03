using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Ship.ShipType bulletPrefabOfThe;
    [SerializeField] private float speed;
    [SerializeField] private float damage;
    private Transform originalParent;

    private bool canMove;
    private Gun.GunShootingDirection directionOfMovment;

    public float GetDammage()
    {
        return damage;
    }

    private void Update()
    {
        if (canMove == true)
        {
            Move();
        }
        //else if (canMove == false)
        //{
        //    transform.position = Vector3.zero;
        //}
    }

    private void Move()
    {

        if (directionOfMovment == Gun.GunShootingDirection.Top)
        {
            transform.position += new Vector3(0f, speed * Time.deltaTime);
        }
        else if (directionOfMovment == Gun.GunShootingDirection.Bottom)
        {
            transform.position += new Vector3(0f, -(speed * Time.deltaTime));
        }
        else if (directionOfMovment == Gun.GunShootingDirection.Right)
        {
            transform.position += new Vector3(speed * Time.deltaTime, 0f);
        }
        else if (directionOfMovment == Gun.GunShootingDirection.Left)
        {
            transform.position += new Vector3(-(speed * Time.deltaTime), 0f);
        }
    }

    public void ResetBullet()
    {
        canMove = false;
        transform.parent = originalParent;
        transform.localPosition = Vector3.zero;
        gameObject.SetActive(false);
    }

    public void SetParentTransform(Transform originalParentTransform)
    {
        originalParent = originalParentTransform;
    }

    public void SetCanMove(bool v)
    {
        canMove = v;
    }

    public void SeTShootDirection(Gun.GunShootingDirection directionOfShooting)
    {
        directionOfMovment = directionOfShooting;
    }
}
