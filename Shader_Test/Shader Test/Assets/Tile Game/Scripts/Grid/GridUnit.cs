using TheTile.Game;
using TheTile.Util;
using UnityEngine;

namespace TheTile
{
    public class GridUnit : MonoBehaviour
    {

        [SerializeField] private GameObject _tile;
        [SerializeField] private GameObject _basement;
        
#if UNITY_EDITOR
        public void CreateTile(string prefabName)
        {
            ClearTile();

            _tile = ResourceManager.NewTile(prefabName, transform);
            var baseTile = _tile.GetComponent<BaseTile>();
            if (baseTile != null)
            {
                baseTile.UpdatePivotScale();
            }
        }

        public void CreateBasement(string prefabName)
        {
            if (_basement != null)
            {
                DestroyImmediate(_basement);
                _basement = null;
            }
            
            var baseTile = _tile.GetComponent<BaseTile>();
            if (baseTile == null)
                return;
            
            var prefab = Resources.Load<GameObject>(prefabName);
            var instance = Instantiate(prefab);
            instance.transform.SetParent(baseTile.Pivot);
            instance.transform.localPosition = Vector3.zero;
            instance.name = prefabName;

            _basement = instance;
        }
        
        public void SetTeam(BaseObject.TeamEnum team)
        {
            _tile?.GetComponent<BaseObject>().SetTeam(team);
            _basement?.GetComponent<BaseObject>().SetTeam(team);
        }
        
        public void ClearTile()
        {
            if (_tile != null)
            {
                DestroyImmediate(_tile);
                _tile = null;
                _basement = null;
            }
        }
#endif
    }
}
