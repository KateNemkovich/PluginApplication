using System.Text;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ElementSummaryCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        //блок ссылок на основные классы, которые будут использованы много раз
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        //блок вычислений
        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);
        var typeId = element.GetTypeId();
        var type = (ElementType) document.GetElement(typeId);

        var summaryBuilder = new StringBuilder();
        summaryBuilder.Append("Element name: ");
        summaryBuilder.AppendLine(element.Name);
        summaryBuilder.Append("Element category: ");
        summaryBuilder.AppendLine(element.Category.Name);
        summaryBuilder.Append("Size: ");
        summaryBuilder.AppendLine(type.Name);
        summaryBuilder.Append("Family name: ");
        summaryBuilder.Append(type.FamilyName);

        //блок вывода на экран (можно через "Element name"+ element.Name+..., а можно через Stringbuilder) 
        TaskDialog.Show("Element summary", summaryBuilder.ToString());

        return Result.Succeeded;
    }
}