using UnityEngine;

public class UIEventHandler : MonoBehaviour
{
    public static bool IsOnBuildTrack => _isOnBuildTrack;

    private static bool _isOnBuildTrack = false;
    
    public void OnToggleBuildTrack(bool value)
    {
        _isOnBuildTrack = value;
    }
}
