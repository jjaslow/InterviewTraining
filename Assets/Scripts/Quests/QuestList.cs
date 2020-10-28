using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{
    public class QuestList : MonoBehaviour
    {
        [SerializeField]
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action OnQuestListUpdated;

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void AddQuest(Quest quest)
        {
            QuestStatus qs = new QuestStatus(quest);
            statuses.Add(qs);
            OnQuestListUpdated?.Invoke();
        }

        public void RefreshQuestList()
        {
            OnQuestListUpdated?.Invoke();
        }






    }



}
