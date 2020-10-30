using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField]
        Dialogue dialogue = null;

        GameObject player;
        PlayerConversant playerConversant;

        [SerializeField]
        bool getExternalDialogue = false;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            playerConversant = player.GetComponent<PlayerConversant>();
        }

        public void StartConversation()
        {
            if (playerConversant.IsActive())
                return;

            if (getExternalDialogue)
                dialogue = GameManager.Instance.ProvideExternalDialogue();

            if (dialogue == null)
                return;

            GameManager.Instance.AddToCompletedDialogues(dialogue);
            playerConversant.StartDialogue(dialogue, this);

            if(CompareTag("ResumeButton"))
                DeactivateButton();
        }

        private void DeactivateButton()
        {
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
        }



    }




}
