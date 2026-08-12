using UnityEngine;
using UnityEngine.InputSystem;

public class ScoreEventTester : MonoBehaviour
{
    [SerializeField] private int _scoreToAdd = 10;

    private int _currentScore;

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            _currentScore += _scoreToAdd;

            Debug.Log($"Changing score to {_currentScore}");
            GameEvents.ChangeScore(_currentScore);
        }
    }
}