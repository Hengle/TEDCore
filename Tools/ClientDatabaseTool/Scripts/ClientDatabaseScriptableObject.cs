using UnityEngine;

namespace TEDCore.ClientDatabase
{
    public abstract class ClientDatabaseScriptableObject : ScriptableObject
    {
        public abstract void LoadTextAsset();
    }
}