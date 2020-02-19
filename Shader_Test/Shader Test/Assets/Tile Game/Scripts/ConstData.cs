using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public static class ConstData
    {
        public const string Object_NoneTileName = "Test Tile";
        public const string Object_EmptyTileName = "Empty Tile";

        public const string Object_HouseBasementName = "House Basement";

        public const string UI_BasementInfoName = "UI_BasementInfo";
        public const string UI_UnitInfoName = "UI_UnitInfo";
        
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
