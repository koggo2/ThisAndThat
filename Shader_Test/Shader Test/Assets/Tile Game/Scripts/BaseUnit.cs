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
        public bool OnMarch = false;
        public Vector3Int MarchPosition;

        [SerializeField] private int _hp = 0;
        [SerializeField] private int _power = 0;

        public override void OnBeat_PostUpdateGrid()
        {
            base.OnBeat_PostUpdateGrid();
            
            if (OnMarch)
            {
                GameGrid.Instance.MoveUnit(this, MarchPosition);                
            }
        }
        
        public void SetTeam(TeamEnum team)
        {
            Team = team;
        }

        public void SetMarchPosition(Vector3Int pos)
        {
            OnMarch = true;
            MarchPosition = pos;
        }

        public void StopMarch()
        {
            OnMarch = false;
            MarchPosition = Vector3Int.zero;
        }

        public void Move(Vector3 position)
        {
            transform.position = position;
        }
    }
}

