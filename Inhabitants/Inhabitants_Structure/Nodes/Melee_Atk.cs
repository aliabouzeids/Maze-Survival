using UnityEngine;


public class Melee_Atk : Node
{
    Animator animator;
    float atk_time = 3f;
    float atk_counter = 0f;
    float atk_range;
    GameObject player;
    LayerMask player_layer;
    Transform position;
    Message_Center msg_center;
    public Melee_Atk(Message_Center _msg_center, Animator _animator, float _atk_time, Transform pos, float range, GameObject p, LayerMask p_layer)
    {
        if (_msg_center != null) msg_center = _msg_center;
        if (_animator != null) animator = _animator;
        atk_time = _atk_time;
        player = p;
        position = pos;
        player_layer = p_layer;
        atk_range = range;
        atk_time = _atk_time;
    }
    public override void On_Enter()
    {
        base.On_Enter();
        Debug.Log("Melee Atk Entered");
    }
    public override Node_States Tree_Tick()
    {
        return Melee_Tick();
    }
    Node_States Melee_Tick()
    {
        if (msg_center != null && msg_center.emergency == Emergency.OBJ_AT_DANGER) return Node_States.Success;
        var direction = position.position - player.transform.position;
        direction.Normalize();

        atk_counter += Time.deltaTime;
        if (animator == null) return Node_States.Failure;
        bool in_range = Physics.Raycast(position.position, direction, atk_range, player_layer);
        if (!in_range)
        {
            atk_counter = 0f;
            animator.SetBool("melee_atk", false);
            return Node_States.Failure;
        }
        if (atk_counter >= atk_time)
        {
            atk_counter = 0f;
            animator.SetBool("melee_atk", false);
            return Node_States.Success;
        }
        if (!animator.GetBool("melee_atk")) animator.SetBool("melee_atk", true);
        return Node_States.Running;
    }
}