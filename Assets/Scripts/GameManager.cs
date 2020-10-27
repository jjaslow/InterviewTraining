using RPG.Dialogue;
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

    PlayerConversant player;

    [SerializeField]
    List<Score> scores = new List<Score>();

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
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
    }

    [SerializeField]
    AudioSource _voiceOverAudio;

    public void PlayVoiceOver(AudioClip clip)
    {
        _voiceOverAudio.clip = clip;
        _voiceOverAudio.Play();
    }

    public void AddToScore(Score newScore)
    {
        scores.Add(newScore);
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

        int index = Random.Range(0, dialogues.Count);
        Dialogue dia = dialogues[index];
        dialogues.RemoveAt(index);
        Debug.Log("found a dialogue: " + dia.name);
        return dia;
    }
}
