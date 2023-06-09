﻿//(c) 2021 Movinarc
//http://movinarc.com
using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Win32;
using System.IO.Compression;
using System.Reflection;
using SharpCompress;
using SharpCompress.Reader;
using SharpCompress.Common;
using System.Collections;
using System.ComponentModel;

namespace Movinarc
{
    public class PUManager : EditorWindow
    {
        Vector2 scrollPos = Vector2.zero;
        Texture2D putitle = null;
        Texture2D puhelp = null;
        Texture2D pudel = null;
        Texture2D line = null;
        Texture2D imgInfo = null;
        Texture2D openicon = null;
        string customPackage = string.Empty;
        bool hasError = false;
        GUIStyle horAlignStyle = null;
        GUIStyle fontStyle = null;
        GUIStyle[] searchStyles = null;
        string searchText = "";
        List<Package> packages = null;
        List<Package> assetStoreList = null;
        Rect rectScrollView;
        double lastTime = 0;
        bool blinkOn = true;
        string unityPath = "";
        string infoToShow = "";
        Color textColor { get { return EditorGUIUtility.isProSkin ? Color.white : Color.black; } }
        Color infoTextColor { get { return EditorGUIUtility.isProSkin ? new Color(.6f, .6f, .6f) : Color.gray; } }
        const string websiteUrl = "http://movinarc.com/unity-package-uninstaller";
        GUILayoutOption[] buttonSize = { GUILayout.Width(32f), GUILayout.Height(32f) };
        GUILayoutOption[] infoButtonSize = { GUILayout.Width(24f), GUILayout.Height(24f) };

        struct Package
        {
            public string fullPath;
            public string fileName;
            public string dateString;
            public DateTime date;
        }

        public class AssetPath
        {
            public string name = "";
            public string filePath = "";
            public bool isDirectory = false;
        }

        [MenuItem("Window/Package Uninstaller")]
        static void CreateWindow()
        {
            var win = EditorWindow.GetWindow(typeof(PUManager));
            win.minSize = new Vector2(410, 450);
            win.titleContent.text = "Package Uninstaller";
            win.titleContent.tooltip = "v2.0";
            win.Focus();
        }

        private void OnEnable()
        {
            putitle = Resources.Load("PU_title") as Texture2D;
            puhelp = Resources.Load("pu_help") as Texture2D;
            pudel = Resources.Load("PU_del") as Texture2D;
            line = Resources.Load("pu_line") as Texture2D;
            imgInfo = Resources.Load("pu_info") as Texture2D;
            openicon = Resources.Load("pu_open") as Texture2D;
        }

        public void Update()
        {
            if (EditorApplication.timeSinceStartup - lastTime > .25)
            {
                blinkOn = !blinkOn;
                lastTime = EditorApplication.timeSinceStartup;
                Repaint();
            }
        }

