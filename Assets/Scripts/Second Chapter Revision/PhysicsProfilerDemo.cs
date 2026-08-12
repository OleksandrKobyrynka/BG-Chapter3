using UnityEngine;
using UnityEngine.InputSystem;

public class PhysicsDebuggerDemo : MonoBehaviour
{
    [SerializeField] private int _spawnCount = 20;
    [SerializeField] private float _spawnRadius = 2f;

    [SerializeField] private float _queryDistance = 10f;
    [SerializeField] private float _queryRadius = 0.5f;

    [SerializeField] private bool _queriesActive = false;


    private void Update()
    {
        if (_queriesActive)
        {
            RunPhysicsQueries();
        }

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            SpawnDynamicSpheres();
        }

        if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            SpawnFilteringDemoObjects();
        }

        if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            DropBoxesForContacts();
        }

        if (Keyboard.current.digit4Key.wasPressedThisFrame)
        {
            SpawnMismatchedCollider();
        }

        if (Keyboard.current.digit5Key.wasPressedThisFrame)
        {
            WakeUpAll();
        }
    }

    private void SpawnDynamicSpheres()
    {
        for (int i = 0; i < _spawnCount; i++)
        {
            Vector3 randomPos = transform.position + Random.insideUnitSphere * _spawnRadius + Vector3.up * 3f;

            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);

            sphere.transform.position = randomPos;

            Rigidbody rb = sphere.AddComponent<Rigidbody>();

            rb.mass = Random.Range(0.5f, 5f);

            rb.centerOfMass = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
        }

        Debug.Log($"Spawned: {_spawnCount} dynamic spheres");
    }

    private void SpawnFilteringDemoObjects()
    {
        GameObject staticBox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        staticBox.name = "StaticCollider_Default";
        staticBox.transform.position =transform.position + Vector3.right * 3f;
        staticBox.layer = LayerMask.NameToLayer("Default");

        GameObject triggerZone = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        triggerZone.name = "TriggerZone_Water";
        triggerZone.transform.position = transform.position + Vector3.left * 3f;
        triggerZone.layer = LayerMask.NameToLayer("Water");
        Collider triggerCollider = triggerZone.GetComponent<Collider>();
        triggerCollider.isTrigger = true;

        GameObject dynamicBody = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        dynamicBody.name = "Rigidbody_IgnoreRaycast";
        dynamicBody.transform.position = transform.position + Vector3.forward * 3f + Vector3.up * 2f;
        dynamicBody.layer = LayerMask.NameToLayer("Ignore Raycast");
        dynamicBody.AddComponent<Rigidbody>();

        Debug.Log("Spawned filtering demo: static / trigger / rigidbody on different layers");
    }

    private void DropBoxesForContacts()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject box = GameObject.CreatePrimitive(PrimitiveType.Cube);
            box.name = $"ContactBox_{i}";

            box.transform.position = transform.position + new Vector3(i, 6f, 0f);

            Rigidbody rb = box.AddComponent<Rigidbody>();
            rb.mass = 1f;
        }

        Debug.Log("Dropped boxes onto ground to generate contacts");
    }

    private void RunPhysicsQueries()
    {
        Vector3 origin = transform.position;

        Physics.Raycast(origin, transform.forward, _queryDistance);

        Physics.SphereCast(origin, _queryRadius, transform.forward, out _, _queryDistance);

        Physics.OverlapSphere(origin,_queryRadius * 2f);

        Physics.BoxCast(origin, Vector3.one * _queryRadius, transform.forward, Quaternion.identity, _queryDistance);
    }

    private void SpawnMismatchedCollider()
    {
        GameObject visual = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        visual.name = "MismatchedVisual";
        visual.transform.position = transform.position + Vector3.up * 4f;
        visual.transform.localScale = Vector3.one * 0.5f;

        Destroy(visual.GetComponent<Collider>());

        BoxCollider hiddenCollider = visual.AddComponent<BoxCollider>();
        hiddenCollider.size = Vector3.one * 3f;

        Rigidbody rb = visual.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        Debug.Log("Spawned object with mismatched render/collision size");
    }

    private void WakeUpAll()
    {
        Rigidbody[] allBodies = FindObjectsByType<Rigidbody>(FindObjectsSortMode.None);

        foreach (Rigidbody rb in allBodies)
        {
            rb.WakeUp();
        }

        Debug.Log($"Awakened {allBodies.Length} objects");
    }
}