using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace PluginApplication;
[Transaction(TransactionMode.Manual)]
public class Command: IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        TaskDialog.Show("Hello", "It's working");
        return Result.Succeeded; 
    }

}