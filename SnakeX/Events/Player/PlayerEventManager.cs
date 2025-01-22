using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SnakeX.Events.Player;

namespace SnakeX.Events
{
    public class PlayerEventManager<T>
    {
    private readonly Dictionary<PlayerEvent, List<Action<T>>> _eventDictionary = new();

    public void Register(PlayerEvent eventName, Action<T> handler)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<Action<T>>();
        }
        _eventDictionary[eventName].Add(handler);
    }

    public void Unregister(PlayerEvent eventName, Action<T> handler)
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

    public void Emit(PlayerEvent eventName, T args)
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