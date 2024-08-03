using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Gun : MonoBehaviour
{
    public enum GunShootingDirection { Top, Bottom, Left, Right }

    [SerializeField] private GunShootingDirection directionOfShooting;
    [SerializeField, AssetsOnly] private Bullet bulletprefab;
    [SerializeField] private int numberofbulletsInPool;
    [SerializeField] private float fireRate;
    [SerializeField] private bool gunIsShooting;//just for visual purpos in the inspector
    [SerializeField] private bool isBurst;
    [SerializeField, ShowIf("isBurst", true)] private int bulletburstCount;
    [SerializeField, ShowIf("isBurst", true)] private int delayBetweenShootsinBurst;


    private List<Bullet> bulletsPoolList = new();
    private int bulletsPoolIndex;


    public void AddToPool()
    {
        UpdateBulletsPool();
    }

    public void StartShooting()
    {
        if (gunIsShooting == true) { return; }
        gunIsShooting = true;
        StartCoroutine(COR_Shoot());
    }

    private IEnumerator COR_Shoot()
    {
        while (true)
        {
            if (isBurst == true)
            {

            }
            else
            {
                Bullet bullet = bulletsPoolList[bulletsPoolIndex];
                bullet.gameObject.SetActive(true);
                bullet.SetCanMove(true);
                bullet.gameObject.transform.parent = null;
                if (bulletsPoolIndex == bulletsPoolList.Count - 1)
                {
                    bulletsPoolIndex = 0;
                }
                else
                {
                    bulletsPoolIndex++;
                }
            }
            yield return new WaitForSeconds(fireRate);
        }
    }

    private void UpdateBulletsPool(bool isReupdated = false)
    {

        if (isReupdated == true)
        {
            RemoveFromBulletsPool();
            Utils.Delay(this, 0.1f, () =>
            {
                AddToBulletsPool();
            });
        }
        else
        {
            AddToBulletsPool();
        }
    }

    private void RemoveFromBulletsPool()
    {
        foreach (Bullet b in bulletsPoolList)
        {
            Destroy(b.gameObject);
        }
        bulletsPoolList.Clear();
    }

    private void AddToBulletsPool()
    {
        bulletsPoolIndex = 0;
        for (int i = 0; i < numberofbulletsInPool; i++)
        {
            var copy = Instantiate(bulletprefab, Vector3.zero, Quaternion.identity);
            copy.SetParentTransform(transform);
            copy.transform.parent = transform;
            copy.gameObject.SetActive(false);
            copy.transform.localPosition = Vector3.zero;
            copy.SeTShootDirection(directionOfShooting);
            bulletsPoolList.Add(copy);
        }
    }

    public void ResetGun()
    {
        UpdateBulletsPool(true);
        StopShooting();
    }

    private void StopShooting()
    {
        StopAllCoroutines();
        gunIsShooting = false;
    }
}
