using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour
    {
        [SerializeField]
        Dialogue dialogue = null;

        PlayerConversant playerConversant;

        [SerializeField]
        bool getExternalDialogue = false;

        private void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        public void StartConversation()
        {
            if (playerConversant.IsActive())
                return;

            if (getExternalDialogue)
                dialogue = GetNextDialogue();

            if (dialogue == null)
                return;

            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>().StartDialogue(dialogue, this);
        }


        Dialogue GetNextDialogue()
        {
            Debug.Log("fetching external dialogue");
            return GameManager.Instance.ProvideDialogue();
        }

    }




}
