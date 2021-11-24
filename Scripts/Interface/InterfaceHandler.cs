using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InterfaceHandler : MonoBehaviour
{

    public ImageList[] images;
    private InterfaceScript intScript;
    private XmlSaveSystem xmlSaver;

    private int x = 0;
    private int y = 0;

    private Vector2 position;

    private bool p1full = false;
    private bool p2full = false;
    private bool g1full;
    private bool canMove = false;

    public Image selectImage;
    public Image runePlace1;
    public Image runePlace2;
    public Image gadgetPlace;

    public Image runeSlot0;
    public Image runeSlot1;
    public Image runeSlot2;
    public Image runeSlot3;
    public Image runeSlot4;

    public Image gadgetSlot0;
    public Image gadgetSlot1;
    public Image gadgetSlot2;
    public Image gadgetSlot3;
    public Image gadgetSlot4;
    public Image gadgetSlot5;


    private Vector2 test0;
    private Vector3 test1;



    
    public Sprite transp;



    // Use this for initialization
    void Start()
    {
        intScript = GameObject.Find("InterfaceHandlerGO").GetComponent<InterfaceScript>();
        xmlSaver = GameObject.Find("XmlSaveTester").GetComponent<XmlSaveSystem>();
        SelectPos();

    }

    // Update is called once per frame
    void Update()
    {
        AllowToMove();
        PlayerMoveInput();

    }


    public void OpenMenu()
    {
        AllowToMove();
        PlayerMoveInput();
        PlayerActionInput();
    }

    /// <summary>
    /// Handels the movecontroll for the interface
    /// </summary>
    private void AllowToMove()
    {
        if (canMove)
        {
            return;
        }

        if (Mathf.Abs(Input.GetAxis("MenuHorizontal")) < 0.15f && Mathf.Abs(Input.GetAxis("MenuVertical")) < 0.15f)
        {
            canMove = true;
        }
    }
    /// <summary>
    /// Handels the player input for the joystick and the Dpad
    /// </summary>
    private void PlayerMoveInput()
    {
        if (canMove)
        {
            if (Input.GetAxis("MenuVertical") > 0.15f)
            {
                MoveUp();
            }
            else if (Input.GetAxis("MenuVertical") < -0.15f)
            {
                MoveDown();
            }
            else if (Input.GetAxis("MenuHorizontal") > 0.15f)
            {
                MoveRight();
            }
            else if (Input.GetAxis("MenuHorizontal") < -0.15f)
            {
                MoveLeft();
            }

        }
    }
    /// <summary>
    /// To move up in the interface
    /// </summary>
    private void MoveUp()
    {
        if (y > 0)
        {
            y--;
        }

        if (x >= images[y].images.Length)
        {
            x = images[y].images.Length - 1;
        }

        SelectPos();
        canMove = false;
    }
    /// <summary>
    /// To move down in the interface
    /// </summary>
    private void MoveDown()
    {
        if (y < images.Length - 1)
        {
            y++;
        }


        if (x >= images[y].images.Length)
        {
            x = images[y].images.Length - 1;
        }

        SelectPos();
        canMove = false;
    }
    /// <summary>
    /// To move right in the interface
    /// </summary>
    private void MoveRight()
    {
        if (x < images[y].images.Length - 1)
        {
            x++;
        }

        SelectPos();
        canMove = false;
    }
    /// <summary>
    /// To move left in the interface
    /// </summary>
    private void MoveLeft()
    {
        if (x > 0)
        {
            x--;
        }

        SelectPos();
        canMove = false;
    }

    /// <summary>
    /// Moves the selected Image
    /// </summary>
    private void SelectPos()
    {

        selectImage.rectTransform.anchoredPosition = images[y].images[x].rectTransform.anchoredPosition;
        selectImage.rectTransform.sizeDelta = new Vector2(images[y].images[x].rectTransform.rect.width, images[y].images[x].rectTransform.rect.height);
    }
    /// <summary>
    /// Handels the player input for buttons
    /// </summary>
    private void PlayerActionInput()
    {
        if (Input.GetButtonDown("MenuButton"))
        {
            if (images[y].images[x].tag=="BackButton")
            {
                CloseAction();
                return;
            }
            if (images[y].images[x].tag=="addRune")
            {
                SelectAction();
                return;
            }
            if (images[y].images[x].tag=="SelectedRRune")
            {
                UnSelectAction();
                return;
            }
            if (images[y].images[x].tag=="Insert")
            {
                if (intScript.setActiveBoolRune)
                {
                    xmlSaver.SaveRune(runePlace1.sprite, runePlace2.sprite, transp);

                    CloseAction();
                }
                if (intScript.setActiveBoolGadget)
                {
                    xmlSaver.SaveGadget(gadgetPlace.sprite, transp);
                    CloseAction();
                }
            }
            if (images[y].images[x].tag=="SaveButton")
            {
                xmlSaver.XMLSaveRune(runeSlot0, runeSlot1, runeSlot2, runeSlot3, runeSlot4);
                xmlSaver.XMLSaveGadget(gadgetSlot0, gadgetSlot1, gadgetSlot2, gadgetSlot3, gadgetSlot4, gadgetSlot5);
                xmlSaver.XMLSavePosition(position.x, position.y);
                CloseAction();
            }
        }
    }


    #region ButtonActions

    /// <summary>
    /// To close the menus
    /// </summary>
    private void CloseAction()
    {
        x = 0;
        y = 0;

        if (intScript.setActiveBoolRune)
        {
            runePlace1.sprite = transp;
            runePlace2.sprite = transp;
        }
        if (intScript.setActiveBoolGadget)
        {
            gadgetPlace.sprite = transp;
        }
        intScript.setActiveBoolGadget = false;
        intScript.setActiveBoolRune = false;
        intScript.setActiveBoolSave = false;
        intScript.menuOpen = false;
        p1full = false;
        p2full = false;


        SelectPos();
    }
    /// <summary>
    /// Selecteds the pic to get saved
    /// </summary>
    private void SelectAction()
    {


        if (p1full && p2full) 
        {
            return;
        }

        if (intScript.setActiveBoolGadget && images[y].images[x].sprite != transp) 
        {
            gadgetPlace.sprite = images[y].images[x].sprite;
            return;
        }

        if (!p1full&&intScript.setActiveBoolRune && images[y].images[x].sprite != transp)
        {
            runePlace1.sprite = images[y].images[x].sprite;
            p1full = true;
            return;
        }

       else if (!p2full && intScript.setActiveBoolRune && images[y].images[x].sprite != transp)
        {
            runePlace2.sprite = images[y].images[x].sprite;
            p2full = true;
            return;
        }
    }
    /// <summary>
    /// Unselected the once selected pic
    /// </summary>
    private void UnSelectAction()
    {
        if (images[y].images[x].sprite!=transp)
        {
            images[y].images[x].sprite = transp;
            if (images[y].images[x]==runePlace1)
            {
                p1full = false;
                runePlace1.sprite = transp;
                return;
            }
            if (images[y].images[x] == runePlace2)
            {                                                  
                p2full = false;
                runePlace2.sprite = transp;
                return;
            }
        }

    }
    /// <summary>
    /// Returns the typ of runes that are selected in string
    /// </summary>
    private string SetRuneAction()
    {
        string tmp=null;

        if (runePlace1.sprite!=transp)
        {
            tmp = runePlace1.sprite.ToString();

            if (runePlace2.sprite!=transp)
            {
                tmp += "/" + runePlace2.sprite.ToString();
            }
        }
        return tmp;
    }

    /// <summary>
    /// Returns the type of gadget that is selected in string
    /// </summary>
    private string SetGadgetAction()
    {
        string tmp=null;


        if (gadgetPlace.sprite!=transp)
        {
            tmp = gadgetPlace.sprite.ToString();
        }
        return tmp;
    }
    #endregion
}
