using RPG.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField]
    AudioSource _voiceOverAudio;
    [SerializeField]
    GameObject resumeSection;

    [Space]
    [SerializeField]
    List<Dialogue> candidateDialogues = new List<Dialogue>();
    int currentDialogue = 0;
    int numberOfResumeDialogues;
    int totalNumberOfDialogues;

    [Space]
    [SerializeField]
    List<Dialogue> completedDialogues = new List<Dialogue>();

    [Space]
    [SerializeField]
    List<Score> scores = new List<Score>();


   


    [System.Serializable]
    public class Score
    {
        public string dialogueName;
        public int score;
        public string description;

        public Score(int score, string description, string dialogueName)
        {
            this.dialogueName = dialogueName;
            this.score = score;
            this.description = description;
        }
    }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        numberOfResumeDialogues = resumeSection.transform.childCount;
        totalNumberOfDialogues = numberOfResumeDialogues + candidateDialogues.Count;
    }



    public void PlayVoiceOver(AudioClip clip)
    {
        _voiceOverAudio.clip = clip;
        _voiceOverAudio.Play();
    }

    public void AddToScore(Score newScore)
    {
        scores.Add(newScore);
    }

    public void AddToCompletedDialogues(Dialogue d)
    {
        if (!d.name.Contains("ResumeSection"))
            return;

        if(!completedDialogues.Contains(d))
            completedDialogues.Add(d);
    }

    public bool ScoreAlreadyAdded(string description, string name)
    {
        foreach (Score score in scores)
        {
            if (score.description == description && score.dialogueName == name)
                return true;
        }
        return false;
    }

    public List<Score> GetFinalScoreList()
    {
        return scores;
    }

    public Dialogue ProvideExternalDialogue()
    {
        if (candidateDialogues.Count == 0 || completedDialogues.Count == totalNumberOfDialogues)
            return null;

        if(completedDialogues.Count == numberOfResumeDialogues)
            currentDialogue++;

        Dialogue dia = candidateDialogues[currentDialogue];

        if (currentDialogue == 0)
            currentDialogue++;

        return dia;
    }


    public void EndOfInterview()
    {
        Debug.Log("END OF INTERVIEW");
        SceneManager.LoadScene(2);
    }

    public int GetTotalNumberOfDialogues()
    {
        return totalNumberOfDialogues;
    }

}
