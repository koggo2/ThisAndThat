using UnityEngine;

public abstract class PathRenderer : MonoBehaviour
{
    public CatmullPath Path;
    public bool autoUpdate = true;
    
    public event System.Action onDestroyed;
    
    public void TriggerUpdate() {
        PathUpdated();
    }


    protected virtual void OnDestroy() {
        if (onDestroyed != null) {
            onDestroyed();
        }
    }

    protected abstract void PathUpdated();
}
