using MerlinSDK;

public class MainController : SimulationController
{
    public Sync sync;

    protected override void Awake()
    {
        base.Awake();
        
        sync.WebsocketOpened += WebsocketOpened;
    }

    private void WebsocketOpened()
    {
        InitializeWorlds(new IWorld[]
        {
            new MainWorld()
        });
    }
}