
using System.Collections.Generic;
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

        public List<BaseUnit> TemporaryUnit;
        

        public TileData()
        {
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            Unit = null;
            Basement = null;
            TemporaryUnit = new List<BaseUnit>();
        }

        public void Update()
        {
            if (Unit != null)
            {
                GameObject.DestroyImmediate(Unit.gameObject);
                Unit = null;
            }
            
            if (Tile != null && TemporaryUnit != null && TemporaryUnit.Count > 0)
            {
                Unit = TemporaryUnit[0];

                for (var i = 1; i < TemporaryUnit.Count; ++i)
                {
                    var nextUnit = TemporaryUnit[i];
                    if (Unit == null)
                    {
                        Unit = nextUnit;
                    }
                    else
                    {
                        if (Unit.Team == nextUnit.Team)
                        {
                            Unit.Hp += nextUnit.Hp;
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

            if (Unit != null)
            {
                if (Unit.Team != Tile.Team)
                {
                    var parent = Tile.transform.parent;
                    var pos = Tile.transform.localPosition;
                    var pivotPos = Tile.Pivot.localPosition;
                    
                    GameObject.DestroyImmediate(Tile.gameObject);
                    var emptyTilePrefab = Resources.Load<GameObject>("Empty Tile");
                    var emptyTileInst = GameObject.Instantiate(emptyTilePrefab);

                    emptyTileInst.transform.parent = parent;
                    emptyTileInst.transform.localPosition = pos;
                    emptyTileInst.transform.localRotation = Quaternion.identity;
                    emptyTileInst.transform.localScale = Vector3.one;


                    var emptyTile = emptyTileInst.GetComponent<EmptyTile>();
                    emptyTile.Pivot.localPosition = pivotPos;
                    Tile = emptyTile;
                }
                
                Tile.SetTeam(Unit.Team);
            }
        }

        public void AddUnit(BaseUnit unit)
        {
            TemporaryUnit.Add(unit);
        }

        public void MoveUnit(Vector3 getCellCenterWorld)
        {
            Unit.Move(getCellCenterWorld);
            Unit = null;
        }

        public void GenerateUnit(Grid _grid, Transform transform)
        {
            if (Basement != null)
            {
                var baseUnit = Basement.GenerateUnit(transform);
                TemporaryUnit.Add(baseUnit);
            }
        }
    }
}