using UnityEngine;

namespace TheTile.Game
{
    public partial class GameController : Singleton<GameController>
    {
        public struct BuildData
        {
            public Vector3Int OriginPos;
            public Vector3Int TargetPos;
            public string PrefabName;
            public bool NeedConstruction;
            public string NextBasementName;
        }
        
    }
}