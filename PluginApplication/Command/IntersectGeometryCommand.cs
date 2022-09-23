using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class IntersectGeometryCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);

        var geometryElement = element.get_Geometry(new Options()
            {
                // IncludeNonVisibleObjects = false, закоменчена, так как по умолчанию программ сама выставит false
                View = uiDocument.ActiveView,
                //Строка описывающая зависимости (как в семействах когда замочки ставили)
                ComputeReferences = false,
                //DetailLevel = ViewDetailLevel.Fine игнорируется, так как уровень детализации устанавливается по умолчанию во View
            }
        );
        // Объявляем переменную для дальнейшего заполнения в цикле
        Solid solid;
        foreach (var geom in geometryElement)
        {
            var geoSolid = geom as Solid;
            if (geoSolid != null)
            {
                solid = geoSolid;
            }
        }

        var elementIds = new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            //Все элементы сравниваются с element и если они пересекаются, то  добавляются в итоговый список
            .WherePasses(new ElementIntersectsSolidFilter())
            .ToElementIds();

        return Result.Succeeded;
    }
}