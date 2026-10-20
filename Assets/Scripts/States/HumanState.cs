using UnityEngine;

public class HumanoidState : BaseState
{
    public HumanoidState(StateController controller) : base(controller) { }

    public override void Enter()
    {
        controller.OnFootMovement.enabled = true;
        controller.PlayerLook.enabled = true;
        controller.HumanoidCamera.SetActive(true);
    }
    public override void Update()
    {
        
    }
    public override void Exit()
    {
        controller.OnFootMovement.enabled = false;
        controller.PlayerLook.enabled = false;
        controller.HumanoidCamera.SetActive(false);
    }
}
