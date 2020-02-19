using System.Collections;
using TheTile.UI;
using UnityEngine;

namespace TheTile.Game.Unit
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
        [SerializeField] private int _speed = 0;
        [SerializeField] private Animator _animator;

        private AStarSearch _aStar;
        private Coroutine _moveCoroutine;

        private void Start()
        {
            UIManager.Instance.RegisterUnitInfoUI(this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
        }

        public void March(AStarSearch aStar)
        {
            transform.parent = null;
            _moveCoroutine = StartCoroutine(Move(aStar));
        }

        private IEnumerator Move(AStarSearch aStar)
        {
            if(_animator != null)
            {
                _animator.Play(ConstData.Animation_Run);
                _animator.SetBool(ConstData.Animation_Parameter_Moving, true);
            }
            
            var index = -1;
            foreach (var dest in aStar.Path)
            {
                ++index;
                
                var origin = transform.position;
                var worldDest = GameGrid.Instance.CellPosToWorld(dest);
                var t = 0f;

                // Debug.Log($"Move : origin = {origin}, dest = {dest}, worldDest = {worldDest}");
                
                DetachUnitOnTile();

                if (index != 0)
                {
                    var targetDirection = worldDest - origin;
                    var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Mathf.PI * 0.5f, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);                    
                }
                
                while (t < 1f)
                {
                    transform.position = Vector3.Lerp(origin, worldDest, t / 1f);
                    // Debug.Log($"Move : t = {t}, position = {transform.position}");
                    yield return null;
                    
                    t += Time.deltaTime;
                }

                AttachUnitOnTile();
            }
            if(_animator != null)
                _animator.SetBool(ConstData.Animation_Parameter_Moving, false);
        }

        private void DetachUnitOnTile()
        {
            var tileData = GameGrid.Instance.GetUnderTileData(this);
            if (tileData != null)
            {
                GameGrid.Instance.DetachUnit(tileData.Pos);
            }
        }

        private void AttachUnitOnTile()
        {
            var tileData = GameGrid.Instance.GetUnderTileData(this);
            if (tileData != null)
            {
                GameGrid.Instance.AttachUnit(tileData.Pos, this, false);
            }
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

