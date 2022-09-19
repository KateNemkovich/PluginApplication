using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ChangeLocationCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        using var move = new Transaction(document);
        move.Start("Move");

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);
        var locationPoint = (LocationPoint) element.Location;
        var point = locationPoint.Point;
        locationPoint.Point = new XYZ(point.X + 1, point.Y, point.Z);

        move.Commit();

        return Result.Succeeded;
    }
}