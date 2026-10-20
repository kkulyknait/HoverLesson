using UnityEngine;

public class HoverVehicle : MonoBehaviour, IInteractable
{
    public void Interact(StateController controller)
    {
        controller.TransitionToState(new VehicleState(controller, gameObject));
    }
}
