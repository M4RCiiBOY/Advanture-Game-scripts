using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIBody))]
[RequireComponent(typeof(MoveAttack))]
[RequireComponent(typeof(Wander))]
[RequireComponent(typeof(Flee))]

public class Bandit : MonoBehaviour
{

    private float MaxHealth;
    public float AggroRange;
    public float MeleeRange;
    public GameObject MeleeAttack;
    public float MeleeCooldown;
    private float cooldownTimer;
    public float RangedRange;
    public GameObject RangeAttack;
    public int MaxArrows;
    private int arrows;
    public float RangeAttackCooldown;
    private float rangeCooldownTimer;
    public int Damage;

    Vector3 currentTarget;
    AIBody body;
    MoveAttack moveAttack;
    Wander wander;
    Flee flee;

    private enum AIState
    {
        wander, range, melee, fleeing
    }
    private AIState state;

    private void Awake()
    {
        wander = gameObject.GetComponent<Wander>();
        flee = gameObject.GetComponent<Flee>();
        moveAttack = gameObject.GetComponent<MoveAttack>();
        body = gameObject.GetComponent<AIBody>();
        moveAttack.MeleeRange = MeleeRange;
        MaxHealth = body.MaxHealth;
    }

    private void Start()
    {
        body.Move = wander;
        state = AIState.wander;
        cooldownTimer = 0;
        rangeCooldownTimer = 0;
        arrows = MaxArrows;
    }

    private void Update()
    {
        if (body.BeingHooked) return;
        switch (state)
        {
            case AIState.wander: currentTarget = body.ClosestPlayer;
                if (Vector3.Distance(currentTarget, transform.position) < AggroRange)
                {
                    state = AIState.range;
                    body.Move = moveAttack;
                }
                break;
            case AIState.range://TOMAYBEDO: Make new movement?
                if (arrows <= 0)
                {
                    state = AIState.melee;
                    break;
                }
                else if (body.Velocity.magnitude == 0)
                {
                    rangeCooldownTimer += Time.deltaTime;
                    if (rangeCooldownTimer <= 0.9f * RangeAttackCooldown || rangeCooldownTimer <= RangeAttackCooldown - 0.3f)
                    {
                        currentTarget = body.ClosestPlayer;
                        moveAttack.Target = currentTarget + ((transform.position - currentTarget).normalized * (RangedRange - MeleeRange));
                    }
                    if (rangeCooldownTimer >= RangeAttackCooldown)
                    {
                        rangeCooldownTimer = 0;
                        Vector2 fireSolution = currentTarget - transform.position;
                        GameObject NormalArrow = Instantiate(this.RangeAttack, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(fireSolution.x, -fireSolution.y) * Mathf.Rad2Deg)));
                        NormalArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                        arrows -= 1;
                    }
                }
                else
                {
                    currentTarget = body.ClosestPlayer;
                    moveAttack.Target = currentTarget + ((transform.position - currentTarget).normalized * (RangedRange - MeleeRange));
                }
                break;
            case AIState.melee:
                if (Vector3.Distance(currentTarget, transform.position) < MeleeRange)
                {
                    cooldownTimer += Time.deltaTime;
                    if (cooldownTimer <= 0.9f * MeleeCooldown || cooldownTimer <= MeleeCooldown - 0.3f)
                    {
                        currentTarget = body.ClosestPlayer;
                        moveAttack.Target = currentTarget;
                    }
                    if (cooldownTimer >= MeleeCooldown)
                    {
                        GameObject Hit = Instantiate(MeleeAttack, 0.5f * MeleeRange * (currentTarget - transform.position).normalized + transform.position, Quaternion.identity);
                        Hit.GetComponent<AIMeleeHit>().Radius = 0.5f * MeleeRange;
                        Hit.GetComponent<AIMeleeHit>().Damage = Damage;
                        Hit.GetComponent<AIMeleeHit>().Players = body.Players;
                        cooldownTimer = 0;
                    }
                }
                else
                {
                    currentTarget = body.ClosestPlayer;
                    moveAttack.Target = currentTarget;
                }
                break;
            case AIState.fleeing: return;
            default:
                break;
        }
        if (body.Health <= 0.2f * MaxHealth)
        {
            body.Move = flee;
            state = AIState.fleeing;
            return;
        }
    }
}
