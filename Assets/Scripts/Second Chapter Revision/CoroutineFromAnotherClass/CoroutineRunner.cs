using System.Collections;
using UnityEngine;

public class CoroutineRunner : MonoBehaviour
{
    private int _value = 0;
    public void StartProcess()
    {
        StartCoroutine(SomeCoroutine());
    }

    public IEnumerator SomeCoroutine()
    {
        Debug.Log("Coroutine started");
        yield return new WaitForSeconds(2f);
        Debug.Log($"Coroutine ended, value: {++_value}");
        //transform.position += Vector3.up;
    }
}
