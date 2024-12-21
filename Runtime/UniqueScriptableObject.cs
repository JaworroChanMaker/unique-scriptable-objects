namespace FoxCultGames.UniqueScriptableObjects
{
    using System;
    using UnityEditor;
    using UnityEngine;
    
    public class UniqueScriptableObject : ScriptableObject
    {
        [SerializeField] private string serializedGuid;

        public Guid Guid
        {
            get
            {
                if (guidCache == Guid.Empty)
                    guidCache = Guid.Parse(serializedGuid);

                return guidCache;
            }
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!string.IsNullOrEmpty(serializedGuid))
                return;
            
            var path = AssetDatabase.GetAssetPath(this);
            serializedGuid = AssetDatabase.AssetPathToGUID(path);
        }
#endif

        private Guid guidCache;
    }
}
