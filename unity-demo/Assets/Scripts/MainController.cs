using MerlinSDK;

public class MainController : SimulationController
{
    protected void Start()
    {

        InitializeWorlds(new IWorld[]
        {
            new MainWorld()
        });
    }
}