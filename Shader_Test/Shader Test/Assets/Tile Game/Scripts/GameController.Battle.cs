
using TheTile.Game.Unit;
using UnityEngine;

namespace TheTile.Game
{
    public partial class GameController
    {
        public void DetachUnitOnTile(BaseUnit unit)
        {
            GameGrid.Instance.DetachUnit(unit);
        }

        public void AttachUnitOnTile(Vector3Int pos, BaseUnit unit)
        {
            GameGrid.Instance.AttachUnit(pos, unit, false);
        }
        
        public void Battle(Vector3Int pos, Vector3Int targetPos)
        {
            var originTileData = GameGrid.Instance.GetTileData(pos);
            var targetTileData = GameGrid.Instance.GetTileData(targetPos);
            if (originTileData == null || targetTileData == null)
                return;

            if (originTileData.Unit == null)
                return;

            if (targetTileData.Unit != null)
            {
                if (originTileData.Unit.Team == targetTileData.Unit.Team)
                {
                    originTileData.Unit.Hp += targetTileData.Unit.Hp;
                    targetTileData.Unit.StopMarch();
                    Destroy(targetTileData.Unit.gameObject);
                }
                else
                {
                    // while (originTileData.Unit.Hp > 0 && targetTileData.Unit.Hp > 0)
                    // {
                    //     originTileData.Unit.Attack();
                    //     originTileData.Unit.Hp -= targetTileData.Unit.Power;
                    //     targetTileData.Unit.Attack();
                    //     targetTileData.Unit.Hp -= originTileData.Unit.Power;                                
                    // }
                    
                    originTileData.Unit.Attack();
                    originTileData.Unit.Hp -= targetTileData.Unit.Power;
                    targetTileData.Unit.Attack();
                    targetTileData.Unit.Hp -= originTileData.Unit.Power;

                    if (originTileData.Unit.Hp <= 0)
                    {
                        originTileData.Unit.StopMarch();
                        originTileData.RemoveUnit();
                    }

                    if (targetTileData.Unit.Hp <= 0)
                    {
                        targetTileData.Unit.StopMarch();
                        targetTileData.RemoveUnit();
                    }
                }
            }
            else // No Unit, Just Basement existed.
            {
                if (originTileData.Unit.Team == targetTileData.Basement.Team)
                    return;

                targetTileData.Basement.Hp -= originTileData.Unit.Power;

                if (targetTileData.Basement.Hp <= 0)
                {
                    targetTileData.RemoveBasement();
                }
            }
        }
    }
}