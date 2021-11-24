using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Canvas))]
public class InterfaceScript : MonoBehaviour
{

    private GameObject runeMenu;
    private GameObject gadgetMenu;
    private GameObject saveMenu;

    private InterfaceHandler runeHandler;
    private InterfaceHandler gadgetHandler;
    private InterfaceHandler saveHandler;

    public bool setActiveBoolRune = false;
    public bool setActiveBoolGadget = false;
    public bool setActiveBoolSave = false;
    public bool menuOpen = false;

    public int playerInfo = 0;
    public Vector2 posPlayer;


    // Use this for initialization
    void Start()
    {
   
        runeHandler = GameObject.Find("RuneMenu").GetComponent<InterfaceHandler>();
        runeMenu = GameObject.Find("RuneMenu");
        runeMenu.SetActive(false);

        gadgetHandler = GameObject.Find("GadgetMenu").GetComponent<InterfaceHandler>();
        gadgetMenu = GameObject.Find("GadgetMenu");
        gadgetMenu.SetActive(false);

        saveHandler = GameObject.Find("SaveMenu").GetComponent<InterfaceHandler>();
        saveMenu = GameObject.Find("SaveMenu");
        saveMenu.SetActive(false);



    }

    // Update is called once per frame
    void Update()
    {
        if (setActiveBoolRune)
        {
            runeHandler.OpenMenu();
            runeMenu.SetActive(setActiveBoolRune);
        }
        else if (!setActiveBoolRune) { runeMenu.SetActive(setActiveBoolRune); }

        if (setActiveBoolGadget)
        {
            gadgetHandler.OpenMenu();
            gadgetMenu.SetActive(setActiveBoolGadget);
        }
        else if (!setActiveBoolGadget) { gadgetMenu.SetActive(setActiveBoolGadget); }

        if (setActiveBoolSave)
        {
            saveHandler.OpenMenu();
            saveMenu.SetActive(setActiveBoolSave);
        }
        else if (!setActiveBoolSave) { saveMenu.SetActive(setActiveBoolSave); }
    }
}
