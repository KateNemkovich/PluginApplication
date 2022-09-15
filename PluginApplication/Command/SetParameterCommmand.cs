using System.Globalization;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class SetParameterCommmand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var reference = uiDocument.Selection.PickObject(ObjectType.Element);
        var element = document.GetElement(reference);

        //var parameter = element.get_Parameter(BuiltInParameter.EXTRUSION_END_PARAM); Поиск по параметру группы (в разделе дефениций)
        //var parameter = element.LookupParameter("SecretParameter"); Поиск по имени
        //Расширение от Ромы которое может найти всё
        var parameter = element.GetParameter("SecretParameter");

        //Пишем AsDouble так как если посмотрим в ревите в лукап, там зайдем в параметры, то если в графе Storage Type записан дабл,
        //то используем AsDouble, если что-то другое, то другой метод, то же самое в Set (там уже выбираем нужную перегрузку)
        var parameterValue = parameter.AsDouble();

        using var roofExtension = new Transaction(document);
        roofExtension.Start("Roof extension");
        parameter.Set(parameterValue - 3d.FromMeters());
        roofExtension.Commit();

        TaskDialog.Show("Roof extension",
            $"New:{parameter.AsDouble().ToString(CultureInfo.InvariantCulture)}\n Old:{parameterValue.ToString()}");

        // var parameterSquare = element.get_Parameter(BuiltInParameter.HOST_AREA_COMPUTED).AsDouble();
        // var convertFromInternalUnits =
        //     UnitUtils.ConvertFromInternalUnits(parameterSquare, UnitTypeId.SquareMillimeters);

        //TaskDialog.Show("Square: ", convertFromInternalUnits.ToString(CultureInfo.InvariantCulture));

        return Result.Succeeded;
    }
}