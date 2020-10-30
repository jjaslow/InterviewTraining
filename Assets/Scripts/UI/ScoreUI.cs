using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using RPG.Dialogue;
using System.Linq;

public class ScoreUI : MonoBehaviour
{
    List<GameManager.Score> scores;

    [SerializeField]
    Text totalScore;
    [SerializeField]
    GameObject scoreContent;
    [SerializeField]
    GameObject scoreSectionHeaderPrefab;
    [SerializeField]
    GameObject scoreDescriptionPrefab;

    [SerializeField]
    List<Dialogue> dialogues = new List<Dialogue>();

    void Awake()
    {
        GetComponent<ScoreCalculator>().onScoreCalculated += UpdateUI;
        CheckDialogueCount();
    }

    private void CheckDialogueCount()
    {
        if (GameManager.Instance.GetTotalNumberOfDialogues() != dialogues.Count)
            Debug.LogWarning("Missing a dialogue in the Score Calculator List. Also check Switch statement in ScoreUI.cs");
    }

    private void UpdateUI()
    {
        scoreContent.transform.DetachChildren();

        //Final Score top header
        totalScore.text = "Final Score: " + GetComponent<ScoreCalculator>().GetTotalScore();

        scores = GameManager.Instance.GetFinalScoreList();

        foreach (Dialogue dialogue in dialogues)
        {
            //get scores for the selected dialogue
            List<GameManager.Score> selectedScores = scores.Where(s => s.dialogueName == dialogue.name).ToList();

            if(selectedScores.Count>0)
            {
                //section header
                GameObject header = Instantiate(scoreSectionHeaderPrefab, scoreContent.transform);
                header.GetComponentInChildren<Text>().text = HeaderName(dialogue.name);

                //list all the relevant scores under the header
                foreach (GameManager.Score score in selectedScores)
                {
                    GameObject description = Instantiate(scoreDescriptionPrefab, scoreContent.transform);

                    Text scoreText = description.transform.GetChild(0).GetComponent<Text>();
                    Text scoreDescription = description.transform.GetChild(1).GetComponent<Text>();

                    scoreText.text = "";
                    if (score.score > 0)
                    {
                        scoreText.text += "+";
                        scoreText.color = Color.green;
                        scoreDescription.color = Color.green;
                    }
                    else
                    {
                        scoreText.color = Color.red;
                        scoreDescription.color = Color.red;
                    }
                    scoreText.text += score.score.ToString();

                    scoreDescription.text = score.description;
                }
            }
        }
    }


    private string HeaderName(string dialogueName)
    {
        string result = dialogueName;

        switch (dialogueName)
        {
            case "ResumeSection1":
                result = "Customer Support and Service";
                break;
            case "ResumeSection2":
                result = "Resolving Customer Problems";
                break;
            case "ResumeSection3":
                result = "Move from Philadelphia to NYC";
                break;
            case "ResumeSection4":
                result = "Cole's Fashion Wear";
                break;
            case "ResumeSection5":
                result = "Darcy's Department Store";
                break;
            case "ResumeSection6":
                result = "College Degree";
                break;
            case "1 Introduction":
                result = "Candidate Greeting";
                break;
            case "3 Benefits":
                result = "Benefits Discussion";
                break;
            default:
                break;
        }
        return result;
    }


}
