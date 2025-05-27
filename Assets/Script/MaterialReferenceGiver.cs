using UnityEngine;

public class MaterialReferenceGiver : MonoBehaviour
{
    [SerializeField] private Material _materialToGive;
    private Shader _shader;
    public Material GrabMaterialData()
    {
        
        return _materialToGive;
    }
    
    // O seguinte script permite ter uma referência ao material que se quer mudar. Qualquer objeto com este material irá reagir às mudanças feitas
    // Também é possivel mudar os parâmetros através de um Animator ou uma Timeline, sem recorrer a código.
    
    
    //Para usar depois a referência para mudar o Cutoff Height (e fazer a animação da muralha cair), é rodar esta linha de código:
    
    //    _materialToGive.SetFloat("_CutoffHeight", 0.5f);
    
    //Podem depois fazer algo como um lerp naquele float entre uns valores numa Coroutine para "animar" o efeito.
    
}
