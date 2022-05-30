using System.Collections;
using System.Collections.Generic;
using MerlinSDK;
using UnityEngine;

public class TestFeature : Feature
{
    public TestFeature()
    {
        Systems = new MerlinSDK.Systems()
            .Add(new TestInitializeSystem());
    }
}
