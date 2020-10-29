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

        string playerName = "Hiring Manager";
        [SerializeField]
        string candidatesName;


        private void Start()
        {
            string name = PlayerPrefs.GetString("playerName");
            if(name!="")
                playerName = PlayerPrefs.GetString("playerName");
        }

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

            //FIRST CHECK IF THERE ARE PLAYER OPTIONS
            //if player turn we need to list our choices of replies
            int numChoiceResponses = currentDialogue.GetChoiceChildren(currentNode).Count();
            if(numChoiceResponses>0)
            {
                isChoosing = true;
                onConversationUpdated?.Invoke();
                return;
            }

            //else its the AI turn
            DialogueNode[] children = currentDialogue.GetAllChildren(currentNode).ToArray();

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
            return currentDialogue.GetChoiceChildren(currentNode);
        }

        public bool IsChoosing()
        {
            return isChoosing;

            //foreach(DialogueNode node in currentDialogue.GetAllChildren(currentNode).ToList())
            //{
            //    if (node.IsAChoice())
            //        return true;
            //}

            //return false;
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
            if(currentNode != null && currentNode.GetScore() != 0)
            {
                AddNodeToScore(currentNode.GetScore(), currentNode.GetDescription());
            }
        }

        public string GetCurrentConversantName()
        {
            if (currentNode.IsPlayerSpeaking() || isChoosing)
                return playerName;
            else
                return candidatesName;
        }


        void AddNodeToScore(int score, string description)
        {
            var newScore = new GameManager.Score(score, description, currentDialogue.name);
            Debug.Log("add score");

            if(!GameManager.Instance.ScoreAlreadyAdded(description, currentDialogue.name))
                GameManager.Instance.AddToScore(newScore);
        }

    }


}
