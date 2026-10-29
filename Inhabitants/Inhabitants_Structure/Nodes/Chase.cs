using UnityEngine;


public class Chase : Node
{
    GameObject player;
    float attack_range = 25f;
    float sight_range = 50f;
    Path_Agent agent;
    Message_Center msg_center;
    Transform player_known_position;
    public Chase(
        Message_Center _msg_center,
        GameObject _player,
        Path_Agent _agent,
        float s_range,
        float a_range,
        LayerMask p_l,
        Transform _player_known_position

    )
    {
        if (_msg_center != null) msg_center = _msg_center;
        player = _player;
        agent = _agent;
        attack_range = a_range;
        sight_range = s_range;
        player_known_position = _player_known_position;
    }
    public override void On_Enter()
    {
        base.On_Enter();
        Debug.Log("Chase Entered");
    }

    public override Node_States Tree_Tick()
    {
        Node_States state = Chase_Tick();
        return state;
    }
    Node_States Chase_Tick()
    {
        if (msg_center.emergency == Emergency.OBJ_AT_DANGER) return Node_States.Success;
        agent.transform.LookAt(player.transform);
        var position = agent.transform.position + new Vector3(0f, 0.5f, 0f);
        var distance = Vector3.Distance(position, player.transform.position);
        bool in_sight = distance <= sight_range || player_known_position != null;
        if (!in_sight) return Node_States.Failure;
        bool can_attack = distance <= attack_range;
        if (can_attack)
        {
            Debug.Log("Chase: can attack the player");
            return Node_States.Success;
        }
        agent.Set_Destination(player.transform.position);
        return Node_States.Running;
    }
}