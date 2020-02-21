using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;  
    public static T Instance  
    {  
        get  
        {  
            if (!_instance)  
            {  
                _instance = GameObject.FindObjectOfType(typeof(T)) as T;
                if (!_instance)  
                {  
                    GameObject container = new GameObject();  
                    container.name = typeof(T).Name;
                    _instance = container.AddComponent(typeof(T)) as T;  
                }  
            }  
  
            return _instance;  
        }  
    }
}
