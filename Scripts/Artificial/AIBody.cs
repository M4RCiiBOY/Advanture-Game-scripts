using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBody : MonoBehaviour
{
    public float MaxSpeed;
    public float MaxAcceleration;
    public float MaxHealth;
    public Vector3 Velocity
    {
        get
        {
            return velocity;
        }
    }
    public IMovement Move
    {
        get
        {
            return move;
        }
        set
        {
            move = value;
        }
    }
    public float Health
    {
        get
        {
            return health;
        }
    }
    public GameObject[] Players
    {
        get
        {
            return players;
        }
    }

    public bool BeingHooked
    {
        get
        {
            return beingHooked;
        }
    }
    private bool beingHooked;
    public bool Casting;

    GameObject[] players;
    public Vector3 ClosestPlayer
    {
        get
        {
            return closestPlayer;
        }
    }
    private Vector3 closestPlayer;
    private Vector3 velocity;
    private Vector3 startPos;
    private IMovement move;
    float health;
    IMovement wander;
    IMovement flee;
    IMovement bossWander;

    private Animator anim;


    //Whisker stuff
    private int collisionCounter;
    [Tooltip("Amount of change when whiskers hit.")]
    public float angleChange;
    private bool skipLeft;
    private float leftTimer;
    private Vector3 rayAvoidance;
    RaycastHit2D hitRight;
    RaycastHit2D hitLeft;
    float angle;
    float minWhiskerMagn;

    private void Awake()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        wander = gameObject.GetComponent<Wander>();
        flee = gameObject.GetComponent<Flee>();
        if (gameObject.GetComponent<DungeonBoss>() != null) bossWander = gameObject.GetComponent<BossWander>();
        health = MaxHealth;
        anim = GetComponent<Animator>();
        collisionCounter = 1;
        startPos = transform.position;
        beingHooked = false;
        Casting = false;
        minWhiskerMagn = gameObject.GetComponent<CircleCollider2D>().radius;
    }

    private void Update()
    {
        if (BeingHooked || Casting) return;
        ClosetPlayer();
        if (move == flee)
        {
            if (Vector3.Distance(startPos, transform.position) > 25) Destroy(gameObject);
        }
        if (move == flee || move == wander) Whisk();
        else if (gameObject.GetComponent<DungeonBoss>() != null)
        {
            if (move == bossWander) Whisk();
        }
        velocity += (Move.Accelerate() / collisionCounter + rayAvoidance) * Time.deltaTime;
        if (velocity.magnitude >= MaxSpeed) velocity = velocity.normalized * MaxSpeed;
        transform.position += velocity * Time.deltaTime;
        Direction();
    }

    private void Direction()
    {
        if (velocity == Vector3.zero) return;
        if (velocity.x > velocity.y)
        {
            if (velocity.x>0)
            {
                StopAnimations();
                anim.SetBool("RightWalk", true);

                //RECHTS
            }
            else
            {
                StopAnimations();
                anim.SetBool("LeftWalk", true);
                //LINKS
            }
        }
        else
        {
            if (velocity.y>0)
            {
                StopAnimations();
                anim.SetBool("UpWalk", true);
                //OBEN
            }
            else
            {
                StopAnimations();
                anim.SetBool("DownWalk", true);
                //UNTEN
            }
        }
    }

    private void StopAnimations()
    {
        anim.SetBool("DownWalk", false);
        anim.SetBool("LeftWalk", false);
        anim.SetBool("RightWalk", false);
        anim.SetBool("UpWalk", false);
    }

    /// <summary>
    /// Damage an AI by an amount.
    /// </summary>
    /// <param name="amount"></param>
    public void Damage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Dying();
        }
    }

    /// <summary>
    /// Heal an AI by amount up to its maximum Health.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(int amount)
    {
        health += health;
        if (health > MaxHealth) health = MaxHealth;
    }

    /// <summary>
    /// Heal an AI by amount and potentially overhealing.
    /// </summary>
    /// <param name="amount"></param>
    public void Overheal(int amount)
    {
        health += health;
    }

    /// <summary>
    /// Stops Agent, but keeps movement going.
    /// </summary>
    public void StopMove()
    {
        velocity = Vector3.zero;
    }

    /// <summary>
    /// Call if Agent should stop moving until being released with OffTheHook().
    /// </summary>
    public void OnTheHook()
    {
        beingHooked = true;
        StopMove();
    }

    /// <summary>
    /// Releases agent and let's them move again.
    /// </summary>
    public void OffTheHook()
    {
        beingHooked = false;
    }

    /// <summary>
    /// Call if player array needs to be changed.
    /// </summary>
    public void GetPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }

    public void Whisk()
    {
        angle = Mathf.Clamp(angleChange * collisionCounter, 10, 90);
        hitLeft  = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, +angle) * velocity, Mathf.Clamp(velocity.magnitude * (90 - angle) / 90, minWhiskerMagn, velocity.magnitude));
        hitRight = Physics2D.Raycast(transform.position, Quaternion.Euler(0, 0, -angle) * velocity, Mathf.Clamp(velocity.magnitude * (90 - angle) / 90, minWhiskerMagn, velocity.magnitude));
        if (hitRight.collider != null)
        {
            if (hitLeft.collider != null)
            {
                skipLeft = true;
                leftTimer = 1;
            }
            if (hitRight.collider.tag != "Player")
            {
                collisionCounter++;
                rayAvoidance = Quaternion.Euler(0, 0, angle + 90) * velocity * 2;
            }
        }
        else
        {
            LeftWhisker();
        }
        collisionCounter = Mathf.Clamp(collisionCounter, 1, 60);
    }

    private void LeftWhisker()
    {
        if (skipLeft)
        {
            leftTimer -= Time.deltaTime;
            if (leftTimer <= 0)
            {
                skipLeft = false;
            }
        }
        else if (hitLeft.collider != null)
        {
            if (hitLeft.collider.tag != "Player")
            {
                collisionCounter++;
                rayAvoidance = Quaternion.Euler(0, 0, -(angle + 90)) * velocity * 2;
            }
            return;
        }
        collisionCounter--;
        rayAvoidance = Vector3.zero;
    }

    private void ClosetPlayer()
    {
        if (players.Length == 0)
        {
            players = GameObject.FindGameObjectsWithTag("Player");
            if (players.Length == 0) Dying();
        }
        GameObject closestPlayer = players[0];
        for (int i = 0; i < players.Length; i++)
        {
            if (Vector3.Distance(players[i].transform.position, transform.position) < Vector3.Distance(closestPlayer.transform.position, transform.position))
            {
                closestPlayer = players[i];
            }
        }
        this.closestPlayer = closestPlayer.transform.position;
    }

    private void Dying()
    {
        OnTheHook();
        anim.SetTrigger("Death");
        Destroy(this.gameObject,.5f);
    }

    private void OnDrawGizmos()
    {
        
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, +angle) * velocity.normalized * (Mathf.Clamp(velocity.magnitude * (90 - angle) / 90, minWhiskerMagn, velocity.magnitude)));
        Gizmos.DrawLine(transform.position, transform.position + Quaternion.Euler(0, 0, -angle) * velocity.normalized * (Mathf.Clamp(velocity.magnitude * (90 - angle) / 90, minWhiskerMagn, velocity.magnitude)));
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + velocity);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + rayAvoidance);
    }
}