using System.Collections.Generic;
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

        using var viewCommand = new Transaction(document);
        viewCommand.Start("API Plan");

        var familyId = new FilteredElementCollector(document)
            .WhereElementIsElementType()
            .OfClass(typeof(ViewFamilyType))
            .Cast<ViewFamilyType>()
            .First(x => x.ViewFamily == ViewFamily.FloorPlan)
            //получает именно айди элемента, а не типа как .GetTypeId
            .Id;


        var levelId = new FilteredElementCollector(document)
            //Ищет экземпляры
            .WhereElementIsNotElementType()
            //Ищет по категории
            .OfCategory(BuiltInCategory.OST_Levels)
            .FirstElementId();

        var titleBlock = new FilteredElementCollector(document)
            //Ищет экземпляры
            .WhereElementIsElementType()
            //Ищет по категории
            .OfCategory(BuiltInCategory.OST_TitleBlocks)
            .FirstElementId();

        var viewPlan = ViewPlan.Create(document, familyId, levelId);
        viewPlan.Name = "First api plan in my life";
        var viewId = viewPlan.Id;

        var viewSheet = ViewSheet.Create(document, titleBlock);
        viewSheet.Name = "First api sheet in my life";
        viewSheet.SheetNumber = "A106";
        var viewSheetId = viewSheet.Id;

        var x1 = viewSheet.Outline.Max.U;
        var x2 = viewSheet.Outline.Min.U;
        var y1 = viewSheet.Outline.Max.V;
        var y2 = viewSheet.Outline.Min.V;
        var pointX = (x1 + x2) / 2;
        var pointY = (y1 + y2) / 2;

        var viewPort = Viewport.Create(document, viewSheetId, viewId, new XYZ(pointX, pointY, 0.0));
        //Изменение масштаба плана, чтобы поместился на листе
        viewPlan.Scale = 200;

        //Добавление фильтров
        var filterID = new List<ElementId>
        {
            Category.GetCategory(document, BuiltInCategory.OST_Walls).Id
        };
        var elementParameterFilter = new ElementParameterFilter(ParameterFilterRuleFactory
            .CreateContainsRule(new ElementId(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS), "API comment"));
        //new LogicalOrFilter();
        // new LogicalAndFilter();
        var filter = ParameterFilterElement.Create(document, "comment", filterID, elementParameterFilter);

        viewPlan.AddFilter(filter.Id);
        viewPlan.SetFilterVisibility(filter.Id, false);
        viewPlan.SetIsFilterEnabled(filter.Id, true);

        viewCommand.Commit();

        //Открывает созданный вид, реализуется только после коммита
        uiDocument.ActiveView = viewSheet;

        return Result.Succeeded;
    }
}