using System.IO;
using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace LayoutExporter
{
    public class LayoutImportWindow : EditorWindow
    {
        [SerializeField] private UnityEditor.DefaultAsset m_LayoutAsset;

        private const int MaxLayoutNameLength = 15;
        private string[] m_Paths;
        private Vector2 m_ScrollPos;

        [MenuItem(EditorSettings.LayoutImportWindow_MenuName, false, EditorSettings.LayoutImportWindow_MenuOrder)]
        static void OpenWindow()
        {
            GetWindow<LayoutImportWindow>(EditorSettings.LayoutImportWindow_WindowTitle);
        }

        void OnGUI()
        {
            if (m_Paths == null)
            {
                InitializePaths();
            }

            EditorGUILayout.LabelField(EditorSettings.LayoutImportWindow_Description);
            m_LayoutAsset = EditorGUILayout.ObjectField(m_LayoutAsset, typeof(UnityEditor.DefaultAsset), false) as UnityEditor.DefaultAsset;
            // レイアウトファイル(.wlt)をUnityEditorへ登録
            EditorGUI.BeginDisabledGroup(m_LayoutAsset == null);
            if (GUILayout.Button("Import"))
            {
                var filePath = Path.GetFullPath(
                    AssetDatabase.GetAssetPath(m_LayoutAsset)
                );
                var fileName = Path.GetFileName(filePath);

                var savePath = EditorUtility.SaveFilePanel(
                    "Save Layout File",
                    WindowLayout.LayoutsPreferencesPath,
                    fileName,
                    "wlt");

                if (!string.IsNullOrEmpty(savePath))
                {
                    var saveFullPath = Path.GetFullPath(savePath);
                    File.Copy(filePath, saveFullPath);

                    Debug.Log("Import: " + saveFullPath);
                }

                InternalEditorUtility.ReloadWindowLayoutMenu(); // レイアウトメニュー更新
            }
            EditorGUI.EndDisabledGroup();

            // ローカルに保存しているレイアウトファイル(.wlt)をUnityEditorへ登録
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(EditorSettings.LayoutImportWindow_Description);
            m_ScrollPos = EditorGUILayout.BeginScrollView(m_ScrollPos);
            foreach (string path in m_Paths)
            {
                string name = Path.GetFileNameWithoutExtension(path);
                if (name.Length > MaxLayoutNameLength)
                    name = name.Substring(0, MaxLayoutNameLength) + "...";

                if (GUILayout.Button(string.Format("{0}", name)))
                {
                    var fileName = Path.GetFileName(path);
                    var savePath = EditorUtility.SaveFilePanel(
                        "Save Layout File",
                        WindowLayout.LayoutsPreferencesPath,
                        fileName,
                        "wlt");

                    if (!string.IsNullOrEmpty(savePath))
                    {
                        var saveFullPath = Path.GetFullPath(savePath);
                        File.Copy(path, saveFullPath);

                        Debug.Log("Import: " + saveFullPath);
                    }

                    InternalEditorUtility.ReloadWindowLayoutMenu(); // レイアウトメニュー更新
                }
            }
            EditorGUILayout.EndScrollView();
        }

        private void InitializePaths()
        {
            string[] allPaths = Directory.GetFiles(WindowLayout.LayoutsExportedPath);
            ArrayList filteredFiles = new ArrayList();
            foreach (string path in allPaths)
            {
                string name = Path.GetFileName(path);
                if (Path.GetExtension(name) == ".wlt")
                {
                    filteredFiles.Add(path);
                }
            }
            m_Paths = filteredFiles.ToArray(typeof(string)) as string[];
        }
    }
}