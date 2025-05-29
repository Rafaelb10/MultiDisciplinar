using UnityEngine;
using UnityEngine.EventSystems;

public class HoverAnimation : MonoBehaviour,IPointerEnterHandler    
{
    public Animator animator;
    public string triggerName = "Hover";

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (animator != null)
        {
            animator.SetTrigger("Hover");
        }
    }
}
