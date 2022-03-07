using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Base class for having any sort of list in the game
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract class RuntimeCollection<T> :ScriptableObject
{
    public List<T> Items = new List<T>();

    public void Add(T t)
    {
        if (!Items.Contains(t)) Items.Add(t);
    }

    public void Remove(T t)
    {
        if (Items.Contains(t)) Items.Remove(t);
    }
}
