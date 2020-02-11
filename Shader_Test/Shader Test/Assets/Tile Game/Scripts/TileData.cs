
using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class TileData
    {
        public BaseObject.TeamEnum Team;
        public BaseTile Tile;
        public BaseUnit Unit;
        public BaseBasement Basement;

        public Queue<BaseUnit> TemporaryUnit;
        

        public TileData()
        {
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            Unit = null;
            Basement = null;
            TemporaryUnit = new Queue<BaseUnit>();
        }

        public void Update()
        {
            if (Tile != null && TemporaryUnit != null && TemporaryUnit.Count > 0)
            {
                while (TemporaryUnit.Count > 0)
                {
                    var nextUnit = TemporaryUnit.Dequeue();
                    if (Unit == null)
                    {
                        Unit = nextUnit;
                    }
                    else
                    {
                        if (Unit.Team == nextUnit.Team)
                        {
                            Unit.Hp += nextUnit.Hp;
                            GameObject.DestroyImmediate(nextUnit.gameObject);
                        }
                        else
                        {
                            while (Unit.Hp > 0 && nextUnit.Hp > 0)
                            {
                                Unit.Hp -= nextUnit.Power;
                                nextUnit.Hp -= nextUnit.Hp - Unit.Power;                                
                            }

                            if (Unit.Hp <= 0)
                            {
                                GameObject.DestroyImmediate(Unit.gameObject);
                                Unit = null;
                            }

                            if (nextUnit.Hp <= 0)
                            {
                                GameObject.DestroyImmediate(nextUnit.gameObject);
                                nextUnit = null;
                            }

                            if (Unit == null && nextUnit != null)
                            {
                                Unit = nextUnit;
                            }
                        }
                    }
                }
                
                TemporaryUnit.Clear();
            }

            // if (Unit != null)
            // {
            //     if (Unit.Team != Tile.Team)
            //     {
            //         var parent = Tile.transform.parent;
            //         var pos = Tile.transform.localPosition;
            //         var pivotPos = Tile.Pivot.localPosition;
            //         
            //         GameObject.DestroyImmediate(Tile.gameObject);
            //         var emptyTilePrefab = Resources.Load<GameObject>("Empty Tile");
            //         var emptyTileInst = GameObject.Instantiate(emptyTilePrefab);
            //
            //         emptyTileInst.transform.parent = parent;
            //         emptyTileInst.transform.localPosition = pos;
            //         emptyTileInst.transform.localRotation = Quaternion.identity;
            //         emptyTileInst.transform.localScale = Vector3.one;
            //
            //
            //         var emptyTile = emptyTileInst.GetComponent<EmptyTile>();
            //         emptyTile.Pivot.localPosition = pivotPos;
            //         Tile = emptyTile;
            //     }
            //     
            //     Tile.SetTeam(Unit.Team);
            // }
        }
    }
}