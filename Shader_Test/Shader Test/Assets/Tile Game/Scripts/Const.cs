using System;
using TheTile.Game;
using UnityEngine;

namespace TheTile
{
    public static class Const
    {
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
