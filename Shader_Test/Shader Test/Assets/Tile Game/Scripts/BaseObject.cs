using UnityEngine;

namespace TheTile.Game
{
    public class BaseObject : MonoBehaviour
    {
        public enum TeamEnum
        {
            NONE = 0,
            A,
            B,
        }

        public TeamEnum Team;
        
        public virtual void OnBeat_PreUpdateGrid() { }
        public virtual void OnBeat_PostUpdateGrid() { }
    }
}
