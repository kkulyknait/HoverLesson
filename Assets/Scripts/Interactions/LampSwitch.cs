using UnityEngine;

public class LampSwitch : MonoBehaviour, IInteractable
{
    [SerializeField] private Light lamp;

    public void Interact(StateController controller)
    {
        lamp.enabled = !lamp.enabled;
    }
}
