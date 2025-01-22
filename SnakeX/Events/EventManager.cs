using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SnakeX.Events
{
    public class EventManager<T>
    {
        private readonly Dictionary<string, List<Action<T>>> _eventDictionary = new();

    public void Register(string eventName, Action<T> handler)
    {
        if (!_eventDictionary.ContainsKey(eventName))
        {
            _eventDictionary[eventName] = new List<Action<T>>();
        }
        _eventDictionary[eventName].Add(handler);
    }

    public void Unregister(string eventName, Action<T> handler)
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

    public void Trigger(string eventName, T args)
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