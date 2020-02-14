
using System.Collections.Generic;
using System.Linq;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public class TileData
    {
        public Vector3Int Pos;
        public BaseObject.TeamEnum Team;
        public BaseTile Tile;
        public BaseUnit Unit;
        public BaseBasement Basement;
        public bool OnMarch = false;
        public Vector3Int MarchPosition;

        public Queue<BaseUnit> TemporaryUnit;
        

        public TileData()
        {
            Pos = Vector3Int.zero;
            Team = BaseObject.TeamEnum.NONE;
            Tile = null;
            Unit = null;
            Basement = null;
            OnMarch = false;
            MarchPosition = Vector3Int.zero;
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
        }
    }
}