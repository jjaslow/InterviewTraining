using RPG.Dialogue;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;



public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    [SerializeField]
    List<Dialogue> dialogues = new List<Dialogue>();
    int currentDialogue = 0;
    int numberOfResumeDialogues;
    int totalNumberOfDialogues;

    [SerializeField]
    List<Dialogue> completedDialogues = new List<Dialogue>();

    [SerializeField]
    List<Score> scores = new List<Score>();

    [Space]
    [SerializeField]
    AudioSource _voiceOverAudio;
    [SerializeField]
    GameObject resumeSection;


    [System.Serializable]
    public class Score
    {
        public int count;
        public string description;

        public Score(int count, string description)
        {
            this.count = count;
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
        totalNumberOfDialogues = numberOfResumeDialogues + dialogues.Count;
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
        if(!completedDialogues.Contains(d))
            completedDialogues.Add(d);
    }

    public bool ScoreAlreadyAdded(string description)
    {
        foreach (Score score in scores)
        {
            if (score.description == description)
                return true;
        }

        return false;
    }

    public Dialogue ProvideDialogue()
    {
        if (dialogues.Count == 0)
            return null;

        if (completedDialogues.Count == totalNumberOfDialogues)
            EndOfInterview();

        Dialogue dia = dialogues[currentDialogue];

        UpdateCurrentDialogue();

        return dia;
    }

    private void UpdateCurrentDialogue()
    {
        if (currentDialogue == 0)
            currentDialogue++;
        else if (completedDialogues.Count < numberOfResumeDialogues + 2-1)    //resume items + 1 and 2 candidate
            return;
        else
            currentDialogue++;
    }

    private void EndOfInterview()
    {
        Debug.Log("END OF INTERVIEW");
    }

    //TODO:: count score
}
