using System;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private const int RPM_TO_DEGREES_PER_SEC = 6;

    [Range(0, 120)]
    [SerializeField] private float _rpm = 50f;

    public float Rpm
    {
        get => _rpm;
    }

    public event Action<float> OnRpmChanged;

    private void OnValidate()
    {
        _rpm = Mathf.Clamp(_rpm, 0f, 120f);
        OnRpmChanged?.Invoke(_rpm);
    }

    private void Update()
    {
        float rotationSpeed = _rpm * RPM_TO_DEGREES_PER_SEC;
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}