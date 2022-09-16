using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class PlaceFamilyCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        using var familyCommand = new Transaction(document);
        familyCommand.Start("Family");

        var path = @"C:\Users\Cate\Desktop\Прога\PluginApplication\PluginApplication\Resources\Family\Boiler.rfa";
        document.LoadFamily(path, out var family);
        //Получение ID самого первого типоразмера семейства
        var familySymbolIds = family.GetFamilySymbolIds().First();
        var symbol = (FamilySymbol) document.GetElement(familySymbolIds);
        if (!symbol.IsActive) symbol.Activate();
        document.Create.NewFamilyInstance(new XYZ(7, 7, 7), symbol, StructuralType.NonStructural);

        familyCommand.Commit();

        uiDocument.PostRequestForElementTypePlacement(symbol);
        return Result.Succeeded;
    }
}