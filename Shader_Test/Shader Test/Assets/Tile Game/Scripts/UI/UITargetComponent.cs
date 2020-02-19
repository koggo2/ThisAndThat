using TheTile.Game;
using UnityEngine;

namespace TheTile.UI
{
    public class UITargetComponent<T> : UIBaseComponent where T : BaseObject
    {
        public T Target => _target as T;
        [SerializeField] private T _target;
        
        public void Init(T targetObject)
        {
            _target = targetObject;
        }
    }
}

