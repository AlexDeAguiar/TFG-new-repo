﻿using AnotherFileBrowser.Windows;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FileBrowserUpdate {
    public static void OpenFileBrowser(Action<string[]> result, string[][] extensions, int filterIndex, bool includeAllFilesOption){
        var bp = new BrowserProperties(Translator._INTL("Choose a File"));
        //bp.initialDir(path) -> where dialog should be opened initially
        bp.filter = getFilterString(extensions,includeAllFilesOption);
        bp.filterIndex = filterIndex;


        //OpenMultiSelectFileBrowser
        //OpenFolderBrowser
        //SaveFileBrowser
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
        new FileBrowser().OpenFileBrowser(bp, paths => result(paths));
    }

    private static string getFilterString(string[][] extensions, bool includeAllFilesOption){
        string filterStr = "";
        for (int i = 0; i < extensions.Length; i++){
            filterStr += Translator._INTL(extensions[i][0]);

            string part1 = "";
            int length = extensions[i].Length;
            for (int j = 1; j < length; j++){
				part1 += "*" + extensions[i][j] + ((j + 1 < length) ? ";" : "");
			}

            filterStr += "|" + part1 + ((i + 1 < extensions.Length) ? "|" : "");
        }

        if(includeAllFilesOption){ filterStr += "|All files (*.*)|*.*"; }
        Debug.Log(filterStr);
        return filterStr;
    }
}
