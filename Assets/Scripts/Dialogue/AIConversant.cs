using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField]
        Dialogue dialogue = null;

        [SerializeField]
        string NPCName;



   

        public bool HandleRaycast(PlayerController callingController)
        {
            if (dialogue == null)
                return false;

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Conversation Started");
                callingController.GetComponent<PlayerConversant>().StartDialogue(dialogue, this);
            }

            return true;
        }

        public string GetNPCName()
        {
            return NPCName;
        }
    }




}
