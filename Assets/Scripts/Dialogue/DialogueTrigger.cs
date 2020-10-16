using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace RPG.Dialogue
{

    public class DialogueTrigger : MonoBehaviour
    {
        [SerializeField]
        string action;

        [SerializeField]
        UnityEvent onTrigger;


        public string GetAction()
        {
            return action;
        }

        public void Trigger(string actionToTrigger)
        {
            if(actionToTrigger == action)
                onTrigger.Invoke();
        }


    }




}
