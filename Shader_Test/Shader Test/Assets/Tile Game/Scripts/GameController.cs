using System;
using UnityEngine;

namespace TheTile.Game
{
    public class GameController : Singleton<GameController>
    {
        public struct BuildData
        {
            public Vector3Int OriginPos;
            public Vector3Int TargetPos;
            public string PrefabName;
            public bool NeedConstruction;
            public string NextBasementName;
        }
        
        public void MarchNBuild(Vector3Int originPos, Vector3Int targetPos)
        {
            var originTileData = GameGrid.Instance.GetTileData(originPos);
            var targetTileData = GameGrid.Instance.GetTileData(targetPos);
            if (originTileData == null || targetTileData == null)
                return;

            MarchToTile(originPos, targetPos);
            
            if (targetTileData.Basement == null)
            {
                var buildData = new BuildData()
                {
                    NeedConstruction = true,
                    OriginPos = originPos,
                    TargetPos = targetPos,
                    PrefabName = ConstData.Object_ConstructionBasementName,
                    NextBasementName = ConstData.Object_HouseBasementName,
                };
                var constructionBasement = BuildBasement<ConstructionBasement>(buildData);
                constructionBasement.SetConstructionData(buildData.NextBasementName);
            }
        }

        public void BuildTile(Vector3Int cellPos, string tilePrefabName)
        {
            var tileData = GameGrid.Instance.GetTileData(cellPos);
            if (tileData == null)
                return;
            
            var prefab = Resources.Load<GameObject>(tilePrefabName);
            var prefabInstance = Instantiate(prefab);
            prefabInstance.transform.SetParent(tileData.Tile.transform.parent);
            prefabInstance.transform.localPosition = tileData.Tile.transform.localPosition;
            
            if(tileData.Tile != null)
                DestroyImmediate(tileData.Tile.gameObject);
            
            var tile = prefabInstance.GetComponent<BaseTile>();
            tile.SetTeam(tileData.Team);
            tileData.Tile = tile;
        }

        private T BuildBasement<T>(BuildData buildData) where T : BaseBasement
        {
            var originTileData = GameGrid.Instance.GetTileData(buildData.OriginPos);
            var targetTileData = GameGrid.Instance.GetTileData(buildData.TargetPos);
            if (originTileData == null || targetTileData == null)
                return null;

            try
            {
                var prefab = Resources.Load<GameObject>(buildData.PrefabName);
                var prefabInstance = Instantiate(prefab);
                prefabInstance.transform.SetParent(targetTileData.Tile.Pivot);
                prefabInstance.transform.localPosition = Vector3.zero;

                var baseBasement = prefabInstance.GetComponent<BaseBasement>();
                baseBasement.SetTeam(originTileData.Team);

                targetTileData.OnConstruction = buildData.NeedConstruction;
                targetTileData.Team = originTileData.Team;
                targetTileData.UpdateBasement(baseBasement);

                return baseBasement as T;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
                Debug.LogError($"BuildBasement has error..!\nBuildData PrefabName == {buildData.PrefabName}");
                throw;
            }
        }

        private void MarchToTile(Vector3Int originPos, Vector3Int targetPos)
        {
            var originTileData = GameGrid.Instance.GetTileData(originPos);
            var targetTileData = GameGrid.Instance.GetTileData(targetPos);

            if (originTileData == null || targetTileData == null)
                return;
            
            if (originPos == targetPos)
            {
                originTileData.APlaceToGo.Clear();
            }
            else
            {
                originTileData.APlaceToGo.Clear();
                originTileData.APlaceToGo.Add(targetPos);

                if (!targetTileData.APlaceToCome.Contains(originPos))
                    targetTileData.APlaceToCome.Add(originPos);
            }
        }

        public void CompleteBasement(Vector3Int worldToCellPos, string nextBasementName)
        {
            BuildTile(worldToCellPos, ConstData.Object_EmptyTileName);
            BuildBasement<BaseBasement>(new BuildData()
            {
                NeedConstruction = false,
                OriginPos = worldToCellPos,
                TargetPos = worldToCellPos,
                PrefabName = nextBasementName,
            });
        }
        
        public void CancelConstruction(Vector3Int cancelledPos)
        {
            var cancelledTileData = GameGrid.Instance.GetTileData(cancelledPos);
            if (cancelledTileData == null) return;
            if (!cancelledTileData.OnConstruction) return;
            
            cancelledTileData.OnConstruction = false;
            cancelledTileData.RemoveBasement();
            
            cancelledTileData.APlaceToCome.ForEach(pos =>
            {
                var tileData = GameGrid.Instance.GetTileData(pos);
                if (tileData.APlaceToGo.Contains(cancelledPos))
                    tileData.APlaceToGo.Remove(cancelledPos);

                if (tileData.Basement is HouseBasement houseBasement)
                {
                    houseBasement.GeneratedUnits.ForEach(unit =>
                    {
                        unit.March(new AStarSearch(GameGrid.Instance, GameGrid.Instance.WorldToCellPos(unit.transform.position), pos));
                    });
                }
            });

            cancelledTileData.APlaceToCome.Clear();
        }
    }
}