using System.Collections;
using System.Collections.Generic;
using LazyECS;
using UnityEngine;

public class MainSimulationController : SimulationController
{
    protected override void Awake()
    {
        base.Awake();

        InitializeWorlds(new IWorld[]
        {
            new MainWorld()
        });
    }
}
