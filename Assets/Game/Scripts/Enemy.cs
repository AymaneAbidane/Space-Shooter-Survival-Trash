using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Ship
{
    [SerializeField] private Path ownPath;//remove serialize later
    [SerializeField] private float collisionDamageToPlayerShip;
    [SerializeField, ChildGameObjectsOnly] private SpriteRenderer shipRenderer;
    [SerializeField, ChildGameObjectsOnly] private ShipAnimationsController animator;
    [SerializeField] private AnimationsEventsController eventController;
    [SerializeField] private float shipExperienceValue;

    [Header("Hit Animation Setting")]
    [SerializeField] private int animationLoopFrams;
    [SerializeField] private float delayBetweenFrams;


    public event EventHandler<float> onEnemyShipDestroyed;

    private Vector3 instantiationPosition;
    private Coroutine movmentCor;

    private void OnEnable()
    {
        eventController.onEnemyDeathAnimationFinished += EventController_onEnemyDeathAnimationFinished;
    }

    private void OnDestroy()
    {
        eventController.onEnemyDeathAnimationFinished -= EventController_onEnemyDeathAnimationFinished;
    }

    private void EventController_onEnemyDeathAnimationFinished(object sender, EventArgs e)
    {
        DestroyShip();
        enemyShipCollider.enabled = true;
    }

    protected override void PlayGettingHitAnimation()
    {
        StartCoroutine(COR_PlayHitAnimation());

        IEnumerator COR_PlayHitAnimation()
        {
            for (int i = 0; i < animationLoopFrams; i++)
            {
                if (i % 2 != 0)
                {
                    SetShipRenderer(false);
                }
                else
                {
                    SetShipRenderer(true);
                }

                yield return new WaitForSeconds(delayBetweenFrams);
            }
            StopCoroutine(COR_PlayHitAnimation());
        }
    }
    private void SetShipRenderer(bool v)
    {
        shipRenderer.enabled = v;
    }


    protected override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (maxhealth <= 0f)
        {
            onEnemyShipDestroyed?.Invoke(this, shipExperienceValue);
            ResetAndBackToPool();
        }
    }

    public void SetInstantiationPosition(Vector3 pos)
    {
        instantiationPosition = pos;
    }

    public float GetCollisionDamage()
    {
        return collisionDamageToPlayerShip;
    }

    protected override void Move()
    {
        movmentCor = StartCoroutine(COR_FollowPath());
    }
    public void StartMoving()
    {
        Move();
    }

    public void SetPath(Path path)
    {
        ownPath = path;
    }

    private IEnumerator COR_FollowPath()
    {
        int pathPointIndex = 1;

        Transform[] pathPoints = ownPath.GetPathPoints();

        SetShipDirection();

        transform.position = pathPoints[0].position;

        while (true)
        {
            if (pathPointIndex < pathPoints.Length)
            {
                Vector3 targetPosition = pathPoints[pathPointIndex].transform.position;
                transform.position = Vector2.MoveTowards(transform.position, targetPosition, (moveSpeed * Time.deltaTime));

                if (transform.position == targetPosition)
                {
                    pathPointIndex++;
                }
            }
            else
            {
                StopCoroutine(movmentCor);
            }

            yield return null;
        }
    }

    private void SetShipDirection()
    {
        if (ownPath.redirectedPath == false && ownPath.axisType == Path.PathAxisType.Vertical)
        {
            SetShipAngle(0f);
        }
        else if (ownPath.redirectedPath == true && ownPath.axisType == Path.PathAxisType.Vertical)
        {
            SetShipAngle(180f);
        }
        else if (ownPath.redirectedPath == false && ownPath.axisType == Path.PathAxisType.Horizontal)
        {
            SetShipAngle(-90f);
        }
        else if (ownPath.redirectedPath == true && ownPath.axisType == Path.PathAxisType.Horizontal)
        {
            SetShipAngle(90f);
        }
    }

    private void SetShipAngle(float degree)
    {
        transform.eulerAngles = Vector3.forward * degree;
    }

    public void ResetAndBackToPool()
    {
        Debug.Log("Collided with player ship will be destroyed");
        StopCoroutine(COR_FollowPath());
        enemyShipCollider.enabled = false;
        animator.PlayDeathAnimation();
        maxhealth = 50f;
        //continuie from here
    }

    protected override void DestroyShip()
    {
        //play destruction animation
        //SetrendeReEnabling(false);
        transform.position = instantiationPosition;
        animator.PlayIdlAnimation();

    }
}
