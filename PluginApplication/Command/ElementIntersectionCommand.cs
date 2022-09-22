using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ElementIntersectionCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);

        var elementIds = new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            //Все элементы сравниваются с element и если они пересекаются, то  добавляются в итоговый список
            .WherePasses(new ElementIntersectsElementFilter(element))
            .ToElementIds();

        uiDocument.Selection.SetElementIds(elementIds);

        return Result.Succeeded;
    }
}