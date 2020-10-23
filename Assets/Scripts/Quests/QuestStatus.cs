using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Quests
{
    [System.Serializable]
    public class QuestStatus
    {
        [SerializeField]
        Quest quest;
        [SerializeField]
        List<string> completedObjectives;

        public QuestStatus(Quest quest)
        {
            this.quest = quest;
            completedObjectives = new List<string>();
        }

        public QuestStatus(object objectState)
        {
            QuestStatusRecord state = objectState as QuestStatusRecord;

            this.quest = Quest.GetByName(state.GetName());
            completedObjectives = state.GetObjectives();
        }

        [System.Serializable]
        class QuestStatusRecord
        {
            string questName;
            List<string> completedObjectives;

            public QuestStatusRecord(string name, List<string> objectives)
            {
                questName = name;
                completedObjectives = objectives;
            }

            public string GetName()
            {
                return questName;
            }

            public List<string> GetObjectives()
            {
                return completedObjectives;
            }
        }

        public object CaptureState()
        {
            return new QuestStatusRecord(quest.GetTitle(), completedObjectives);
        }






        public Quest GetQuest()
        {
            return quest;
        }

        public IEnumerable<string> GetCompletedObjectives()
        {
            return completedObjectives;
        }
           
        public string GetQuestName()
        {
            return quest.GetTitle();
        }



        public void CompleteObjective(string completedObjective)
        {
            completedObjectives.Add(completedObjective);
            CompletedQuest();
        }

        void CompletedQuest()
        {
            if (completedObjectives.Count == quest.GetObjectiveCount())
            {
                Debug.Log("COMPLETED QUEST");
                GiveReward();
            }
                
        }

        private void GiveReward()
        {
            //TODO:: implement
        }
    }





}
