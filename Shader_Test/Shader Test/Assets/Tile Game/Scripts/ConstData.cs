using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public static class ConstData
    {
        public const string NoneTilePrefabName = "Test Tile";
        public const string EmptyTilePrefabName = "Empty Tile";

        public const string HouseBasementPrefabName = "House Basement";
        
        public static Color AColor = Color.green;
        public static Color BColor = Color.cyan;

        public static Color GetTeamColor(BaseObject.TeamEnum teamEnum)
        {
            switch (teamEnum)
            {
                case BaseObject.TeamEnum.NONE:
                    return Color.white;
                case BaseObject.TeamEnum.A:
                    return AColor;
                case BaseObject.TeamEnum.B:
                    return BColor;
                default:
                    return Color.white;
            }
        }
    }
}
