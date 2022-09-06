using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class CollectWindowsCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //блок ссылок на основные классы, которые будут использованы много раз
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var windows = new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfCategory(BuiltInCategory.OST_Windows)
            .ToElements();
/*
        new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .WherePasses(new ElementCategoryFilter(BuiltInCategory.OST_Windows))
            .ToElements();
*/
        TaskDialog.Show("Windows count: ", windows.Count.ToString());

        return Result.Succeeded;
    }
}