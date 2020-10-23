using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        Dialogue currentDialogue;

        DialogueNode currentNode;

        bool isChoosing = false;

        public event Action onConversationUpdated;

        AIConversant currentConversant = null;

        [SerializeField]
        string playerName;

        public void StartDialogue(Dialogue newDialogue, AIConversant NPC)
        {
            currentConversant = NPC;

            currentDialogue = newDialogue;
            currentNode = currentDialogue.GetRootNode();

            onConversationUpdated?.Invoke();
            TriggerEnterAction();
        }


        public bool IsActive()
        {
            return currentDialogue != null;
        }


        public string GetText()
        {
            if (currentNode == null)
                return "null dialogue error.";

            return currentNode.Text;
        }

        public void Next()
        {
            TriggerExitAction();

            //if player turn we need to list our choices of replies
            int numPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if(numPlayerResponses>0)
            {
                isChoosing = true;
                onConversationUpdated?.Invoke();
                return;
            }

            //else its the AI turn
            DialogueNode[] children = currentDialogue.GetAIChildren(currentNode).ToArray();

            //if a choice doesnt have a next node
            if (children.Length == 0)
            {
                Quit();
                return;
            }

            int index = UnityEngine.Random.Range(0, children.Length);
            currentNode = children[index];
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }



        public bool HasNext()
        {
            int len = currentDialogue.GetAllChildren(currentNode).Count();
            return len > 0;
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);

            //yield return "aaa";
            //yield return "bbb";
            //yield return "ccc";
        }

        public bool IsChoosing()
        {
            return isChoosing;
        }

        public void SelectChoice(DialogueNode chosenNode)
        {
            isChoosing = false;
            currentNode = chosenNode;
            TriggerEnterAction();
            Next();
        }




        public void Quit()
        {
            TriggerExitAction();

            currentConversant = null;
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;

            onConversationUpdated?.Invoke();

        }


        private void TriggerEnterAction()
        {
            if(currentNode!=null && currentNode.GetOnEnterAction()!="")
            {
                foreach(DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
                    trigger.Trigger(currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (currentNode != null && currentNode.GetOnExitAction() != "")
            {
                foreach (DialogueTrigger trigger in currentConversant.GetComponents<DialogueTrigger>())
                    trigger.Trigger(currentNode.GetOnExitAction());
            }
        }

        public string GetCurrentConversantName()
        {
            if (isChoosing)
                return playerName;
            else
                return currentConversant.GetNPCName();
        }


    }


}
