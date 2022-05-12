using System.Collections;
using System.Collections.Generic;
using LazyECS.Component;
using UnityEngine;

public class TestComponent : IComponent
{
    public int Value { get; private set; }

    public bool Set(object pos) {
        Value = (int)pos;
        return true;
    }

    public object Get() => Value;
}
