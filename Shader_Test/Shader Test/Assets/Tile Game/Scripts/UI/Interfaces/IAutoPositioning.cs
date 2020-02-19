using TheTile.Game;
using UnityEngine;

namespace TheTile.UI.Interface
{
    interface IAutoPositioning
    {
        void UpdatePosition(Camera camera, RectTransform canvasRect);
    }
}
