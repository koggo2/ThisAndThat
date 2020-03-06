using UnityEngine;

namespace TheTile.Util
{
    public static class ResourceManager
    {
        public static GameObject NewTile(string prefabName, Transform parent)
        {
            var prefab = Resources.Load<GameObject>($"Tiles/{prefabName}");
            var instance = GameObject.Instantiate(prefab, parent, true);
            instance.transform.localPosition = Vector3.zero;
            instance.name = prefabName;

            return instance;
        }
    }
}