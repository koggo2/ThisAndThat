using UnityEngine;

namespace TheTile.Game
{
    public class EmptyTile : BaseTile
    {
        [SerializeField] private GameObject _flag;

        public override void SetTeam(TeamEnum unitTeam)
        {
            base.SetTeam(unitTeam);

            if (_flag != null)
            {
                var meshRenderer = _flag.GetComponent<MeshRenderer>();
                if (meshRenderer != null)
                {
                    meshRenderer.sharedMaterial = Resources.Load<Material>($"Materials/Team {unitTeam.ToString()} Material");
                }
            }
        }
    }
}
