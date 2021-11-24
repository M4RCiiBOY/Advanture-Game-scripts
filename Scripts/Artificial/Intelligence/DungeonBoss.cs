using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIBody))]
[RequireComponent(typeof(MoveAttack))]
[RequireComponent(typeof(BossWander))]
public class DungeonBoss : MonoBehaviour
{
    //Boss Arena:
    //  - Left/Right Side has large holes that players can fall into (large damage but not deadly)
    //  - Pillars to hide needed?
    //DungeonBoss AI:
    //  - Tries to stay somewhat away from the players
    //  - regularly throws dynamite in players predicted movement
    //  - can run to a side the players don't occupy and summon a wind tyring to shove players into the other hole
    //      - Hook can stop casting
    //      - Shield blocks the wind
    //  - summons a sphere that will target hook-player
    //      - Shield player can prevent damage by blocking
    //  - At 2/5 health web players and summon multiple bandits

    //Needs:
    //  - Left/Right spellpositions
    //  - both players gadget info and (optionally) player velocity
    //Prefabs needed:
    //  - Bandit
    //  - Sphere spell
    //  - Web spell

    [Tooltip("Time between dynamite throws")]
    public float DynamiteThrowCooldown;
    private float dynamiteCooldown;

    [Tooltip("Time between spells")]
    public float SpellCooldown;
    private float spellCooldownTimer;
    [Tooltip("Time needed to cast spells")]
    public float CastTime;
    private float timeCasting;
    private float windTimer;

    public GameObject Dynamite;
    private GameObject dynamiteAttack;
    public GameObject SphereSpell;
    private GameObject sphereAttack;
    public GameObject SpellPositionLeft;
    public GameObject WindZoneLeft;
    public GameObject SpellPositionRight;
    public GameObject WindZoneRight;
    public GameObject WebSpell;
    public GameObject Bandit;

    private GameObject[] players;
    private GameObject hookPlayer;

    private bool castedWindLast;
    private bool windOff;
    private bool banditsNotSummoned;

    private AIBody body;
    private MoveAttack moveAttack;
    private BossWander wander;

    private enum AIState
    {
        basic, wind, sphere, summon
    }
    private AIState state;


