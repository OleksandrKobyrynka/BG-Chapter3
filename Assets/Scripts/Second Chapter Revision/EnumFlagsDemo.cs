using System;
using UnityEngine;

[Flags]
public enum PlayerState
{
    None = 0,
    Running = 1 << 0,
    Jumping = 1 << 1,
    Shielded = 1 << 2,
    Poisoned = 1 << 3,

    Moving = Running | Jumping,
    Buffed = Shielded | Poisoned
}

public class EnumFlagsDemo : MonoBehaviour
{
    [SerializeField] private PlayerState _state;

    private void Start()
    {
        PrintState();
    }

    [ContextMenu("Print State")]
    private void PrintState()
    {
        Debug.Log($"Current state: {_state}");

        Debug.Log($"Running: {HasState(PlayerState.Running)} \n" +
            $"Jumping: {HasState(PlayerState.Jumping)}\n" +
            $"Shielded: {HasState(PlayerState.Shielded)}\n" +
            $"Poisoned: {HasState(PlayerState.Poisoned)}");
    }

    [ContextMenu("Add Poisoned")]
    private void AddPoisoned()
    {
        _state |= PlayerState.Poisoned;
        PrintState();
    }

    [ContextMenu("Remove Shielded")]
    private void RemoveShielded()
    {
        _state &= ~PlayerState.Shielded;
        PrintState();
    }

    [ContextMenu("Toggle Jumping")]
    private void ToggleJumping()
    {
        _state ^= PlayerState.Jumping;
        PrintState();
    }

    private bool HasState(PlayerState state)
    {
        return (_state & state) != 0;
    }
}