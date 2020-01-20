using UnityEngine;

public class RailPoint : MonoBehaviour
{
    private Renderer _meshRenderer;

    void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    
    private void OnMouseEnter()
    {
        _meshRenderer.material.color = Color.red;
    }

    private void OnMouseExit()
    {
        _meshRenderer.material.color = Color.white;
    }
}
