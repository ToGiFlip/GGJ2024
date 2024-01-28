using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

public static class ScoreManager
{
    private const string ScoreKey = "LastScores";

    private static List<float> Scores;
    public static float AverageScore { get; private set; }
    public static float? LastScore => Scores.Count > 0 ? Scores[^1] : null;

    static ScoreManager()
    {
        Scores = LoadScores();
        AverageScore = CalculateAverageScore();
    }

    private static float CalculateAverageScore() => Scores.Count > 0 ? Scores.Average() : 0;

    private static List<float> LoadScores()
    {
        var scores = PlayerPrefs.GetString(ScoreKey, "");
        if (string.IsNullOrEmpty(scores)) return new List<float>(10);
        var splitScores = scores.Split(",");
        var parsedScores = new List<float>(10);
        foreach (var score in splitScores)
        {
            try
            {
                parsedScores.Add(float.Parse(score, CultureInfo.InvariantCulture));
            }
            catch (Exception e)
            {
                Debug.LogError($"Failed to parse score {score}");
                Debug.LogException(e);
                throw;
            }
        }

        return parsedScores;
    }

    private static void SaveScores()
    {
        var scoreString = string.Join(",", Scores.Select(score => score.ToString(CultureInfo.InvariantCulture)));
        PlayerPrefs.SetString(ScoreKey, scoreString);
        PlayerPrefs.Save();
    }

    public static void AddScore(float score)
    {
        if (Scores.Count >= 10)
        {
            Scores.RemoveAt(0);
        }

        Scores.Add(score);
        AverageScore = CalculateAverageScore();
        SaveScores();
    }
}