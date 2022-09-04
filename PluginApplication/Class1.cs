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
        var uiDocument = commandData.Application.ActiveUIDocument;
        var element = uiDocument.Selection.PickObject(ObjectType.Element);
        var document = uiDocument.Document;
        var receivedElement=document.GetElement(element);
        var idelement = receivedElement.GetTypeId();
        var element1 = document.GetElement(idelement);
        TaskDialog.Show("Pick element", receivedElement.Name);
        TaskDialog.Show("Category", receivedElement.Category.Name);
        TaskDialog.Show("Size",element1.Name);
        TaskDialog.Show("Size", ((ElementType)document.GetElement(idelement)).FamilyName);
        
        

        return Result.Succeeded;
    }
}