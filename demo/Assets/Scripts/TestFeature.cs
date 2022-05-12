using System.Collections;
using System.Collections.Generic;
using LazyECS;
using UnityEngine;

public class TestFeature : Feature
{
    public TestFeature()
    {
        Systems = new LazyECS.Systems()
            .Add(new TestInitializeSystem());
    }
}
