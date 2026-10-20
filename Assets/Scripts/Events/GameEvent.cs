using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewGameEvent", menuName = "Modular Assets/Game Event")]
public class GameEvent : ScriptableObject
{
    // Tracks every listener currently subscribed to this event.
    private readonly List<GameEventListener> _listeners = new List<GameEventListener>();

    [ContextMenu("Raise")]
    public void Raise()
    {
        // Iterate backwards: if a listener removes itself during the loop,
        // a forward loop would skip the next index and throw an out-of-bounds error.
        for (int i = _listeners.Count - 1; i >= 0; i--)
        {
            _listeners[i].OnEventRaised();
        }
    }

    public void RegisterListener(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
        {
            _listeners.Add(listener);
        }
    }

    public void UnregisterListener(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
        {
            _listeners.Remove(listener);
        }
    }
}
