using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    public int playernumber;
    public float speedMultiplicator = 1;
    public float dodgeMultiplicator = 2;
    public GameObject Arrow;
    public GameObject Hook;
    public GameObject RemoteBomb;
    public GameObject Fan;
    public GameObject LeftStrike;
    public GameObject RightStrike;
    public GameObject UpStrike;
    public GameObject DownStrike;
    public GameObject Shield;

    public GameObject LeftShield;
    public GameObject RightShield;
    public GameObject UpShield;
    public GameObject DownShield;
    public GameObject Parent;

    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    public enum Runes
    {
        None,
        Fire,
        Water,
        Thunder,
        Steam, //Fire Water
        Explosion, //Fire Thunder
        StunLighting //Thunder Water
    }

    public enum WeaponType
    {
        Sword,
        Bow
    }

    public enum GadgetType
    {
        Shield,
        Hook,
        RemoteBomb,
        Fan
    }



    public GadgetType gadgetType;
    public WeaponType weaponType = WeaponType.Sword;

    private Runes Rune1P1 = Runes.None;
    private Runes Rune2P1 = Runes.None;
    private Runes RuneInteractionP1 = Runes.None;
    private Runes Rune1P2 = Runes.None;
    private Runes Rune2P2 = Runes.None;
    private Runes RuneInteractionP2 = Runes.None;
    private IRune script;
    private int randomNumber = 0;
    private bool dodging = false;
    private bool dodgeCooldown = false;
    private bool newActionsAllowed = true;
    private InterfaceScript interfaceScript;
    private Health hpScript;
    private Direction dir;
    private Rigidbody2D rg;
    private Animator anim;
    private Vector2 lastMoveVec;
    private DistanceJoint2D joint;

    int hp = 8;
    int hpOld;


    public string[] controllers;
    void Start()
    {
        transform.parent = null;
        rg = GetComponent<Rigidbody2D>();
        try
        {
            interfaceScript = GameObject.Find("InterfaceHandlerGO").GetComponent<InterfaceScript>();
            hpScript = GameObject.Find("HealthPlayer" + playernumber).GetComponent<Health>();
        }
        catch (Exception)
        {
        }
        anim = GetComponent<Animator>();
        joint = GetComponent<DistanceJoint2D>();
        joint.connectedBody = Camera.main.GetComponent<Rigidbody2D>();
        dir = Direction.Down;
        hpOld = hp;
        controllers = new string[10];
    }

    void Update()
    {
        //for testing
        if (weaponType == WeaponType.Sword)
        {
            anim.SetInteger("Weapon", 0);
        }
        if (weaponType == WeaponType.Bow)
        {
            anim.SetInteger("Weapon", 1);
        }
        if (gadgetType == GadgetType.Shield)
        {
            anim.SetInteger("Gadget", 0);
        }
        if (gadgetType == GadgetType.Hook)
        {
            anim.SetInteger("Gadget", 1);
        }

        PlayerView();
        ManageHealth();

        if (interfaceScript.menuOpen)
        {
            StopAnimations();
            return;
        }

        if (newActionsAllowed)
        {
            Move();
            Actions();
        }
        else
        {
            CheckForShield();
            StopAnimations();
        }

    }

    private void CheckForShield()
    {
        if (Input.GetButtonUp("P" + playernumber + "_Gadget") || !Input.GetKey(KeyCode.G))
        {
            newActionsAllowed = true;
            anim.SetBool("Shield", false);
            UpShield.SetActive(false);
            DownShield.SetActive(false);
            LeftShield.SetActive(false);
            RightShield.SetActive(false);
        }
    }

    private void Actions()
    {
        if (Input.GetButtonDown("P" + playernumber + "_Dodge") && !dodgeCooldown) //A on an XBOX Controller
        {
            anim.SetTrigger("Dodge");
            dodging = true;
            dodgeCooldown = true;
            StartCoroutine(Dodge());
        }

        if (Input.GetButtonDown("P" + playernumber + "_Attack")) //B on an XBOX Controller
        {
            anim.SetTrigger("Attack");
            if (weaponType == WeaponType.Bow)
                Shoot();
            if (weaponType == WeaponType.Sword)
                Strike();
            newActionsAllowed = false;
            StartCoroutine(Attack());
        }

        if (Input.GetButtonDown("P" + playernumber + "_Gadget") || Input.GetKeyDown(KeyCode.G)) //Y on an XBOX Controller
        {
            switch (gadgetType)
            {
                case GadgetType.Shield:
                    Debug.Log("Yes");
                    anim.SetBool("Shield", true);
                    newActionsAllowed = false;
                    switch (dir)
                    {
                        case Direction.Up:
                            UpShield.SetActive(true);
                            break;
                        case Direction.Down:
                            DownShield.SetActive(true);
                            break;
                        case Direction.Left:
                            LeftShield.SetActive(true);
                            break;
                        case Direction.Right:
                            RightShield.SetActive(true);
                            break;
                    }
                    return;
                case GadgetType.Hook:
                    GrapplingHook();
                    return;
                case GadgetType.RemoteBomb:
                    Bomb();
                    return;
                case GadgetType.Fan:
                    Luefter();
                    return;
                default:
                    break;
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

    private void ManageHealth() //only call when gaining or loosing health later
    {
        if (hp <= 0)
        {
            anim.SetTrigger("Death");
            newActionsAllowed = false;
            return;
        }

        if (hp != hpOld)
        {
            if (hp < hpOld)
            {
                hpScript.HealthUpdate(true, hpOld - hp);
                hpOld = hp;
            }
            else
            {
                hpScript.HealthUpdate(false, hp - hpOld);
                hpOld = hp;
            }
        }
        else
        {
            return;
        }
    }

    public void RecieveDamage(int value)
    {
        hp -= value;
    }

    void Move()
    {
        if (dodging)
        {
            rg.MovePosition((Vector2)transform.position + lastMoveVec.normalized * speedMultiplicator * dodgeMultiplicator);
            return;
        }

        if (Mathf.Abs(Input.GetAxis("P" + playernumber + "_Horizontal")) > .3f || Mathf.Abs(Input.GetAxis("P" + playernumber + "_Vertical")) > .3f)
        {
            if (Mathf.Abs(Input.GetAxis("P" + playernumber + "_Horizontal")) > Mathf.Abs(Input.GetAxis("P" + playernumber + "_Vertical")))
            {
                if (Input.GetAxis("P" + playernumber + "_Horizontal") > .3f)
                {
                    // GOING RIGHT
                    StopAnimations();
                    anim.SetBool("RightWalk", true);
                    dir = Direction.Right;
                }
                else
                {
                    // GOING LEFT
                    StopAnimations();
                    anim.SetBool("LeftWalk", true);
                    dir = Direction.Left;
                }
            }
            if (Mathf.Abs(Input.GetAxis("P" + playernumber + "_Vertical")) > Mathf.Abs(Input.GetAxis("P" + playernumber + "_Horizontal")))
            {
                if (Input.GetAxis("P" + playernumber + "_Vertical") > .3f)
                {
                    // GOING UP
                    StopAnimations();
                    anim.SetBool("UpWalk", true);
                    dir = Direction.Up;
                }
                else
                {
                    // GOING DOWN
                    StopAnimations();
                    anim.SetBool("DownWalk", true);
                    dir = Direction.Down;
                }
            }

            lastMoveVec = new Vector2(Input.GetAxis("P" + playernumber + "_Horizontal"), Input.GetAxis("P" + playernumber + "_Vertical"));
            rg.MovePosition((Vector2)transform.position + lastMoveVec * speedMultiplicator);
        }
        else
        {
            StopAnimations();
        }
    }
    #region RuneDistribution
    public void RuneDistP1(string rune1, string rune2)
    {
        Debug.Log("Go");
        if (rune1 == "WaterRune")
        {
            Rune1P1 = Runes.Water;
        }
        else if (rune1 == "FireRune")
        {
            Rune1P1 = Runes.Fire;
        }
        else if (rune1 == "ThunderRune")
        {
            Rune1P1 = Runes.Thunder;
        }
        else
        {
            Rune1P1 = Runes.None;
        }

        if (rune2 == "WaterRune")
        {
            Rune2P1 = Runes.Water;
        }
        else if (rune2 == "FireRune")
        {
            Rune2P1 = Runes.Fire;
        }
        else if (rune1 == "ThunderRune")
        {
            Rune1P1 = Runes.Thunder;
        }
        else
        {
            Rune2P1 = Runes.None;
        }

        if (Rune1P1 == Runes.Fire && Rune2P1 == Runes.Water || Rune2P1 == Runes.Water && Rune1P1 == Runes.Fire)
        {
            RuneInteractionP1 = Runes.Steam;
        }

        Debug.Log("Rune1: " + Rune1P1 + " Rune2: " + Rune2P1 + " RuneIT: " + RuneInteractionP1);
    }
    public void RuneDistP2(string rune1, string rune2)
    {
        Debug.Log("Go");
        if (rune1 == "WaterRune")
        {
            Rune1P2 = Runes.Water;
        }
        else if (rune1 == "FireRune")
        {
            Rune1P2 = Runes.Fire;
        }
        else if (rune1 == "ThunderRune")
        {
            Rune1P2 = Runes.Thunder;
        }
        else
        {
            Rune1P2 = Runes.None;
        }

        if (rune2 == "WaterRune")
        {
            Rune2P2 = Runes.Water;
        }
        else if (rune2 == "FireRune")
        {
            Rune2P2 = Runes.Fire;
        }
        else if (rune1 == "ThunderRune")
        {
            Rune1P2 = Runes.Thunder;
        }
        else
        {
            Rune2P2 = Runes.None;
        }

        if (Rune1P2 == Runes.Fire && Rune2P2 == Runes.Water || Rune2P2 == Runes.Water && Rune1P2 == Runes.Fire)
        {
            RuneInteractionP2 = Runes.Steam;
        }

        Debug.Log("Rune1: " + Rune1P2 + " Rune2: " + Rune2P2 + " RuneIT: " + RuneInteractionP2);
    }
    #endregion

    private void Strike()
    {
        Runes RuneCurrent;
        if (RuneInteractionP1 != Runes.None)
        {
            RuneCurrent = RuneInteractionP1;
        }
        else
        {
            RuneCurrent = Rune1P1;
        }

        switch (RuneCurrent)
        {
            case Runes.None:
                break;
            case Runes.Fire:
                Rune_Fire tmpScript1 = gameObject.GetComponent<Rune_Fire>();
                script = (IRune)tmpScript1;
                break;
            case Runes.Water:
                Rune_Water tmpScript2 = gameObject.GetComponent<Rune_Water>();
                script = (IRune)tmpScript2;
                break;
            case Runes.Thunder:
                Rune_Thunder tmpScript3 = gameObject.GetComponent<Rune_Thunder>();
                script = (IRune)tmpScript3;
                break;
            case Runes.Steam:
                Rune_Steam tmpScript4 = gameObject.GetComponent<Rune_Steam>();
                script = (IRune)tmpScript4;
                break;
            case Runes.Explosion:
                Rune_Explosion tmpScript5 = gameObject.GetComponent<Rune_Explosion>();
                script = (IRune)tmpScript5;
                break;
            case Runes.StunLighting:
                Rune_StunLighting tmpScript6 = gameObject.GetComponent<Rune_StunLighting>();
                script = (IRune)tmpScript6;
                break;

            default:
                break;
        }
        switch (dir)
        {
            case Direction.Up:
                UpStrike.SetActive(true);
                if (script != null)
                {
                    script.ParticleEffect(new Vector3(0, 0.1f, 0), true);
                }
                break;
            case Direction.Down:
                DownStrike.SetActive(true);
                if (script != null)
                {
                    script.ParticleEffect(new Vector3(0, 0.1f, 0), true);
                }
                break;
            case Direction.Left:
                LeftStrike.SetActive(true);
                if (script != null)
                {
                    script.ParticleEffect(new Vector3(0, 0.1f, 0), true);
                }
                break;
            case Direction.Right:
                RightStrike.SetActive(true);
                if (script != null)
                {
                    script.ParticleEffect(new Vector3(0, 0.1f, 0), true);
                }
                break;
        }
        StartCoroutine(EndStrike());
    }

    private IEnumerator EndStrike()
    {
        yield return new WaitForSeconds(.1f);
        LeftStrike.SetActive(false);
        RightStrike.SetActive(false);
        UpStrike.SetActive(false);
        DownStrike.SetActive(false);
    }

    //Instantiating the Arrow in the direction the player is facing and determining if a Rune Effect triggers.
    private void Shoot()
    {
        Runes RuneCurrent = Runes.None;
        //Chance for a Special Arrow.
        randomNumber = UnityEngine.Random.Range(0, 10);
        if (randomNumber == 0)
            RuneCurrent = Rune1P1;
        else if (randomNumber == 1)
            RuneCurrent = Runes.Water;
        else if (randomNumber == 2)
            RuneCurrent = RuneInteractionP1;
        else
            RuneCurrent = Runes.None;

        switch (RuneCurrent)
        {
            case Runes.None:
                GameObject NormalArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg)));
                NormalArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.Fire:
                GameObject FireArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                FireArrow.GetComponent<Arrow>().RuneSelect(1);
                FireArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.Water:
                GameObject WaterArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                WaterArrow.GetComponent<Arrow>().RuneSelect(2);
                WaterArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.Thunder:
                GameObject ThunderArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                ThunderArrow.GetComponent<Arrow>().RuneSelect(3);
                ThunderArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.Steam:
                GameObject SteamArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                SteamArrow.GetComponent<Arrow>().RuneSelect(4);
                SteamArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.Explosion:
                GameObject ExplosionArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                ExplosionArrow.GetComponent<Arrow>().RuneSelect(5);
                ExplosionArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            case Runes.StunLighting:
                GameObject StunLightingArrow = Instantiate(this.Arrow, transform.position, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg))) as GameObject;
                StunLightingArrow.GetComponent<Arrow>().RuneSelect(6);
                StunLightingArrow.GetComponent<Arrow>().AssignPlayer(this.gameObject);
                break;
            default:
                break;
        }
    }

    void GrapplingHook() //Hook
    {
        GameObject Hook = Instantiate(this.Hook, transform.position + (Vector3)lastMoveVec.normalized * .3f, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg)));
    }
    void Bomb() //RemoteBomb
    {
        GameObject RemoteBomb = Instantiate(this.RemoteBomb, transform.position + (Vector3)lastMoveVec.normalized * .3f, Quaternion.identity);
    }
    void Luefter() //Fan
    {
        GameObject Fan = Instantiate(this.Fan, transform.position + (Vector3)lastMoveVec.normalized * .25f, Quaternion.Euler(0, 0, (Mathf.Atan2(lastMoveVec.x, -lastMoveVec.y) * Mathf.Rad2Deg)));
    }

    /// <summary>
    /// function wich lets the player look with a raycast
    /// </summary>
    void PlayerView()
    {
        Vector2 lookDir = DirtoVector2(dir);
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lookDir, 0.15f);
        Debug.DrawRay(transform.position, lookDir * 0.15f, Color.blue);
        if (hit.collider == null)
        {
            return;
        }
        if (hit.collider.tag == "RuneGuy")
        {
            if (Input.GetButtonDown("P" + playernumber + "_UI")) //X on an XBOX Controller
            {
                interfaceScript.setActiveBoolRune = true;
                interfaceScript.menuOpen = true;
                interfaceScript.playerInfo = Convert.ToInt32(playernumber);
            }
        }
        if (hit.collider.tag == "GadgetGuy")
        {
            if (true)
            {
                if (Input.GetButtonDown("P" + playernumber + "_UI")) //X on an XBOX Controller
                {
                    interfaceScript.setActiveBoolGadget = true;
                    interfaceScript.menuOpen = true;
                    interfaceScript.playerInfo = Convert.ToInt32(playernumber);
                }

            }
        }
        if (hit.collider.tag == "DialogNPC")
        {
            if (Input.GetButtonDown("P" + playernumber + "_UI")) //X on an XBOX Controller
            {
                
            }
        }
        if (hit.collider.tag == "SaveGuy")
        {
            if (Input.GetButtonDown("P" + playernumber + "_UI")) //X on an XBOX Controller
            {
                interfaceScript.setActiveBoolSave = true;
                interfaceScript.menuOpen = true;
                interfaceScript.playerInfo = Convert.ToInt32(playernumber);
                interfaceScript.posPlayer = gameObject.transform.position;
            }
        }
    }

    /// <summary>
    /// Transforms the enum Direction to a Vector2
    /// </summary>

    public Vector2 DirtoVector2(Direction dir)
    {
        Vector2 look = new Vector2(0, 0);

        switch (dir)
        {
            case Direction.Up:
                look = new Vector2(0, 1);
                return look;
            case Direction.Down:
                look = new Vector2(0, -1);
                return look;
            case Direction.Left:
                look = new Vector2(-1, 0);
                return look;
            case Direction.Right:
                look = new Vector2(1, 0);
                return look;
            default:
                break;
        }

        return look;
    }


    private IEnumerator Dodge()
    {
        yield return new WaitForSeconds(.3f);
        dodging = false;
        StartCoroutine(DodgeCool());
    }

    private IEnumerator DodgeCool() //not necessarily for balance, but to avoid dodge-spam issues
    {
        yield return new WaitForSeconds(.1f);
        dodgeCooldown = false;
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(.4f);
        newActionsAllowed = true;
    }

}
