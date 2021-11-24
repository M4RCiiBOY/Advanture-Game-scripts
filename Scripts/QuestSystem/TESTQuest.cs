using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;
using System.IO;
using System.Xml;

public class TESTQuest : MonoBehaviour
{
    private string Title;
    private string QuestType;
    private string Objective;
    private string Amount;
    private string Description;
    private string Reward;

	void Start ()
    {
		
	}
	
	void Update ()
    {
		//if(Input.GetKeyDown(KeyCode.Space))
  //      {
  //          testXMLLoad();
  //      }
	}

    private void testXMLLoad()
    {
        XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/QuestsXML/" + "LectusIslandQuests.quest");
        reader.ReadToFollowing("ID");
        reader.ReadStartElement();
        int tmpcount = reader.ReadContentAsInt();
        for (int i = 0; i <= tmpcount; i++)
        {
            reader.ReadToFollowing("Title");
            reader.ReadStartElement();
            Title = reader.ReadContentAsString();
            reader.ReadToFollowing("QuestType");
            reader.ReadStartElement();
            QuestType = reader.ReadContentAsString();
            reader.ReadToFollowing("Objective");
            reader.ReadStartElement();
            Objective = reader.ReadContentAsString();
            reader.ReadToFollowing("Amount");
            reader.ReadStartElement();
            Amount = reader.ReadContentAsString();
            reader.ReadToFollowing("Description");
            reader.ReadStartElement();
            Description = reader.ReadContentAsString();
            reader.ReadToFollowing("Reward");
            reader.ReadStartElement();
            Reward = reader.ReadContentAsString();
            Debug.Log("Title: " + Title + "\nQuestType: " + QuestType + "\nObjective: " + Objective + "\nAmount: " + Amount
                + "\nDescription: " + Description + "\nReward: " + Reward);
        }

    }
}
