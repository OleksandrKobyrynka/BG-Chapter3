using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tachometer : MonoBehaviour
{
    [SerializeField] private Rotator _rotator;
    [SerializeField] private Image _fillImage;
    [SerializeField] private float _smoothSpeed = 10f;

    private float _targetFill;
    private float _maxRpm = 120f;

    private void OnEnable()
    {
        if (_rotator != null)
        {
            _rotator.OnRpmChanged += SetRpm;
        }
    }
    private void OnDisable()
    {
        if (_rotator != null)
        {
            _rotator.OnRpmChanged -= SetRpm;
        }
    }

    private void Start()
    {
        if (_rotator != null)
        {
            SetRpm(_rotator.Rpm);
        }
    }

    public void SetRpm(float rpm)
    {
        _targetFill = Mathf.Clamp01(rpm / _maxRpm);
    }

    private void Update()
    {
        if (_fillImage != null)
        {
            _fillImage.fillAmount = Mathf.Lerp(_fillImage.fillAmount, _targetFill, Time.deltaTime * _smoothSpeed);
        }
    }
}