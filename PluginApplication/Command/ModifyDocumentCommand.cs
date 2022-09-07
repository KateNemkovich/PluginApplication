using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ModifyDocumentCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        var references = uiDocument.Selection.PickObjects(ObjectType.Element);
        //используем Transaction для отмены действия, так как ревит написан на с++ и там нужно очищать,поэтому используем using для этого 
        // в целом, когда используется IDisposable, то необходима очистка, но в ревите только для Transaction,SubTransaction и TransactionGroup
        using var deliteGroup = new TransactionGroup(document);
        deliteGroup.Start("Group modify");
        
        using var deliteSelecteion = new Transaction(document);
        /*
         Перед началом выполнения Transaction ей дайют название, чтобы в ревите появились кнопки, без них будет Exception, 
        существует 3 способа:transaction.SetName(), можно также указать его в самом методе Start transaction.Start(string name)или в конструкторе
         using var transaction = new Transaction(document,"name");
         */
        //Начало Transaction
        deliteSelecteion.Start("Modify");

        foreach (var reference in references) document.Delete(reference.ElementId);
        //transaction.RollBack() Обычно используется для временных транзакций, отменяет все изменения документа,
        //но чаще ипользуется метод Commit, он подтверждает изменение всех транзакций внутри
        deliteSelecteion.Commit();

        using var deliteWindows = new Transaction(document);
        deliteWindows.Start("Modify windows");

        var windows = new FilteredElementCollector(document)
            .WhereElementIsNotElementType()
            .OfCategory(BuiltInCategory.OST_Windows)
            .ToElementIds();

        document.Delete(windows);

        deliteWindows.Commit();
        //Объединяет все транзакции в одну
        deliteGroup.Assimilate();

        return Result.Succeeded;
    }
}