using System.Globalization;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ElementParametersCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);

        var parameterSquare = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
        var convertFromInternalUnits =
            UnitUtils.ConvertFromInternalUnits(parameterSquare, UnitTypeId.SquareMillimeters);

        TaskDialog.Show("Square: ", convertFromInternalUnits.ToString(CultureInfo.InvariantCulture));

        return Result.Succeeded;
    }
}