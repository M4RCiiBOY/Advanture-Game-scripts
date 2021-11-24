using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIBody))]
[RequireComponent(typeof(Orbit))]
[RequireComponent(typeof(MoveAttack))]
[RequireComponent(typeof(Wander))]
[RequireComponent(typeof(Flee))]

public class Wolf : MonoBehaviour {

    public float MaxHealth;
    public float AggroRange;
    public float MeleeRange;
    public GameObject MeleeHit;
    public float TimeInOrbit;
    public float MeleeCooldown;
    public int Damage;

    private float orbitTimer;
    private float cooldownTimer;

    public Vector3 currentTarget;
    AIBody body;
    IMovement orbit;
    MoveAttack moveAttack;
    IMovement wander;
    IMovement flee;

    private enum AIState
    {
        wander, orbit, attack, flee
    }
    private AIState state;


    private void Awake()
    {
        wander = gameObject.GetComponent<Wander>();
        flee = gameObject.GetComponent<Flee>();
        orbit = gameObject.GetComponent<Orbit>();
        moveAttack = gameObject.GetComponent<MoveAttack>();
        body = gameObject.GetComponent<AIBody>();
        moveAttack.MeleeRange = MeleeRange;
        MaxHealth = body.MaxHealth;
        cooldownTimer = MeleeCooldown;
    }

    private void Start()
    {
        changeState(AIState.wander);
    }

    private void Update()
    {
        if (body.BeingHooked) return;
        //find closest player
        switch (state)
        {
            case AIState.wander:
                currentTarget = body.ClosestPlayer;
                if (Vector3.Distance(currentTarget, transform.position) < AggroRange)
                {
                    changeState(AIState.orbit);
                    return;
                }
                break;
            case AIState.orbit:
                currentTarget = body.ClosestPlayer;
                if (orbitTimer <= 0)
                {
                    changeState(AIState.attack);
                }
                else if (Vector2.Distance(transform.position, currentTarget) <= 1.5f)
                {
                    orbitTimer -= Time.deltaTime;
                }
                break;
            case AIState.attack:
                if (cooldownTimer <= 0.9f * MeleeCooldown || cooldownTimer <= MeleeCooldown - 0.3f)
                {
                    currentTarget = body.ClosestPlayer;
                    moveAttack.Target = currentTarget;
                }
                if (cooldownTimer >= MeleeCooldown)
                {
                    GameObject Hit = Instantiate(MeleeHit, 0.5f * (currentTarget - transform.position).normalized * MeleeRange + transform.position, Quaternion.identity);
                    Hit.GetComponent<AIMeleeHit>().Radius = 0.5f * MeleeRange;
                    Hit.GetComponent<AIMeleeHit>().Damage = Damage;
                    Hit.GetComponent<AIMeleeHit>().Players = body.Players;
                    Hit.GetComponent<AIMeleeHit>().Swipe = true;
                    cooldownTimer = 0;
                }
                else cooldownTimer += Time.deltaTime;
                break;
            case AIState.flee: return;
            default:
                break;
        }
        if (body.Health <= 0.2f * MaxHealth)
        {
            changeState(AIState.flee);
            return;
        }
    }

    /// <summary>
    /// Setup for the various states
    /// </summary>
    /// <param name="newState"></param>
    private void changeState(AIState newState)
    {
        switch (newState)
        {
            case AIState.wander:
                state = AIState.wander;
                body.Move = wander;
                break;
            case AIState.orbit:
                state = AIState.orbit;
                body.Move = orbit;
                orbitTimer = TimeInOrbit;
                break;
            case AIState.attack:
                cooldownTimer = 0.75f * MeleeCooldown;
                body.Move = moveAttack;
                state = AIState.attack;
                break;
            case AIState.flee:
                state = AIState.flee;
                body.Move = flee;
                break;
            default:
                break;
        }
    }
}
