using UnityEngine;

public interface IInterectable
{
    public string InteractName { get;}
    void Interact();
    void PossibleToInterect();
    void ResetColor();
}