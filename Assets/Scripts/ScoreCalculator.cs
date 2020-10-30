using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    List<GameManager.Score> scores;

    //starting value
    int totalScore = 50;

    public event Action onScoreCalculated;

    private void Start()
    {
        scores = GameManager.Instance.GetFinalScoreList();

        CalculateTotalScore(scores);

        onScoreCalculated?.Invoke();
    }


    private void CalculateTotalScore(List<GameManager.Score> scores)
    {
        foreach (var score in scores)
            totalScore += score.score;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }
}


/*
 *         public Score(int count, string description, string dialogueName)
        {
            this.dialogueName = dialogueName;
            this.count = count;
            this.description = description;
        */