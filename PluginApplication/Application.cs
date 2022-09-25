using Autodesk.Revit.UI;
using Nice3point.Revit.Extensions;
using PluginApplication.Command;

namespace PluginApplication;

public class Application : IExternalApplication
{
    public Result OnStartup(UIControlledApplication application)
    {
        // application.CreateRibbonTab("PluginApplication");
        // var ribbonPanel = application.CreateRibbonPanel("PluginApplication", "Panel");
        // var type = typeof(Command);
        // var ribbonItem = (RibbonButton) ribbonPanel.AddItem(new PushButtonData("Button", "Something",
        //     Assembly.GetAssembly(type).Location,
        //     type.FullName));
        // ribbonItem.Image =
        //     new BitmapImage(
        //         new Uri("pack://application:,,,/PluginApplication;component/Resources/Images/RibbonIcon16.png"));
        // ribbonItem.LargeImage =
        //     new BitmapImage(
        //         new Uri("pack://application:,,,/PluginApplication;component/Resources/Images/RibbonIcon32.png"));

        var panel = application.CreatePanel("Panel", "PluginApplication");

        var summaryButton = panel.AddPushButton<ElementSummaryCommand>("Element Summary");
        summaryButton.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        summaryButton.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");


        var windowsButton = panel.AddPushButton<CollectWindowsCommand>("Windows Count");
        windowsButton.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        windowsButton.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var squareButton = panel.AddPushButton<ElementParametersCommand>("Square");
        squareButton.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        squareButton.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var modifyDocument = panel.AddPushButton<ModifyDocumentCommand>("Modify");
        modifyDocument.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        modifyDocument.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var roofExtension = panel.AddPushButton<SetParameterCommmand>("Roof extension");
        roofExtension.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        roofExtension.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var wallCreation = panel.AddPushButton<CreateWallCommand>("Wall");
        wallCreation.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        wallCreation.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var family = panel.AddPushButton<PlaceFamilyCommand>("Family");
        family.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        family.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var move = panel.AddPushButton<ChangeLocationCommand>("Move");
        move.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        move.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var intersections = panel.AddPushButton<ElementIntersectionCommand>("Intersections");
        intersections.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        intersections.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");

        var rayIntersections = panel.AddPushButton<RayIntersectorCommand>("RayIntersections");
        rayIntersections.SetImage("/PluginApplication;component/Resources/Images/RibbonIcon16.png");
        rayIntersections.SetLargeImage("/PluginApplication;component/Resources/Images/RibbonIcon32.png");


        return Result.Succeeded;
    }

    public Result OnShutdown(UIControlledApplication application)
    {
        return Result.Succeeded;
    }
}