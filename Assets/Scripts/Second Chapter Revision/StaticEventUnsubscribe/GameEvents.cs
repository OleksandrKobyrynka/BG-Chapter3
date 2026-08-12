using System;
using UnityEngine;

public static class GameEvents
{
    public static event Action<int> ScoreChanged;

    public static void ChangeScore(int score)
    {
        ScoreChanged?.Invoke(score);
    }

    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    //private static void ResetStaticEvents()
    //{
    //    ScoreChanged = null;
    //}
}