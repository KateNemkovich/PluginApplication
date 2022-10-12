using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.ExtensibleStorage;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Nice3point.Revit.Extensions;

namespace PluginApplication.Command;

[Transaction(TransactionMode.Manual)]
public class ExtensibleStorageCommand : IExternalCommand
{
    public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
    {
        var builder = new SchemaBuilder(new Guid("6A86F4AE-751E-46A5-A6D3-C30770267B6F"))
            //Название 
            .SetSchemaName("Something")
            //Описание документации
            .SetDocumentation("Smth")
            //Владелец для прав пользователя
            .SetVendorId("Kate")
            .SetReadAccessLevel(AccessLevel.Public)
            .SetWriteAccessLevel(AccessLevel.Public);

        builder.AddSimpleField("fieldName", typeof(string))
            .SetDocumentation("documentation");
        var schema = builder.Finish();

        var uiDocument = commandData.Application.ActiveUIDocument;
        var document = uiDocument.Document;

        /*using var storageCommand = new Transaction(document);
        storageCommand.Start("Storage");
        
        var entity = new Entity(schema);
        entity.Set("fieldName", "fieldValue");
        document.ProjectInformation.SetEntity(entity);
        
        storageCommand.Commit();
        
        var entity1 = document.ProjectInformation.GetEntity(schema);
        var getEntity = entity1.Get<string>("fieldName");
        */

        using var storageCommand = new Transaction(document);
        storageCommand.Start("Storage");
        document.ProjectInformation.SaveEntity(schema, "Storage", "fieldName");
        storageCommand.Commit();

        var loadEntity = document.ProjectInformation.LoadEntity<string>(schema, "fieldName");
        return Result.Succeeded;
    }
}