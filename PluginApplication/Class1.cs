using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication;

[Transaction(TransactionMode.Manual)]
public class Command : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument =
            commandData.Application
                .ActiveUIDocument; //блок ссылок на основные классы, которые будут использованы много раз
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element); //блок вычислений
        var element = document.GetElement(reference);
        var typeId = element.GetTypeId();
        var type = (ElementType) document.GetElement(typeId);

        TaskDialog.Show("Pick element", element.Name); //блок вывода на экран
        TaskDialog.Show("Category", element.Category.Name);
        TaskDialog.Show("Size", type.Name);
        TaskDialog.Show("Size", type.FamilyName);

        return Result.Succeeded;
    }
}