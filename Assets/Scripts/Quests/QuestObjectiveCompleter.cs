using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG.Quests
{

    public class QuestObjectiveCompleter : MonoBehaviour
    {
        [SerializeField] string questName;
        [SerializeField] string objectiveName;

        bool canDestroy = false;




        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CompleteObjective();
            }

            if(canDestroy)
                Destroy(this.gameObject);

            return true;
        }

        public  void CompleteObjective()
        {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();

            foreach (QuestStatus status in questList.GetStatuses())
            {
                if (status.GetQuestName() == questName && HasObjective(status) && !status.GetCompletedObjectives().Contains(objectiveName))
                {
                    status.CompleteObjective(objectiveName);
                    questList.RefreshQuestList();
                    canDestroy = true;
                }
            }
        }

        private bool HasObjective(QuestStatus status)
        {
            foreach(var objective in status.GetQuest().GetObjectives())
            {
                if (objective.reference == objectiveName)
                    return true;
            }
            return false;
        }
    }







}
