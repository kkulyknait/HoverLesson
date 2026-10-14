using UnityEngine;

public class ConsoleState : BaseState
{
    public ConsoleState(StateController controller) : base(controller) { }

    public override void Enter()
    {
       controller.SecurityCamera.SetActive(true);
    }
    public override void Update()
    {
        
        float pan = controller.InputReader.LookInput.x * controller.PanSensitivity;
        controller.SecurityCamera.transform.Rotate(0f, pan, 0f, Space.World);
       
    }
    public override void Exit()
    {
         controller.SecurityCamera.SetActive(false);
    }
}