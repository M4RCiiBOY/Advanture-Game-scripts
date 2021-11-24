using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace QuestSystem 
{
    public class QuestManager : MonoBehaviour
    {
        public Text t;
        public bool[] isDone;

        private string[] title;
        private string[] questType;
        private string[] objectives;
        private int[] id;
        private int[] count;
        private int[] amount;

        private void Start()
        {
            isDone = new bool[10];
            title = new string[10];
            questType = new string[10];
            objectives = new string[10];
            id = new int[10];
            count = new int[10];
            amount = new int[10];
        }

        //listener if Enemy dies/ item is collected/ location is visited etc.
        public void AddQuest(int ID, string Title, string Objective, int Amount, string QuestType)
        {
            id[ID] = ID;
            title[ID] = Title;
            questType[ID] = QuestType;
            objectives[ID] = Objective;
            amount[ID] = Amount;
            isDone[ID] = false;
            Display(ID);
        }
        
        void Display(int id)
        {
            t.text = (title[id] + "\n" + questType[id] + " " + objectives[id] + " " + count[id] + "/" + amount[id]);
        }

        void TrackingQuest(int ID)
        {
            count[ID]++;
            if(count[ID] <= amount[ID])
            {
                isDone[ID] = true;
            }
            Display(id[ID]);
        }
    }
}
