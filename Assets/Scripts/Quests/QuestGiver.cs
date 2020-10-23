using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Quests
{

    public class QuestGiver : MonoBehaviour
    {

        [SerializeField] Quest quest;

        

        public void AddQuest()
        {
            if (quest == null)
                return;

            GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>().AddQuest(quest);
            quest = null;
        }

    }




}

