using UnityEngine;

namespace TheTile.Util
{
    public class LineManager : Singleton<LineManager>
    {
        private const float ArcPointCount = 24f;

        [SerializeField] private float _arcHeight = 1.0f;
        
        private LineRenderer _lineRenderer;

        void Awake()
        {
            _lineRenderer = GetComponent<LineRenderer>();
            
            _lineRenderer.startColor = Color.red;
            _lineRenderer.endColor = Color.red;
            _lineRenderer.startWidth = 0.1f;
            _lineRenderer.endWidth = 0.1f;
            _lineRenderer.positionCount = 24;
            _lineRenderer.enabled = false;
        }

        public void DrawArc(Vector3Int start, Vector3Int end)
        {
            _lineRenderer.enabled = true;
            var vStart = GameGrid.Instance.CellPosToWorld(start);
            var vEnd = GameGrid.Instance.CellPosToWorld(end);
            
            Debug.Log($"start = {start}/{vStart}, end = {end}/{vEnd}");

            var direction = vEnd - vStart;
            direction /= ArcPointCount;

            for (var i = 0; i < ArcPointCount; ++i)
            {
                var point = vStart + direction * i;
                point.y += _arcHeight * Mathf.Sin(Mathf.PI * i / ArcPointCount);
                
                _lineRenderer.SetPosition(i, point);
            }
        }

        public void HideLine()
        {
            _lineRenderer.enabled = false;
        }
    }
}
