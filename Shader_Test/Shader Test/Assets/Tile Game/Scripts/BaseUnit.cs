using UnityEngine;

namespace TheTile.Game
{
    public class BaseUnit : BaseObject
    {
        public int Hp
        {
            get => _hp;
            set => _hp = value;
        }

        public int Power => _power;

        [SerializeField] private int _hp = 0;
        [SerializeField] private int _power = 0;
        
        public void SetTeam(TeamEnum team)
        {
            Team = team;
            
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Const.GetTeamColor(Team);
            }
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}

