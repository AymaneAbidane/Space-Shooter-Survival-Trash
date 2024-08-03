using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBullets : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.gameObject.TryGetComponent<Bullet>(out Bullet bullet))
        {
            bullet.ResetBullet();
            Debug.Log("Bullet Out Of Scene");
        }
    }
}
