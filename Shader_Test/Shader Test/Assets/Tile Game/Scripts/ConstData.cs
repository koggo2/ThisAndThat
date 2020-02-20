using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public static class ConstData
    {
        public const string Object_NoneTileName = "Test Tile";
        public const string Object_EmptyTileName = "Empty Tile";
        public const string Object_ConstructionBasementName = "Construction Basement";
        public const string Object_HouseBasementName = "House Basement";

        public const string Unit_WorkerPrefabPath = "Unit/Unit_Worker";

        public const string UI_BasementInfoName = "UI_BasementInfo";
        public const string UI_UnitInfoName = "UI_UnitInfo";
        public const string UI_Cancel = "UI_Cancel";

        public static string Animation_Idle = "Idle";
        public static string Animation_Run = "Run";
        public static string Animation_Attack01 = "Attack01";

        public static string Animation_Parameter_Moving = "Moving";
        
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