        public void OnGUI()
        {
            #region init
            if (packages == null || packages.Count <= 0)
            {
                TreeView tv = CreateInstance<TreeView>();
                tv.allCheckedByDefault = true;
                packages = new List<Package>();
                infoToShow = "";
                try
                {
#if UNITY_EDITOR_WIN
                unityPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                unityPath = Path.Combine(unityPath, "Unity");
#elif UNITY_EDITOR_OSX
                    unityPath = System.Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "/Library/Unity";
#endif
                    var info = new DirectoryInfo(unityPath);
                    var files = info.GetFiles("*.unitypackage", SearchOption.AllDirectories);


                    foreach (var file in files)
                    {
                        if (file.DirectoryName.Contains("cache")) continue;
                        packages.Add(new Package()
                        {
                            fileName = Path.GetFileNameWithoutExtension(file.FullName),
                            fullPath = file.FullName,
                            date = file.CreationTime,
                            dateString = file.CreationTime.ToString("g")
                        });
                    }
                    packages = packages.OrderBy(i => i.fileName).ToList();
                    assetStoreList = new List<Package>(packages);
                }
                catch
                {
                    if (!hasError)
                        Debug.LogError("Package Uninstaller could not locate the Unity folder on your disk! Please check whether your editor has been installed correctly and then try again.\nFor troubleshooting visit https://forum.unity3d.com/threads/unity-package-uninstaller.378829/");
                    hasError = true;
                }
                horAlignStyle = new GUIStyle();
                horAlignStyle.padding = new RectOffset(0, 5, 15, 0);
                horAlignStyle.normal.textColor = textColor;

                fontStyle = new GUIStyle();
                fontStyle.fontSize = 9;
                fontStyle.normal.textColor = infoTextColor;
                fontStyle.padding = new RectOffset(5, 0, 5, 0);
                fontStyle.wordWrap = true;


                searchStyles = new GUIStyle[3];
                searchText = "";
            }
            #endregion

            GUI.skin.label.alignment = TextAnchor.MiddleLeft;

            #region Header
            GUILayout.BeginHorizontal();
            GUILayout.Label(putitle, GUILayout.Width(260), GUILayout.Height(40));
            EditorGUILayout.Space();
            if (GUILayout.Button(new GUIContent(puhelp, "Help"), GUILayout.Width(32), GUILayout.Height(40)))
            {
                Application.OpenURL(websiteUrl);
            }
            GUILayout.EndHorizontal();
            #endregion

            #region Search/Select
            searchStyles[0] = GUI.skin.FindStyle("Toolbar");
            searchStyles[1] = GUI.skin.FindStyle("ToolbarSeachTextField");
            searchStyles[2] = GUI.skin.FindStyle("ToolbarSeachCancelButton");

            GUILayout.BeginHorizontal();
            GUILayout.Box("Search / Select ", GUILayout.ExpandWidth(true));
            var qrect = GUILayoutUtility.GetLastRect();
            if (qrect.Contains(Event.current.mousePosition))
            {
                infoToShow = "[Search / Select] \nType the name of the package you are looking for to filter the list of packages.";
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal(searchStyles[0]);
            GUI.SetNextControlName("txtSearch");
            searchText = GUILayout.TextField(searchText, searchStyles[1]);

            if (GUILayout.Button("", searchStyles[2]))
            {
                searchText = "";
                GUI.FocusControl(null);
            }

            GUI.FocusControl("txtSearch");
            GUILayout.EndHorizontal();

            GUILayout.Space(15.0f);
            scrollPos = GUILayout.BeginScrollView(scrollPos, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            GUILayout.BeginVertical();

            if (assetStoreList != null && assetStoreList.Count > 0)
            {
                foreach (var item in assetStoreList)
                {
                    GUILayout.BeginHorizontal();
                    if (GUILayout.Button(new GUIContent(pudel, "Uninstall"), buttonSize))
                    {
                        CheckUninstall(item.fullPath);
                    }
                    var irect = GUILayoutUtility.GetLastRect();
                    irect.width = position.width;
                    if (irect.Contains(Event.current.mousePosition) && rectScrollView.Contains(Event.current.mousePosition))
                    {
                        infoToShow = String.Format("[Name] {0}\n[Path] {1}\n[Downladed] {2}", item.fileName, item.fullPath, item.dateString);
                    }
                    GUILayout.Label(item.fileName, this.horAlignStyle, GUILayout.MinWidth(230));


                    GUILayout.EndHorizontal();
                    GUILayout.Space(5.0f);
                    GUILayout.Label(line);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndScrollView();
            if (Event.current.type == EventType.Repaint)
            {
                rectScrollView = GUILayoutUtility.GetLastRect();
                rectScrollView.x = scrollPos.x;
                rectScrollView.y = scrollPos.y;
            }
            #endregion

            GUILayout.Space(10.0f);

            #region Browse
            GUILayout.BeginHorizontal();
            GUILayout.Box("Browse ", GUILayout.ExpandWidth(true));
            qrect = GUILayoutUtility.GetLastRect();
            if (qrect.Contains(Event.current.mousePosition))
            {
                infoToShow = "[Browse] \nClick Open to select a unity package file, from your computer. Then click uninstall to unimport the package.";
            }
            GUILayout.EndHorizontal();
            GUILayout.BeginHorizontal();

            if (GUILayout.Button(new GUIContent(openicon, "Browse..."), buttonSize)) // Custom Open Button
            {
                var f = EditorUtility.OpenFilePanel("Select the .unitypackage file you are going to remove from project", "", "unitypackage");
                if (System.IO.File.Exists(f))
                {
                    this.customPackage = f;
                }
            }
            if (string.IsNullOrEmpty(customPackage))
                GUI.enabled = false;
            else
                GUI.enabled = true;
            if (GUILayout.Button(new GUIContent(pudel, "Uninstall"), buttonSize)) // Custom Uninstall Button
            {
                CheckUninstall(this.customPackage);
            }
            GUI.enabled = true;
            GUILayout.BeginVertical();
            if (string.IsNullOrEmpty(customPackage))
                GUILayout.Label("---", horAlignStyle);
            else
            {
                FileInfo fi = new FileInfo(customPackage);
                var pkgName = Path.GetFileNameWithoutExtension(fi.FullName);
                GUILayout.BeginHorizontal();
                GUILayout.Label(pkgName, this.horAlignStyle, GUILayout.ExpandWidth(true));
                GUILayout.EndHorizontal();
                var irect = GUILayoutUtility.GetLastRect();
                if (irect.Contains(Event.current.mousePosition))
                {
                    infoToShow = String.Format("[Name] {0}\n[Path] {1}\n[Downladed] {2}", pkgName, fi.FullName, fi.CreationTime.ToString("g"));
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            #endregion

            GUILayout.Space(10.0f);
            GUILayout.Label(line, GUILayout.ExpandWidth(true));

            if (GUI.changed)
            {
                FilterAssetStoreList(searchText);
            }
            GUILayout.BeginHorizontal();
            GUILayout.Label(imgInfo, infoButtonSize);

            if (!String.IsNullOrEmpty(infoToShow))
            {
                GUILayout.Label(infoToShow, fontStyle, GUILayout.Height(70), GUILayout.ExpandWidth(true));
            }
            else
            {
                GUILayout.Label("", fontStyle, GUILayout.Height(70), GUILayout.ExpandWidth(true));
            }
            GUILayout.EndHorizontal();

        }

        void FilterAssetStoreList(string what)
        {
            if (packages != null)
                assetStoreList = packages.FindAll(x => x.fileName.ToLowerInvariant().Contains(what.ToLowerInvariant()));
        }

        public void PrepareForUnimport(string file2import, string tempPath)
        {
            // test absolute
            if (!File.Exists(file2import))
            {
                // test relative
                file2import = Path.Combine(Environment.CurrentDirectory, file2import);
                if (!File.Exists(file2import))
                    throw new FileNotFoundException(file2import);
            }

            if (!file2import.ToLower().EndsWith(".unitypackage"))
                throw new Exception("You must select *.unitypackage files.");

            if (!Directory.Exists(tempPath))
                Directory.CreateDirectory(tempPath);

            EditorUtility.DisplayProgressBar("Uninstalling Package", "Initializing...", .25f);
            HolyGZip(file2import, tempPath);

            EditorUtility.DisplayProgressBar("Uninstalling Package", "Fetching Package Structure...", .5f);
            List<AssetPath> fileList = GenerateAssetList(tempPath);


            var assetList = AssetDatabase.GetAllAssetPaths().ToList();
            var foundList = new List<string>();
            foreach (var item in assetList)
            {
                if (fileList.Exists((f) => f.filePath.Equals(item, StringComparison.OrdinalIgnoreCase)))
                {
                    foundList.Add(item);
                }
            }
            var dirs = PUSelection.DirectoriesPathList(fileList);

            EditorUtility.ClearProgressBar();
            bool goOn = true;
            if (foundList.Count <= 0)
            {
                if (!EditorUtility.DisplayDialog("No files found.", "It seems that '" + Path.GetFileNameWithoutExtension(file2import) + "' doesn't exist in your project. Do you want to continue anyway?", "Continue Anyway", "No"))
                {
                    goOn = false;
                    PUSelection.RemoveMess(tempPath);
                }
            }
            if (goOn)
            {
                PUSelection pus = ScriptableObject.CreateInstance<PUSelection>();
                pus.fileList = fileList;
                pus.directories = dirs;
                pus.foundList = foundList;
                pus.packageName = Path.GetFileNameWithoutExtension(file2import);
                pus.packagePath = file2import;
                pus.tempPath = tempPath;
                pus.titleContent = new GUIContent("");
                pus.minSize = new Vector2(300, 200);

                pus.ShowUtility();
            }
        }

        void CheckUninstall(string customPackage)
        {
            try
            {

                UninstallPackage(customPackage);

            }
            catch (Exception ex)
            {
                EditorUtility.ClearProgressBar();
                Debug.LogError(ex.Message);
            }
        }

        public void UninstallPackage(string path)
        {
            string tempPath = Path.Combine(Path.GetTempPath(), "PU" + DateTime.Now.Ticks.ToString());

            PrepareForUnimport(path, tempPath);
        }

        public void HolyGZip(string gzipFileName, string targetDir)
        {
            using (Stream stream = File.OpenRead(gzipFileName))
            {
                var reader = ReaderFactory.Open(stream);
                var progress = .25f;

                while (reader.MoveToNextEntry())
                {
                    if (!reader.Entry.IsDirectory)
                    {
                        reader.WriteEntryToDirectory(targetDir, ExtractOptions.ExtractFullPath | ExtractOptions.Overwrite);
                        progress += .75f * reader.Entry.Size / stream.Length;
                        EditorUtility.DisplayProgressBar("Uninstalling Package", "Analysing... ", progress);

                    }
                }
            }
        }

        private List<AssetPath> GenerateAssetList(string contentPath)
        {
            string appPath = Application.dataPath.Substring(0, Application.dataPath.LastIndexOf(@"Assets"));
            var lst = new List<AssetPath>();
            var directoryInfo = new DirectoryInfo(contentPath).GetDirectories();
            foreach (var item in directoryInfo)
            {
                string pathnameFromFile = File.ReadAllLines(Path.Combine(item.FullName, "pathname")).ToList().First();
                //ensure path correction
                pathnameFromFile = pathnameFromFile.Replace(@"\\", "/");
                pathnameFromFile = pathnameFromFile.Replace(@"\", "/");

                var asset = new AssetPath();
                if (Directory.Exists(Path.Combine(appPath, pathnameFromFile)))
                {
                    asset.name = Path.GetDirectoryName(pathnameFromFile);

                    if (!pathnameFromFile.EndsWith("/"))
                    {
                        pathnameFromFile += "/";
                    }
                }
                else
                {
                    asset.name = Path.GetFileName(pathnameFromFile);
                }
                asset.filePath = pathnameFromFile;

                lst.Add(asset);
            }
            var ordered = lst.OrderByDescending(i => i.filePath.Count(x => x == '/'));

            return ordered.ToList();
        }
    }
}