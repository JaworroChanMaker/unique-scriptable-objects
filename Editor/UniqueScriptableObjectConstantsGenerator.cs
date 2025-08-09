namespace FoxCultGames.UniqueScriptableObjects.Editor
{
    using System.Linq;
    using UnityEditor;

    public static class UniqueScriptableObjectConstantsGenerator
    {
        [MenuItem("Tools/Generate Unique SO Constants")]
        public static void Generate()
        {
            var builder = new System.Text.StringBuilder();
            builder.AppendFormat(FileHeader, System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss"));
            builder.AppendFormat("namespace {0}.Constants\n", EditorSettings.projectGenerationRootNamespace);
            builder.AppendLine(SymbolContent);
            
            var grouping = AssetDatabase.FindAssets("t:UniqueScriptableObject")
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<UniqueScriptableObject>)
                .GroupBy(asset => asset.GetType());
            
            foreach (var entry in grouping)
            {
                builder.AppendLine($"\t\tpublic static class {entry.Key.Name}");
                builder.AppendLine("\t\t{");
                
                foreach (var asset in entry)
                {
                    builder.AppendLine($"\t\t\tpublic static readonly Guid {asset.name} = Guid.Parse(\"{asset.Guid}\");");
                }
                
                builder.AppendLine("\t\t}");
            }
            
            builder.AppendLine("\t}");
            builder.AppendLine("}");
            
            System.IO.File.WriteAllText(OutputPath, builder.ToString());
            AssetDatabase.Refresh();
            UnityEngine.Debug.Log("Unique Scriptable Object constants generated successfully.");
        }
        
        private const string OutputPath = "Assets/Scripts/Constants/USO.cs";
        private const string FileHeader =
            "// This file is auto-generated. Do not modify it manually.\n" +
            "// Generated on: {0}\n";
        private const string SymbolContent = 
            "{\n" +
            "\tusing System;\n\n" +
            "\tpublic static class USO\n" +
            "\t{";
    }
}