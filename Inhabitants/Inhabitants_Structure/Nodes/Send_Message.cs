using UnityEngine;

public class Send_Message : Node
{
    I_Message message;
    Message_Center msg_center;
    int id;

    float message_interval = 2f;
    float message_timer = 0f;
    public Send_Message(
        int _id,
    I_Message _message,
        Message_Center _msg_center
)
    {
        id = _id;
        message = _message;
        msg_center = _msg_center;
    }

    public override void On_Enter()
    {
        base.On_Enter();
        message_timer = Random.Range(0.005f, 4f);
    }
    public override Node_States Tree_Tick()
    {
        return Send_Message_Tick();
    }

    Node_States Send_Message_Tick()
    {
        message_timer += Time.deltaTime;
        if (message_interval >= message_timer) return Node_States.Running;
        message_timer = 0f;
        if (!message.is_empty())
        {
            msg_center.pending_messages[id] = message;
        }

        return Node_States.Success;
    }
}