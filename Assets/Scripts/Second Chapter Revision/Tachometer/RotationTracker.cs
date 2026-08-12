using TMPro;
using UnityEngine;

public class RotationTracker : MonoBehaviour
{
    private const float FULL_ROTATION_ANGLE = 360f;

    [SerializeField] private TMP_Text _infoText;

    private float _previousYAngle;
    private float _totalRotatedAngle;
    private float _elapsedTime;

    private bool _isTracking;

    private void OnEnable()
    {
        ResetTracker();
    }

    private void LateUpdate()
    {
        UpdateRotationTracking();
    }

    private void UpdateRotationTracking()
    {
        float currentYAngle = transform.localEulerAngles.y;

        float angleDelta = Mathf.DeltaAngle(_previousYAngle, currentYAngle);

        _previousYAngle = currentYAngle;

        if (Mathf.Abs(angleDelta) > 0.001f)
        {
            _isTracking = true;
        }

        if (!_isTracking)
        {
            return;
        }

        _elapsedTime += Time.deltaTime;

        _totalRotatedAngle += Mathf.Abs(angleDelta);

        int fullRotations = Mathf.FloorToInt(_totalRotatedAngle / FULL_ROTATION_ANGLE);

        UpdateText(fullRotations);
    }

    public void ResetTracker()
    {
        _previousYAngle = transform.localEulerAngles.y;
        _totalRotatedAngle = 0f;
        _elapsedTime = 0f;
        _isTracking = false;

        UpdateText(0);
    }

    private void UpdateText(int fullRotations)
    {
        if (_infoText == null)
        {
            return;
        }

        int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
        int seconds = Mathf.FloorToInt(_elapsedTime % 60f);

        _infoText.text = $"Time: {minutes:00}:{seconds:00}\n" +
            $"Rotations: {fullRotations}";
    }
}