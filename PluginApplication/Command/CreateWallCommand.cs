using System.Collections.Generic;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class CreateWallCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        using var wallCommand = new Transaction(document);
        wallCommand.Start("Wall");

        var point1 = new XYZ(0, 0, 0);
        var point2 = new XYZ(0, 6, 0);
        var point3 = new XYZ(12, 6, 0);
        var point4 = new XYZ(12, 0, 0);
        var point5 = new XYZ(6, 8, 0);

        var curves = new List<Curve>
        {
            Line.CreateBound(point1, point2),
            Arc.Create(point2, point3, point5),
            Line.CreateBound(point3, point4),
            Line.CreateBound(point4, point1)
        };
        var levelId = new FilteredElementCollector(document)
            //Ищет экземпляры
            .WhereElementIsNotElementType()
            //Ищет по категории
            .OfCategory(BuiltInCategory.OST_Levels)
            .FirstElementId();

        var floorId = new FilteredElementCollector(document)
            //Ищет экземпляры
            .WhereElementIsElementType()
            //Ищет по категории
            .OfCategory(BuiltInCategory.OST_Floors)
            .FirstElementId();

        foreach (var curve in curves) Wall.Create(document, curve, levelId, false);

        var curveLoop = CurveLoop.Create(curves);
        //var curveLoop2 = CurveLoop.CreateViaOffset(curveLoop,13d,XYZ.BasisY);
        var loop = new List<CurveLoop> {curveLoop};
        Floor.Create(document, loop, floorId, levelId);

        wallCommand.Commit();

        return Result.Succeeded;
    }
}