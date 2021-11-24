using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.Win32;
using System.IO;
using System.Xml;

namespace QuestSystem
{
    public class Quest : MonoBehaviour
    {
        public string title;
        public string description;
        public string questType;
        public string objectives;
        public string reward;
        public int amount;
        string source;
        int id;
        bool isActive;
        bool isDone;

        public Quest()
        {

        }

        public void getInformation(int id)
        {
            XmlReader reader = XmlReader.Create(Application.streamingAssetsPath + "/QuestsXML/" + "LectusIslandQuests.quest");
            reader.ReadToFollowing("ID");
            reader.ReadStartElement();
            int tmpcount = reader.ReadContentAsInt();
            for (int i = 0; i <= tmpcount; i++)
            {
                reader.ReadToFollowing("Title");
                reader.ReadStartElement();
                title = reader.ReadContentAsString();
                reader.ReadToFollowing("QuestType");
                reader.ReadStartElement();
                questType = reader.ReadContentAsString();
                reader.ReadToFollowing("Objective");
                reader.ReadStartElement();
                objectives = reader.ReadContentAsString();
                reader.ReadToFollowing("Amount");
                reader.ReadStartElement();
                amount = reader.ReadContentAsInt();
                reader.ReadToFollowing("Description");
                reader.ReadStartElement();
                description = reader.ReadContentAsString();
                reader.ReadToFollowing("Reward");
                reader.ReadStartElement();
                reward = reader.ReadContentAsString();
                Debug.Log("Title: " + title + "\nQuestType: " + questType + "\nObjective: " + objectives + "\nAmount: " + amount.ToString()
                    + "\nDescription: " + description + "\nReward: " + reward);
            }
        }
    }
}
