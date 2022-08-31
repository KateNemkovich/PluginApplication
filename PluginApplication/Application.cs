using System.Reflection;
using Autodesk.Revit.UI;

namespace PluginApplication;

public class Application:IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        application.CreateRibbonTab("PluginApplication");
        var ribbonPanel = application.CreateRibbonPanel("PluginApplication","Panel");
        var type = typeof (Command);
        ribbonPanel.AddItem(new PushButtonData("Button", "Something", Assembly.GetAssembly(type).Location,
            type.FullName));

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}