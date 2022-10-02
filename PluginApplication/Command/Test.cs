using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class Test : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        using var move = new Transaction(document);
        move.Start("Test");
        /*
    var reference1 = uiDocument.Selection.PickObject(ObjectType.Element);
    var pipe1 = document.GetElement(reference1);
    var locationPointPipe1 = (LocationPoint) pipe1.Location;
    var pointPipe1 = locationPointPipe1.Point;
    
    var reference2 = uiDocument.Selection.PickObject(ObjectType.Element);
    var pipe2 = document.GetElement(reference2);
    var locationPointPipe2 = (LocationPoint) pipe2.Location;
    var pointPipe2 = locationPointPipe2.Point;
    element.Move(pointPipe1.X, pointPipe1.Y, pointPipe2.Z);
      */
        var reference1 = uiDocument.Selection.PickObject(ObjectType.Element);
        var pipe1 = document.GetElement(reference1);
        var reference2 = uiDocument.Selection.PickObject(ObjectType.Element);
        var pipe2 = document.GetElement(reference2);

        var parameter2 = pipe2.get_Parameter(BuiltInParameter.RBS_CURVE_VERT_OFFSET_PARAM).AsDouble();
        var parameter1 = pipe1.get_Parameter(BuiltInParameter.RBS_CURVE_VERT_OFFSET_PARAM).Set(parameter2);

        move.Commit();

        return Result.Succeeded;
    }
}