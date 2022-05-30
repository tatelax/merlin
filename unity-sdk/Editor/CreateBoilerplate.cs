using UnityEditor;

internal static class CreateBoilerplate
{
    private static string MainControllerBoilerplate = 
        "public class MainController : SimulationController\n{\n\n}";

    
    [MenuItem("Assets/Create/Merlin/Main Controller", false, 1)]
    private static void CreateMainController()
    {
        ProjectWindowUtil.CreateAssetWithContent(
            "MainController.cs",
            MainControllerBoilerplate);
    }

    // private static void CreateWorld()
    // {
    //     ProjectWindowUtil.CreateScriptAssetFromTemplateFile("Assets/WorldTemplate", "NewWorld.cs");
    // }
}
