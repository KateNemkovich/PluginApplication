using System;
using System.Reflection;
using System.Windows.Media.Imaging;
using Autodesk.Revit.UI;

namespace PluginApplication;

public class Application : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        application.CreateRibbonTab("PluginApplication");
        var ribbonPanel = application.CreateRibbonPanel("PluginApplication", "Panel");
        var type = typeof(Command);
        var ribbonItem = (RibbonButton) ribbonPanel.AddItem(new PushButtonData("Button", "Something",
            Assembly.GetAssembly(type).Location,
            type.FullName));
        ribbonItem.Image =
            new BitmapImage(
                new Uri("pack://application:,,,/PluginApplication;component/Resources/Images/RibbonIcon16.png"));
        ribbonItem.LargeImage =
            new BitmapImage(
                new Uri("pack://application:,,,/PluginApplication;component/Resources/Images/RibbonIcon32.png"));

        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}