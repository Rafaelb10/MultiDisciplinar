using UnityEngine;

public class MaterialReferenceGiver : MonoBehaviour
{
    [SerializeField] private Material _materialToGive;

    public Material GrabMaterialData()
    {
        return _materialToGive;
    }
}
