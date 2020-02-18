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

        public TeamEnum Team => _team;
        [SerializeField] protected TeamEnum _team;
        
        public virtual void OnBeat_PreUpdateGrid() { }
        public virtual void OnBeat_PostUpdateGrid() { }

        public virtual void SetTeam(TeamEnum team)
        {
            _team = team;
        }
    }
}
