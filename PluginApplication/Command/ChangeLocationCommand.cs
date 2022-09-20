using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

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
        //Первый способ перемещения объекта
        locationPoint.Point = new XYZ(point.X + 1, point.Y, point.Z);
        //Второй способ перемещения объекта
        element.Location.Move(new XYZ(point.X + 1, point.Y + 1, point.Z));
        //Третий способ
        ElementTransformUtils.MoveElement(document, reference.ElementId, new XYZ(point.X - 1, point.Y + 0, point.Z));
        //Четвёртый способ(лучше двигать им)
        element.Move(point.X + 2, point.Y + 2, point.Z);
        move.Commit();

        return Result.Succeeded;
    }
}