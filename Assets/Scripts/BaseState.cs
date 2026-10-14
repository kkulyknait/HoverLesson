using UnityEngine;

public abstract class BaseState 
{
    protected StateController controller;
    public BaseState(StateController controller)
    {
        this.controller = controller;
    }
    public abstract void Enter();
    public abstract void Update();
    public abstract void Exit();

}
