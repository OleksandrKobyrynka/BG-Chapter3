using UnityEngine;
using UnityEngine.InputSystem;

public class CoroutineInitiator : MonoBehaviour
{
    [SerializeField] private CoroutineRunner _coroutineRunner;

    private void Update()
    {
        if (Keyboard.current.digit1Key.wasPressedThisFrame)
        {
            _coroutineRunner.StartProcess();
        }

        else if (Keyboard.current.digit2Key.wasPressedThisFrame)
        {
            StartCoroutine(_coroutineRunner.SomeCoroutine());
        }

        else if (Keyboard.current.digit3Key.wasPressedThisFrame)
        {
            _coroutineRunner.StartCoroutine(_coroutineRunner.SomeCoroutine());
        }
    }
}
