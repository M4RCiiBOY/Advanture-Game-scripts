using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class XmlSaveSystem : MonoBehaviour
{

    private PlayerCtrl playerctrl;
    private InterfaceScript intScript;
    private DbScript intHand;

    public string savePath;

    private bool gadgetMenuSave = false;
    private bool runeMenuSave = false;
    private bool posSave = false;

    string rune1;
    string rune2;

    public class Data
    {
        public string gadgetData;

        public string runeData1;

        public string runeData2;
    }


    // Use this for initialization
    void Start()
    {
        // Data saveData = new Data();
        savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/My Games/Lectus Island";
        intScript = GameObject.Find("InterfaceHandlerGO").GetComponent<InterfaceScript>();
        playerctrl = GameObject.Find("Player").GetComponent<PlayerCtrl>();


        CheckPosSave();
        CheckGadgetMenuSave();
        CheckRuneMenuSave();

        if (runeMenuSave)
        {
            LoadRuneMenu(intHand.runeSlot0, intHand.runeSlot1, intHand.runeSlot2, intHand.runeSlot3, intHand.runeSlot4);
           
        }

        if (gadgetMenuSave)
        {
            LoadGadgetMenu(intHand.gadgetSlot0, intHand.gadgetSlot1, intHand.gadgetSlot2, intHand.gadgetSlot3, intHand.gadgetSlot4, intHand.gadgetSlot5);
 
        }

        if (!Directory.Exists(savePath))
        {
            CreateMyGames();
        }

        PlayerPos();

    }
    #region function


    /// <summary>
    /// Handels the saveproces and checks if the directoy exists and if not create it
    /// </summary>
    void SaveFile()
    {
       
        if (!Directory.Exists(savePath))
        {
            CreateMyGames();
        }
    }
 
    /// <summary>
    /// Creates the My Games folder in the document folder
    /// </summary>
    void CreateMyGames()
    {
        Directory.CreateDirectory(savePath);
    }

    void CheckPosSave()
    {
        if (File.Exists(savePath + "/position.save"))
        {
            posSave = true;
        }
    }
    void CheckGadgetMenuSave()
    {
        if (File.Exists(savePath + "/gadgetMenu.save"))
        {
            gadgetMenuSave = true;

        }
    }
    void CheckRuneMenuSave()
    {
        if (File.Exists(savePath + "/gadgetMenu.save"))
        {
            runeMenuSave = true;

        }
    }

    #endregion
    #region XML Save stuff

    /// <summary>
    /// Creates a XMl file to save the selected runes for the indiviual player
    /// </summary>
    /// <param name="rune1">Selected rune 1</param>
    /// <param name="rune2">Selected rune 2</param>
    /// <param name="transp">Transperent sprite</param>
    public void SaveRune(Sprite rune1, Sprite rune2, Sprite transp)
    {
        string sRune1 = null;
        string sRune2 = null;
        Data myData = new Data();



        if (rune1 != transp)
        {
            sRune1 = rune1.name;

        }
        if (rune2 != transp)
        {
            sRune2 = rune2.name;
            //myData.runeData = tmp;
        }

        XmlTextWriter writer = new XmlTextWriter(savePath + "/rune" + intScript.playerInfo + ".save", System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;

        createNode(sRune1, sRune2, writer);
        writer.WriteEndDocument();
        writer.Close();
    }

    /// <summary>
    /// Creates a XML file to save the selected gadget for the individual player
    /// </summary>
    /// <param name="gadget">The selectes gadget</param>
    /// <param name="transp">Transparent sprite</param>
    public void SaveGadget(Sprite gadget, Sprite transp)
    {
        string sGadget = null;
        if (gadget != transp)
        {
            sGadget = gadget.name;
        }

        XmlTextWriter writer = new XmlTextWriter(savePath + "/gadget" + intScript.playerInfo + ".save", System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        createNode(sGadget, writer);
        writer.WriteEndDocument();
        writer.Close();

    }
    /// <summary>
    /// Creates a XMl file to save the rune slots in the runeMenu
    /// </summary>
    /// <param name="runePlace0">Rune Slot 1</param>
    /// <param name="runePlace1">Rune Slot 2</param>
    /// <param name="runePlace2">Rune Slot 3</param>
    /// <param name="runePlace3">Rune Slot 4</param>
    /// <param name="runePlace4">Rune Slot 5</param>
    public void XMLSaveRune(Image runePlace0, Image runePlace1,
    Image runePlace2, Image runePlace3, Image runePlace4)
    {
        XmlTextWriter writer = new XmlTextWriter(savePath + "/runeMenu.save", System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        createNode(runePlace0.name, runePlace1.sprite.name, runePlace2.sprite.name, runePlace3.sprite.name, runePlace4.sprite.name, writer);
        writer.WriteEndDocument();
        writer.Close();
    }
    /// <summary>
    /// Saves the gadget slot of the GadgetMenu in a XML file
    /// </summary>
    /// <param name="gadgetPlace0">Gadget Slot 1</param>
    /// <param name="gadgetPlace1">Gadget Slot 2</param>
    /// <param name="gadgetPlace2">Gadget Slot 3</param>
    /// <param name="gadgetPlace3">Gadget Slot 4</param>
    /// <param name="gadgetPlace4">Gadget Slot 5</param>
    /// <param name="gadgetPlace5">Gadget Slot 6</param>
    public void XMLSaveGadget(Image gadgetPlace0, Image gadgetPlace1, Image gadgetPlace2, Image gadgetPlace3, Image gadgetPlace4, Image gadgetPlace5)
    {
        XmlTextWriter writer = new XmlTextWriter(savePath + "/gadgetMenu.save", System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        createNode(gadgetPlace0.sprite.name, gadgetPlace1.sprite.name, gadgetPlace2.sprite.name, gadgetPlace3.sprite.name, gadgetPlace4.sprite.name, gadgetPlace5.sprite.name, writer);
        writer.WriteEndDocument();
        writer.Close();
    }
    /// <summary>
    /// Saves the Position from the players in a XML file
    /// </summary>
    /// <param name="xPos"></param>
    /// <param name="yPos"></param>
    public void XMLSavePosition(float xPos, float yPos)
    {
        XmlTextWriter writer = new XmlTextWriter(savePath + "/position.save", System.Text.Encoding.UTF8);
        writer.WriteStartDocument(true);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        createNode(xPos, yPos, writer);
        writer.WriteEndDocument();
        writer.Close();
    }

    #endregion

    #region XML Load stuff
    /// <summary>
    /// Loads the names from the XML file to the GadgetMenu
    /// </summary>
    /// <param name="gS0">Gadget Slot for 1</param>
    /// <param name="gS1">Gadget Slot for 2</param>
    /// <param name="gS2">Gadget Slot for 3</param>
    /// <param name="gS3">Gadget Slot for 4</param>
    /// <param name="gS4">Gadget Slot for 5</param>
    /// <param name="gS5">Gadget Slot for 6</param>
    public void LoadGadgetMenu(Image gS0, Image gS1, Image gS2, Image gS3, Image gS4, Image gS5)
    {
       

        XmlTextReader xRead = new XmlTextReader(savePath + "/gadgetMenu.save");
        int i = 0;
        while (xRead.Read())
        {

            if (xRead.NodeType == XmlNodeType.Text)
            {

                switch (i)
                {
                    case 0:
                        gS0.sprite.name = xRead.Value;
                      
                        break;

                    case 1:
                        gS1.sprite.name = xRead.Value;
                       
                        break;

                    case 2:
                        gS2.sprite.name = xRead.Value;
                       
                        break;

                    case 3:
                        gS3.sprite.name = xRead.Value;
                        
                        break;

                    case 4:
                        gS4.sprite.name = xRead.Value;
                        
                        break;

                    case 5:
                        gS5.sprite.name = xRead.Value;
                       
                        break;


                }

            }
            i++;
        }

    }

    /// <summary>
    /// Loads the names from the XML file to the RuneMenu
    /// </summary>
    /// <param name="rS0">Rune Slot for 1</param>
    /// <param name="rS1">Rune Slot for 2</param>
    /// <param name="rS2">Rune Slot for 3</param>
    /// <param name="rS3">Rune Slot for 4</param>
    /// <param name="rS4">Rune Slot for 5</param>
    public void LoadRuneMenu(Image rS0, Image rS1, Image rS2, Image rS3, Image rS4)
    {
        XmlTextReader xRead = new XmlTextReader(savePath + "/runeMenu.save");
        int i = 0;
        {

        }
        while (xRead.Read())
        {

            if (xRead.NodeType == XmlNodeType.Text)
            {

                switch (i)
                {
                    case 0:
                        rS0.sprite.name = xRead.Value;
                     
                        break;

                    case 1:
                        rS1.sprite.name = xRead.Value;
                       
                        break;

                    case 2:
                        rS2.sprite.name = xRead.Value;
                        
                        break;

                    case 3:
                        rS3.sprite.name = xRead.Value;
                       
                        break;

                    case 4:
                        rS4.sprite.name = xRead.Value;
                       
                        break;


                }

            }
            i++;
        }

    }

    //public void LoadCurrentRunesP1()
    //{
    //    XmlTextReader xRead = new XmlTextReader(savePath + "/rune1.save");
    //    int i = 0;
    //    while (xRead.Read())
    //    {
    //        if (xRead.NodeType == XmlNodeType.Text)
    //        {
    //            if (i == 0)
    //            {
    //                rune1 = xRead.Value;

    //                i++;
    //            }
    //            else
    //            {
    //                rune2 = xRead.Value;
    //            }
    //        }
    //    }
    //    playerctrl.RuneDistP1(rune1, rune2);
    //}

    //public void LoadCurrentRunesP2()
    //{
    //    XmlTextReader xRead = new XmlTextReader(savePath + "/rune2.save");
    //    int i = 0;
    //    while (xRead.Read())
    //    {
    //        if (xRead.NodeType == XmlNodeType.Text)
    //        {
    //            if (i == 0)
    //            {
    //                rune1 = xRead.Value;
    //                i++;
    //            }
    //            else
    //            {
    //                rune2 = xRead.Value;
    //            }
    //        }
    //    }
    //    playerctrl.RuneDistP2(rune1, rune2);
    //}

        /// <summary>
        /// Loads the Player position from a saved xml file 
        /// </summary>
    public void PlayerPos()
    {
        XmlTextReader xRead = new XmlTextReader(savePath + "/position.save");
        bool sec = true;
        float tempX = 0f;
        while (xRead.Read())
        {
            if (xRead.NodeType == XmlNodeType.Text)
            {
                if (sec)
                {
                    tempX = float.Parse(xRead.Value);
                    sec = false;
                }
                else
                {
                    Camera.main.GetComponent<CameraCtrl>().PlacePlayers(tempX, float.Parse(xRead.Value));


                }
            }
        }
    }
    #endregion
    #region CreateNode Block

    /// <summary>
    /// Creates a node in XML for the Runes
    /// </summary>
    /// <param name="rune1">First rune</param>
    /// <param name="rune2">Second rune</param>
    /// <param name="writer">Xml writer</param>
    private void createNode(string rune1, string rune2, XmlTextWriter writer)
    {
        writer.WriteStartElement("Runes");
        writer.WriteStartElement("Slot1");
        writer.WriteString(rune1);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot2");
        writer.WriteString(rune2);
        writer.WriteEndElement();
        writer.WriteEndElement();

    }
    /// <summary>
    /// Creates a node in XML for the Gadgets
    /// </summary>
    /// <param name="gadget">The gadget</param>
    /// <param name="writer">Xml Writer</param>
    private void createNode(string gadget, XmlTextWriter writer)
    {
        writer.WriteStartElement("Gadget");
        writer.WriteString(gadget);
        writer.WriteEndElement();
    }

    /// <summary>
    /// Creates a node in Xml for the rune places in the rune menu
    /// </summary>
    /// <param name="rPlace0">Rune place 1</param>
    /// <param name="rPlace1">Rune place 2</param>
    /// <param name="rPlace2">Rune place 3</param>
    /// <param name="rPlace3">Rune place 4</param>
    /// <param name="rPlace4">Rune place 5</param>
    /// <param name="writer">Xml writer</param>
    private void createNode(string rPlace0, string rPlace1, string rPlace2, string rPlace3, string rPlace4, XmlTextWriter writer)
    {
        writer.WriteStartElement("RuneMenu");
        writer.WriteStartElement("Slot0");
        writer.WriteString(rPlace0);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot1");
        writer.WriteString(rPlace1);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot2");
        writer.WriteString(rPlace2);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot3");
        writer.WriteString(rPlace3);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot4");
        writer.WriteString(rPlace4);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }
    /// <summary>
    /// Creates a XML file for the Gadgets in the gadgetMenu
    /// </summary>
    /// <param name="gPlace0">Gadget place 1</param>
    /// <param name="gPlace1">Gadget place 2</param>
    /// <param name="gPlace2">Gadget place 3</param>
    /// <param name="gPlace3">Gadget place 4</param>
    /// <param name="gPlace4">Gadget place 5</param>
    /// <param name="gPlace5">Gadget place 6</param>
    /// <param name="writer">Xml writer</param>
    private void createNode(string gPlace0, string gPlace1, string gPlace2, string gPlace3, string gPlace4, string gPlace5, XmlTextWriter writer)
    {
        writer.WriteStartElement("GadgetMenu");
        writer.WriteStartElement("Slot0");
        writer.WriteString(gPlace0);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot1");
        writer.WriteString(gPlace1);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot2");
        writer.WriteString(gPlace2);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot3");
        writer.WriteString(gPlace3);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot4");
        writer.WriteString(gPlace4);
        writer.WriteEndElement();
        writer.WriteStartElement("Slot5");
        writer.WriteString(gPlace5);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }

    private void createNode(float xPos, float yPos, XmlWriter writer)
    {
        writer.WriteStartElement("PlayerPosition");
        writer.WriteStartElement("X");
        writer.WriteValue(xPos);
        writer.WriteEndElement();
        writer.WriteStartElement("Y");
        writer.WriteValue(yPos);
        writer.WriteEndElement();
        writer.WriteEndElement();
    }
    #endregion

}
