using System.Globalization;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ViewsCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        using var wallCommand = new Transaction(document);
        wallCommand.Start("API Plan");

        var familyId = new FilteredElementCollector(document)
            .WhereElementIsElementType()
            .OfClass(typeof(ViewFamilyType))
            .Cast<ViewFamilyType>()
            .First(x => x.ViewFamily == ViewFamily.FloorPlan)
            .Id;

        var levelId = new FilteredElementCollector(document)
            //Ищет экземпляры
            .WhereElementIsNotElementType()
            //Ищет по категории
            .OfCategory(BuiltInCategory.OST_Levels)
            .FirstElementId();

        var viewPlan = ViewPlan.Create(document, familyId, levelId);
        viewPlan.Name = "First api plan in my life";

        wallCommand.Commit();

        return Result.Succeeded;
    }
}