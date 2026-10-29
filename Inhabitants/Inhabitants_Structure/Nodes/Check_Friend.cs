using UnityEngine;

//this is where they read the messages and check everyone on their team...
public class Check_Friend : Node
{
    Message_Center msg_center;
    int id;

    public Check_Friend(int _id, Message_Center _center)
    {
        msg_center = _center;
        id = _id;
    }
    public override void On_Enter()
    {
        base.On_Enter();
    }
    public override Node_States Tree_Tick()
    {
        return Read_Message();
    }

    Node_States Read_Message()
    {
        int percentage = Random.Range(1, 101);

        if (msg_center != null && percentage > 70) msg_center.Process_Messages(id);
        return Node_States.Success;
    }

}