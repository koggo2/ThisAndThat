using System.Collections.Generic;
using UnityEngine;

namespace TheTile.Game
{
    public interface WeightedGraph<T>
    {
        int Cost(Vector3Int a, Vector3Int b);
        IEnumerable<Vector3Int> Neighbors(Vector3Int pos);
    }
}
