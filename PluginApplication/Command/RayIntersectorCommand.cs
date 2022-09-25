using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class RayIntersectorCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);

        var pickObject = uiDocument.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);
        var pickObjectGlobalPoint = pickObject.GlobalPoint;
        var referenceIntersector =
            new ReferenceIntersector(new ElementCategoryFilter(BuiltInCategory.OST_Roofs)
                    , FindReferenceTarget.Element
                    , (View3D) uiDocument.ActiveView)
                .FindNearest(pickObjectGlobalPoint, (XYZ.BasisZ));
        if (referenceIntersector == null)
        {
            TaskDialog.Show("Ray", "Nothing found");
            return Result.Succeeded;
        }

        var point = referenceIntersector.GetReference().GlobalPoint;
        TaskDialog.Show("Length: ", pickObjectGlobalPoint.DistanceTo(point).ToMillimeters().Round(0).ToString());

        return Result.Succeeded;
    }
}