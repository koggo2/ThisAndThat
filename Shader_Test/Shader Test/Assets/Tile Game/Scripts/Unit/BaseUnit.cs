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
        public HouseBasement OriginBasement { get; set; }

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

            OriginBasement.UnlinkGeneratedUnit(this);
            
            StopAllCoroutines();
            CancelInvoke();
        }

        public void March(AStarSearch aStar)
        {
            transform.parent = null;
            
            StopMarch();
            _moveCoroutine = StartCoroutine(Move(aStar));
        }

        public void StopMarch()
        {
            if (_moveCoroutine != null)
            {
                StopCoroutine(_moveCoroutine);
                _moveCoroutine = null;
            }
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

                if (gameObject == null)
                    yield break;
                
                var origin = transform.position;
                var worldDest = GameGrid.Instance.CellPosToWorld(dest);
                var t = 0f;

                GameController.Instance.DetachUnitOnTile(this);

                if (index != 0)
                {
                    var targetDirection = worldDest - origin;
                    var newDirection = Vector3.RotateTowards(transform.forward, targetDirection, Mathf.PI * 0.5f, 0.0f);
                    transform.rotation = Quaternion.LookRotation(newDirection);                    
                }
                
                while (t < 1f)
                {
                    yield return CheckNextTile(dest);
                    
                    transform.position = Vector3.Lerp(origin, worldDest, t / 1f);
                    // Debug.Log($"Move : t = {t}, position = {transform.position}");
                    yield return null;
                    
                    t += Time.deltaTime;
                }

                GameController.Instance.AttachUnitOnTile(dest, this);
            }
            if(_animator != null)
                _animator.SetBool(ConstData.Animation_Parameter_Moving, false);
        }

        private IEnumerator CheckNextTile(Vector3Int nextCellPos)
        {
            var nextTileData = GameGrid.Instance.GetTileData(nextCellPos);
            while ((nextTileData.Basement != null && Team != nextTileData.Basement.Team) || (nextTileData.Unit != null && Team != nextTileData.Unit.Team))
            {
                GameController.Instance.Battle(GameGrid.Instance.WorldToCellPos(transform.position), nextCellPos);
                yield return null;
            }
        }

        public virtual void Attack()
        {
            if(_animator != null)
            {
                _animator.SetBool(ConstData.Animation_Parameter_Moving, false);
                _animator.Play(ConstData.Animation_Attack1);
            }
        }
    }
}

