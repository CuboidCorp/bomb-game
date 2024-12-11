using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;

public static class TextLocalizationHandler
{
    public static void Setup()
    {
        LocalizationSettings.SelectedLocaleChanged += OnSelectedLocaleChanged;
    }

    private static void OnSelectedLocaleChanged(Locale obj)
    {
        Debug.Log("Locale changed to: " + obj.Identifier.Code);
        //TODO : Implement the logic to update the text
    }

    public static string LoadString(string tableCollectionName, string entryName)
    {
        var loadingOperation = LocalizationSettings.StringDatabase.GetTableAsync(tableCollectionName);
        loadingOperation.WaitForCompletion();

        if (loadingOperation.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            var stringTable = loadingOperation.Result;
            return GetLocalizedString(stringTable, entryName);
        }
        else
        {
            Debug.LogError("Could not load String Table\n" + loadingOperation.OperationException.ToString());
            return string.Empty;
        }
    }

    static string GetLocalizedString(StringTable table, string entryName)
    {
        StringTableEntry entry = table.GetEntry(entryName);
        return entry.GetLocalizedString();
    }

    public static LocalizedString GetSmartString(string tableCollectionName, string entryName)
    {
        return new LocalizedString { TableReference = tableCollectionName, TableEntryReference = entryName };
    }

}
