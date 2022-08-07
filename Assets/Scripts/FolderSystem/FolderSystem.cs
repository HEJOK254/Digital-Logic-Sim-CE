using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FolderSystem
{
    private static bool Inizialized = false;
    public static IEnumerable<KeyValuePair<int, string>> Enum => Folders.AsEnumerable();

    public static Dictionary<int, string> DefaultFolder
    {
        get => new Dictionary<int, string>()
        {
            { 0, "Comp" },
            { 1, "Gate" },
            { 2, "Misc"}
        };
    }

    private static Dictionary<int, string> Folders;


    public static void Init()
    {
        Folders = new Dictionary<int, string>(DefaultFolder);

        foreach (var kv in SaveSystem.LoadCustomFolders())
            Folders.TryAdd(kv.Key, kv.Value);

        Inizialized = true;
    }
    public static void Reset()
    {
        Folders = null;
        Inizialized = false;
    }
    public static int ReverseIndex(string DicValue) => Inizialized ? Folders.FirstOrDefault(x => x.Value == DicValue).Key : -1;

    public static bool ContainsIndex(int i) => Inizialized ? Folders.ContainsKey(i) : false;

    public static void AddFolder(string newFolderName)
    {
        if (!Inizialized) return;

        Folders[Folders.Count] = newFolderName;
        SaveSystem.SaveCustomFolders(Folders);
    }

    internal static void DeleteFolder(string folderName)
    {
        if (!Inizialized) return;

        DeleteFolder(ReverseIndex(folderName));
    }
    public static void DeleteFolder(int Index)
    {
        if (!Inizialized) return;
        Folders.Remove(Index);
    }

    public static bool FolderNameAvailable(string name)
    {
        if (!Inizialized) return false;

        foreach (string f in Folders.Values)
        {
            if (string.Equals(name.ToUpper(), f.ToUpper()))
                return false;
        }
        return true;
    }





    public static bool CompareValue(int index, string value) => Inizialized && ContainsIndex(index) && string.Equals(Folders[index], value);

    public static void RenameFolder(string OldFolderName, string NewFolderName)
    {
        if (!Inizialized) return;

        if (!Folders.ContainsValue(OldFolderName)
            || string.Equals(OldFolderName, NewFolderName))
            return;

        var index = ReverseIndex(OldFolderName);
        Folders[index] = NewFolderName;

        SaveSystem.SaveCustomFolders(Folders);
    }

}