    void Awake()
    {
        body = gameObject.GetComponent<AIBody>();
        moveAttack = gameObject.GetComponent<MoveAttack>();
        wander = gameObject.GetComponent<BossWander>();

        changeMove(wander);
        changeState(AIState.basic);
        banditsNotSummoned = true;
        windTimer = 0;
        windOff = true;
    }

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        hookPlayer = players[0]; //fallback if no player has hook
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<PlayerCtrl>().gadgetType != PlayerCtrl.GadgetType.Shield)
            {
                hookPlayer = players[i];
                break;
            }
        }
        body.GetPlayers();
        wander.GetPlayers();
    }

    void Update()
    {
        if (body.BeingHooked)
        {
            if (state != AIState.summon)
            {
                body.Casting = false;
                changeState(AIState.basic);
                //Disable Windzones
                WindZoneLeft.SetActive(false);
                WindZoneRight.SetActive(false);
                windOff = true;
                return;
            }
        }
        switch (state)
        {
            case AIState.basic:
                dynamiteCooldown += Time.deltaTime;
                if(dynamiteCooldown >= DynamiteThrowCooldown)
                {
                    dynamiteCooldown = 0;
                    //throw Dynamite
                    dynamiteAttack = Instantiate(Dynamite, transform.position, Quaternion.identity);
                    dynamiteAttack.GetComponent<Dynamite>().Target = players[Random.Range(0, players.Length)].transform.position;
                }

                spellCooldownTimer += Time.deltaTime;
                if (spellCooldownTimer >= SpellCooldown)
                {
                    spellCooldownTimer = 0;
                    if (castedWindLast) changeState(AIState.sphere);
                    else if (Random.Range(0, 3) == 0) changeState(AIState.wind);
                    else changeState(AIState.sphere);
                }
                break;

            case AIState.wind:
                if (!body.Casting)
                {
                    if (Vector3.Distance(transform.position, SpellPositionLeft.transform.position) <= 0.2f ||
                        Vector3.Distance(transform.position, SpellPositionRight.transform.position) <= 0.2f)
                    {
                        body.Casting = true;
                    }
                }
                else
                {
                    if (windOff)
                    {
                        timeCasting += Time.deltaTime;
                        if (timeCasting >= CastTime)
                        {
                            //Enable correct Windzone
                            if (Vector3.Distance(transform.position, SpellPositionLeft.transform.position) <= 0.2f)
                            {
                                WindZoneLeft.SetActive(true);
                            }
                            else WindZoneRight.SetActive(true);
                            windOff = false;
                        }
                    }
                    else
                    {
                        windTimer += Time.deltaTime;
                        if (windTimer >= 1.5f)
                        {
                            windTimer = 0;
                            //Disable Windzone to combat TriggerStay2D being an utter mess
                            if (Vector3.Distance(transform.position, SpellPositionLeft.transform.position) <= 0.2f)
                            {
                                WindZoneLeft.SetActive(false);
                            }
                            else WindZoneRight.SetActive(false);
                            windOff = true;
                        }
                    }
                }
                break;

            case AIState.sphere:
                timeCasting += Time.deltaTime;
                if (timeCasting >= CastTime)
                {
                    //Cast Sphere with target as hookPlayer
                    sphereAttack = Instantiate(SphereSpell, transform.position, Quaternion.identity);
                    sphereAttack.GetComponent<SphereSpell>().Target = hookPlayer;
                    changeState(AIState.basic);
                }
                break;

            case AIState.summon:
                if (!body.Casting)
                {
                    if (Vector3.Distance(transform.position, SpellPositionLeft.transform.position) <= 0.2f ||
                        Vector3.Distance(transform.position, SpellPositionRight.transform.position) <= 0.2f)
                    {
                        body.Casting = true;
                    }
                }
                else
                {
                    timeCasting += Time.deltaTime;
                    if (timeCasting >= CastTime)
                    {
                        //Summon Bandits
                        float radius = 0.4f;
                        for (int i = 0; i < 3; i++)
                        {
                            float angle = Random.Range(0, 360) * Mathf.PI / 180;
                            GameObject bandit = Instantiate(Bandit,
                                transform.position + new Vector3(radius * Mathf.Cos(angle),
                                                                 radius * Mathf.Sin(angle)),
                                                                 Quaternion.identity);
                            bandit.GetComponent<Bandit>().MaxArrows = 0;
                            bandit.GetComponent<Bandit>().AggroRange = 100;
                        }
                        changeState(AIState.basic);
                    }
                }
                break;
            default:
                break;
        }
        if (banditsNotSummoned)
        {
            if (body.Health <= 0.4f * body.MaxHealth)
            {
                changeState(AIState.summon);
                banditsNotSummoned = false;
            }
        }
    }

    /// <summary>
    /// De-/Accelerates Boss if moving out/in moveAttack. Otherwise changes movement.
    /// </summary>
    /// <param name="newMove"></param>
    private void changeMove(IMovement newMove)
    {
        if (body.Move != (IMovement)moveAttack)
        {
            if (newMove == (IMovement)moveAttack)
            {
                body.MaxAcceleration *= 10 * body.MaxAcceleration;
                body.MaxSpeed *= 5 * body.MaxSpeed;
            }
        }
        else if(newMove != (IMovement)moveAttack)
        {
            body.MaxAcceleration /= 10 * body.MaxAcceleration;
            body.MaxSpeed /= 5 * body.MaxSpeed;
        }
        body.Move = newMove;
    }

    /// <summary>
    /// Setup for the various states
    /// </summary>
    /// <param name="newState"></param>
    private void changeState(AIState newState)
    {
        switch (newState)
        {
            case AIState.basic:
                state = AIState.basic;
                changeMove(wander);
                body.Casting = false;
                dynamiteCooldown = 0;
                break;

            case AIState.wind:
                state = AIState.wind;
                castedWindLast = true;
                body.StopMove();
                timeCasting = 0;
                changeMove(moveAttack);
                if (Vector3.Distance(hookPlayer.transform.position, SpellPositionLeft.transform.position) <
                    Vector3.Distance(hookPlayer.transform.position, SpellPositionRight.transform.position))
                {
                    moveAttack.Target = SpellPositionLeft.transform.position;
                }
                else moveAttack.Target = SpellPositionRight.transform.position;
                break;

            case AIState.sphere:
                state = AIState.sphere;
                castedWindLast = false;
                body.Casting = true;
                timeCasting = 0;
                break;

            case AIState.summon:
                state = AIState.summon;
                //Unsummon Dynamite and Spheres, Stop Wind
                Destroy(sphereAttack);
                Destroy(dynamiteAttack);
                WindZoneRight.SetActive(false);
                WindZoneLeft.SetActive(false);
                windOff = true;
                //Cast Web spell
                for (int i = 0; i < players.Length; i++)
                {
                    GameObject web = Instantiate(WebSpell, transform.position, Quaternion.identity);
                    web.GetComponent<WebSpell>().Target = players[i];
                }
                timeCasting = 0;
                castedWindLast = false;
                changeMove(moveAttack);
                if (Vector3.Distance(transform.position, SpellPositionLeft.transform.position) <
                    Vector3.Distance(transform.position, SpellPositionRight.transform.position))
                {
                    moveAttack.Target = SpellPositionLeft.transform.position;
                }
                else moveAttack.Target = SpellPositionRight.transform.position;
                break;

            default:
                break;
        }
    }
}
