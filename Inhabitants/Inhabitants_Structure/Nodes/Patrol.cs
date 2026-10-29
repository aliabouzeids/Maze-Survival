using UnityEngine;
using System.Collections.Generic;

public class Patrol : Node
{
    GameObject player;
    Path_Agent agent;
    Animator animator;
    List<Transform> search_points;
    public Transform life_source;
    int index = 0;
    int point_offset = 3;
    float sight_range = 50f;
    LayerMask player_layer;
    Message_Center msg_center;
    Transform player_known_position;
    public Patrol
    (
        Message_Center _msg_center,
        GameObject _player,
        float _range,
        LayerMask _p_layer,
        Path_Agent _agent,
        Animator _animator,
        List<Transform> points,
        int _offset,
        Transform _player_known_position
    )
    {
        if (_msg_center != null) msg_center = _msg_center;
        player = _player;
        sight_range = _range;
        player_layer = _p_layer;
        animator = _animator;
        agent = _agent;
        search_points = points;
        point_offset = _offset;
        player_known_position = _player_known_position;
        index = Random.Range(0, search_points.Count);
    }
    public override void On_Enter()
    {
        base.On_Enter();

    }
    public override Node_States Tree_Tick()
    {
        Node_States state = Patrol_Tick();
        return state;
    }
    public override void On_Exit()
    {
        base.On_Exit();
    }
    Node_States Patrol_Tick()
    {
        if (msg_center.emergency == Emergency.OBJ_AT_DANGER) return Node_States.Success;

        if (search_points.Count > 0)
        {
            var position = agent.transform.position + new Vector3(0f, 0.5f, 0);
            var direction = player.transform.position - position;
            direction.Normalize();
            bool found_player = Physics.Raycast(position, direction, sight_range, player_layer) || player_known_position != null;
            if (found_player)
            {
                agent.Set_Destination(agent.transform.position);
                return Node_States.Success;
            }
            if (agent.destination != search_points[index].position) agent.Set_Destination(search_points[index].position);
            if (agent.Is_Near_This(search_points[index].position, point_offset))
            {
                index += Random.Range(0, search_points.Count);
                if (index >= search_points.Count)
                {
                    index = Random.Range(0, search_points.Count);
                    return Node_States.Failure;
                }
            }
            return Node_States.Running;
        }
        return Node_States.Success;
    }
}
