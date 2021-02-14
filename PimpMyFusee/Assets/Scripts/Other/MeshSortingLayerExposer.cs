using UnityEngine;

/// <summary>
/// Place on a 3D object to modify its visibility relative to Sprite 'sorting
/// layers'.
/// </summary>
[ExecuteInEditMode]
public sealed class MeshSortingLayerExposer : MonoBehaviour
{

    [SerializeField] private string SortingLayerName = "Default";
    [SerializeField] private int SortingOrder = 0;







    public void OnValidate()                                                    // ON VALIDATE
    {
        apply();
    }


    public void OnEnable()                                                                      // ON ENABLE
    {
        apply();
    }






    private void apply()
    {
        if (GetComponent<MeshRenderer>())
        {
            var meshRenderer = gameObject.GetComponent<MeshRenderer>();
            meshRenderer.sortingLayerName = SortingLayerName;
            meshRenderer.sortingOrder = SortingOrder;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            var skinnedMeshRenderer = gameObject.GetComponent<SkinnedMeshRenderer>();
            skinnedMeshRenderer.sortingLayerName = SortingLayerName;
            skinnedMeshRenderer.sortingOrder = SortingOrder;
        }
    }
}