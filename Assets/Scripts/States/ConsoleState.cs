using UnityEngine;

public class ConsoleState : BaseState
{
    public ConsoleState(StateController controller) : base(controller) { }

    public override void Enter()
    {
        controller.SecurityCamera.SetActive(true);
        // Only one script should react to Interact while at the console.
        controller.PlayerInteraction.enabled = false;
        controller.InputReader.InteractEvent += HandleInteract;
    }
    public override void Update()
    {
        // Pan the CCTV camera left and right with the look input.
        float pan = controller.InputReader.LookInput.x * controller.PanSensitivity;
        controller.SecurityCamera.transform.Rotate(0f, pan, 0f, Space.World);
    }
    public override void Exit()
    {
        controller.SecurityCamera.SetActive(false);
        controller.PlayerInteraction.enabled = true;
        controller.InputReader.InteractEvent -= HandleInteract;
    }

    private void HandleInteract()
    {
        controller.TransitionToState(new HumanoidState(controller));
    }
}