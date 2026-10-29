using UnityEngine;


public class Idle : Node
{
    public float idle_time = 5f;
    float idle_counter = 0f;
    Animator animator;
    Message_Center msg_center;
    public Idle(Message_Center _msg_center, Animator _animator, float _idle_time)
    {
        if (_msg_center != null) msg_center = _msg_center;
        if (_animator != null) animator = _animator;
        idle_time = _idle_time;
    }
    public override void On_Enter()
    {
        base.On_Enter();
        idle_counter = 0f;
    }
    public override Node_States Tree_Tick()
    {
        return Idle_Tick();
    }

    Node_States Idle_Tick()
    {
        if (msg_center.emergency == Emergency.OBJ_AT_DANGER) return Node_States.Success;
        idle_counter += Time.deltaTime;
        if (idle_counter >= idle_time)
        {
            if (animator != null) animator.SetBool("idle", false);
            return Node_States.Success;
        }
        if (animator != null && !animator.GetBool("idle")) animator.SetBool("idle", true);
        return Node_States.Running;
    }
}