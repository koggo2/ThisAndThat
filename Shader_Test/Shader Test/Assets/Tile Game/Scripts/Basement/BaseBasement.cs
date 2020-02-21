using TheTile.UI;
using TheTile.Util;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseBasement : BaseObject
    {
        [SerializeField] protected int _constructionValue;
        [SerializeField] protected int _hp;

        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }
        public int ConstructionValue => _constructionValue;

        protected virtual void Awake()
        {
            _hp = _constructionValue;
        }

        protected virtual void Start()
        {
            UIManager.Instance.RegisterBasementInfoUI(this);
        }
    }
}
