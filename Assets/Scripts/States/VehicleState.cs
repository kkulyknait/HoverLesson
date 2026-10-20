using UnityEngine;

public class VehicleState : BaseState
{
    private readonly HoverController _hover;
    private readonly Transform _vehicle;

    public VehicleState(StateController controller, GameObject vehicle) : base(controller)
    {
        _vehicle = vehicle.transform;
        _hover = vehicle.GetComponent<HoverController>();
    }

    public override void Enter()
    {
        _hover.enabled = true;
        controller.VehicleCamera.SetActive(true);
        controller.PlayerInteraction.enabled = false;
        controller.InputReader.InteractEvent += HandleInteract;
    }

    public override void Update() { }

    public override void Exit()
    {
        _hover.enabled = false;
        controller.VehicleCamera.SetActive(false);
        controller.PlayerInteraction.enabled = true;
        controller.InputReader.InteractEvent -= HandleInteract;
        MovePlayerBesideVehicle();
    }

    private void HandleInteract()
    {
        controller.TransitionToState(new HumanoidState(controller));
    }

    private void MovePlayerBesideVehicle()
    {
        // A CharacterController overwrites position changes while it is enabled.
        CharacterController body = controller.GetComponent<CharacterController>();
        body.enabled = false;
        controller.transform.position = _vehicle.position + _vehicle.right * 3f;
        body.enabled = true;
    }
}