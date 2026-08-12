using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum RaycastMode
{
    LayerMask,
    IgnoreRaycastLayer,
    SpecificObject
}

public class IgnoreRaycastDemo : MonoBehaviour
{
    [SerializeField] private RaycastMode _mode;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private LayerMask _raycastLayers;
    [SerializeField] private Transform _objectToIgnore;

    private void Update()
    {
        Debug.DrawRay(transform.position, transform.forward * _maxDistance, Color.green);

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            CastRay();
        }
    }

    private void CastRay()
    {
        Ray ray = new Ray(transform.position,transform.forward);

        switch (_mode)
        {
            case RaycastMode.LayerMask:
                CastUsingLayerMask(ray);
                break;

            case RaycastMode.IgnoreRaycastLayer:
                CastIgnoringIgnoreRaycastLayer(ray);
                break;

            case RaycastMode.SpecificObject:
                CastIgnoringSpecificObject(ray);
                break;
        }
    }

    private void CastUsingLayerMask(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, _raycastLayers))
        {
            Debug.Log($"LayerMask hit: {hit.collider.name}");
        }
        else
        {
            Debug.Log("LayerMask: nothing hit");
        }
    }

    private void CastIgnoringIgnoreRaycastLayer(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit, _maxDistance, Physics.DefaultRaycastLayers))
        {
            Debug.Log($"Default layers hit: {hit.collider.name}");
        }
        else
        {
            Debug.Log("Default layers: nothing hit");
        }
    }

    private void CastIgnoringSpecificObject(Ray ray)
    {
        RaycastHit[] hits = Physics.RaycastAll(ray, _maxDistance, Physics.DefaultRaycastLayers);

        RaycastHit closestValidHit = default;
        bool foundValidHit = false;

        foreach (RaycastHit hit in hits)
        {
            if (IsIgnoredObject(hit.collider.transform))
            {
                continue;
            }

            if (!foundValidHit || hit.distance < closestValidHit.distance)
            {
                closestValidHit = hit;
                foundValidHit = true;
            }
        }

        if (foundValidHit)
        {
            Debug.Log($"Specific object ignored, hit: {closestValidHit.collider.name}");
        }
        else
        {
            Debug.Log("Specific object ignored: nothing else hit");
        }
    }

    private bool IsIgnoredObject(Transform hitTransform)
    {
        if (_objectToIgnore == null)
        {
            return false;
        }

        return hitTransform == _objectToIgnore || hitTransform.IsChildOf(_objectToIgnore);
    }
}