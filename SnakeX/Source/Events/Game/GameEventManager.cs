using System;
using System.Collections.Generic;

namespace SnakeX.Events.Game
{
    public class GameEventManager<T>
    {
    private readonly Dictionary<GameEvent, List<Action<T>>> _eventDictionary = new();

    public void Register(GameEvent eventName, Action<T> handler)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<Action<T>>();
        }
        _eventDictionary[eventName].Add(handler);
    }

    public void Unregister(GameEvent eventName, Action<T> handler)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName].Remove(handler);
            if (_eventDictionary[eventName].Count == 0)
            {
                _eventDictionary.Remove(eventName);
            }
        }
    }

    public void Emit(GameEvent eventName, T args)
    {
        if (_eventDictionary.ContainsKey(eventName))
        {
            foreach (var handler in _eventDictionary[eventName])
            {
                handler.Invoke(args);
            }
        }
    } 
    }
}