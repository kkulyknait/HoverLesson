using UnityEngine;

public class SecurityConsole : MonoBehaviour, IInteractable
{
    public void Interact(StateController controller)
    {
        controller.TransitionToState(new ConsoleState(controller));
    }
}
