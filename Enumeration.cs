// https://lostechies.com/jimmybogard/2008/08/12/enumeration-classes/
using System;
using System.Collections.Generic;
using System.Reflection;

public abstract class Enumeration<T>: IComparable
    where T : Enumeration<T>, new()
{
    private readonly int _value;
    private readonly string _displayName;

    protected Enumeration()
    {
    }

    protected Enumeration(int value, string displayName)
    {
        _value = value;
        _displayName = displayName;
    }

    public int Value
    {
        get { return _value; }
    }

    public string DisplayName
    {
        get { return _displayName; }
    }

    public override string ToString()
    {
        return DisplayName;
    }

    public static IEnumerable<T> GetAll()
    {
        var type = typeof(T);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);

        foreach (var info in fields)
        {
            var instance = new T();
            var locatedValue = info.GetValue(instance) as T;

            if (locatedValue != null)
            {
                yield return locatedValue;
            }
        }
    }

    public override bool Equals(object obj)
    {
        var otherValue = obj as Enumeration<T>;

        if (otherValue == null)
        {
            return false;
        }

        var typeMatches = GetType().Equals(obj.GetType());
        var valueMatches = _value.Equals(otherValue.Value);

        return typeMatches && valueMatches;
    }

    public override int GetHashCode()
    {
        return _value.GetHashCode();
    }

    public int CompareTo(object other)
    {
        return Value.CompareTo(((Enumeration<T>)other).Value);
    }
}