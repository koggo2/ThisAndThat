using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TheTile.Game
{
    public class BaseTile : BaseObject
    {
        public void SetTeam(TeamEnum unitTeam)
        {
            Team = unitTeam;
            
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                renderer.material.color = Const.GetTeamColor(Team);
            }
        }
    }
}
