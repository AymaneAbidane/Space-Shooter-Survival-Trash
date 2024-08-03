using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Ship
{
    [SerializeField, ChildGameObjectsOnly] private ShipAnimationsController animations;
    [SerializeField, ChildGameObjectsOnly] private AnimationsEventsController eventsController;


    [SerializeField] private GameObject fireEngineGo;
    [SerializeField] private InputActionReference moveDirectionInputs;
    [SerializeField] private Rigidbody2D playerRb;

    [SerializeField] private Vector2 clampingVector;

    private Vector2 moveDirection;

    private void OnEnable()
    {
        eventsController.onPlayerDeathAnimationFinished += EventsController_onPlayerDeathAnimationFinished;
        Debug.Log(gameObject.name + " is enabeld");
        SetFireEngineGoActivation(true);
        playerGunsManager.AddBulletsToAllGuns();

        Utils.Delay(this, 0.5f, () =>
        {
            playerGunsManager.StartShooting();
        });
    }

    private void OnDisable()
    {
        eventsController.onPlayerDeathAnimationFinished -= EventsController_onPlayerDeathAnimationFinished;
        Debug.Log(gameObject.name + " is disabeld");
    }

    private void EventsController_onPlayerDeathAnimationFinished(object sender, EventArgs e)
    {
        // ResetPlayerShip();
    }

    protected override void InvokHealthEvent(float health)
    {
        base.InvokHealthEvent(health);
        if (health <= 0f)
        {
            ResetPlayerShip();
        }
    }

    private void Update()
    {
        moveDirection = moveDirectionInputs.action.ReadValue<Vector2>();
        ClampPlayerPosition();
    }

    private void ClampPlayerPosition()
    {
        float xClampingValue = clampingVector.x;
        float yClampingValue = clampingVector.y;

        float finaleClampingX = Mathf.Clamp(transform.position.x, -xClampingValue, xClampingValue);
        float finaleClampingY = Mathf.Clamp(transform.position.y, -yClampingValue, yClampingValue);

        transform.position = new Vector2(finaleClampingX, finaleClampingY);
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected override void Move()
    {
        float x = moveDirection.x;
        float y = moveDirection.y;
        playerRb.velocity = Time.fixedDeltaTime * moveSpeed * new Vector2(x, y);
    }

    protected override void DestroyShip()
    {
        Debug.Log("Player Ship Destroyed");
        SetFireEngineGoActivation(false);
        animations.PlayDeathAnimation();
    }


    [Button]
    private void TESTFUNC_TakeDamage(float damage)
    {
        TakeDamage(damage);
    }

    public float GetHealthValue()
    {
        return maxhealth;
    }

    private void ResetPlayerShip()
    {
        maxhealth = 150f;//this not the end for this methode we add other stuff to it
        transform.position = Vector3.zero;
        playerGunsManager.ResetPlayerGuns();
    }

    private void SetFireEngineGoActivation(bool v)
    {
        fireEngineGo.SetActive(v);
    }
}
