using LazyECS;

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
