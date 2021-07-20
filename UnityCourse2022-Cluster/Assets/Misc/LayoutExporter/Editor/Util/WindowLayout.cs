using System.IO;
using UnityEditorInternal;
using UnityEngine;

namespace LayoutExporter
{
    internal class WindowLayout
    {
        public static readonly string LayoutsPreferencesPath = Path.Combine(InternalEditorUtility.unityPreferencesFolder, "Layouts", "default");
        public static readonly string LayoutsExportedPath = Path.Combine(Application.dataPath, "Misc", "LayoutExporter", "wlt");
    }
}