using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{

    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;

        [SerializeField] TMP_Text currentSpeakerName;
        [SerializeField] TMP_Text currentText;

        [SerializeField] GameObject nextButton;
        [SerializeField] GameObject choiceButtonPrefab;
        [SerializeField] GameObject quitButton;

        [SerializeField] GameObject AIResponse;
        [SerializeField] GameObject playerResponse;

        // Start is called before the first frame update
        void Start()
        {
            playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            UpdateUI();
        }



        public void OnNextButton()
        {
            playerConversant.Next();
        }

        public void OnQuitButton()
        {
            playerConversant.Quit();
        }


        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());

            if (!playerConversant.IsActive())
                return;

            AIResponse.SetActive(!playerConversant.IsChoosing());
            playerResponse.SetActive(playerConversant.IsChoosing());

            currentSpeakerName.text = playerConversant.GetCurrentConversantName();

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
                quitButton.SetActive(false);
            }
            else
            {
                currentText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
                quitButton.SetActive(!playerConversant.HasNext());
            }
        }

        private void BuildChoiceList()
        {
            playerResponse.transform.DetachChildren();
            foreach (DialogueNode choice in playerConversant.GetChoices())
            {
                GameObject b = Instantiate(choiceButtonPrefab, playerResponse.transform);
                b.GetComponentInChildren<TMP_Text>().text = choice.Text;
                PointerEvents pe = b.GetComponent<PointerEvents>();
                pe.OnClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                }
                );
            }
        }

  


    }


}

