using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public int[] QuestID;
    int ID;

    private string[] title;
    private string[] description;
    private string[] questType;
    private string[] objectives;
    private string[] reward;
    private int[] amount;

    private QuestSystem.Quest q;
    private QuestSystem.QuestManager qm;

    void Start ()
    {
        q = GameObject.Find("QuestHandler").GetComponent<QuestSystem.Quest>();
        qm = GameObject.Find("QuestHandler").GetComponent<QuestSystem.QuestManager>();
        title = new string[10];
        description = new string[10];
        questType = new string[10];
        objectives = new string[10];
        reward = new string[10];
        amount = new int[10];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            LoadQuest();
        }
    }

    void LoadQuest()
    {
        for (int i = 0; i < QuestID.Length; i++)
        {
            q.getInformation(QuestID[i]);
            ID = QuestID[i];
            title[QuestID[i]] = q.title;
            description[QuestID[i]] = q.description;
            questType[QuestID[i]] = q.questType;
            objectives[QuestID[i]] = q.objectives;
            reward[QuestID[i]] = q.reward;
            amount[QuestID[i]] = q.amount;
            AcceptQuest();
        }
    }

    void DisplayQuest()
    {
        for (int i = 0; i < QuestID.Length; i++)
        {
            //title[QuestID[i]] Display QuestTitles Available
        }
            //On TitlePress display everything else
            //ID = Titlepress
    }

    void AcceptQuest()
    {
        qm.AddQuest(QuestID[ID],title[ID], objectives[ID], amount[ID], questType[ID]);
    }

    void ReturnQuest()
    {
        if (qm.isDone[ID] == true)
        {
            //reward
        }
    }
}
