using UnityEngine;

public class StaticEventUnsubscribe : MonoBehaviour
{
    private void OnEnable()
    {
        GameEvents.ScoreChanged += OnScoreChanged;
        Debug.Log($"{name}: subscribed");
    }

    private void OnDisable()
    {
        GameEvents.ScoreChanged -= OnScoreChanged;
        Debug.Log($"{name}: unsubscribed");
    }

    private void OnScoreChanged(int score)
    {
        Debug.Log($"{name}: New score = {score}");
    }
}