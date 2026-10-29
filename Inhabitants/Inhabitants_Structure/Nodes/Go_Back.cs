using UnityEngine;
public class Go_Back : Node
{
    Message_Center msg_center;
    Path_Agent agent;
    Transform source;

    public Go_Back(Message_Center _msg_center, Transform source, Path_Agent _agent)
    {
        if (_msg_center != null) msg_center = _msg_center;
        if (_agent != null) agent = _agent;
    }
    public override void On_Enter()
    {
        base.On_Enter();
        Debug.Log("go_back entered");
    }
    public override Node_States Tree_Tick()
    {
        return Return();
    }
    Node_States Return()
    {
        if (msg_center.emergency != Emergency.OBJ_AT_DANGER) return Node_States.Success;
        agent.Set_Destination(source.position);
        if (agent.Is_Near_This(source.position, 5f)) return Node_States.Success;
        return Node_States.Running;
    }
}