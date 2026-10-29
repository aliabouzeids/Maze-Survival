using UnityEngine;
public class Ranged_Atk : Node
{
    GameObject player;
    LayerMask player_layer;
    GameObject bullet;
    GameObject vfx;
    Transform spawn_point;
    Animator animator;
    float range = 50f;
    float speed = 1f;
    float spawn_interval = 1f;
    float spawn_interval_counter = 0f;

    bool shooted;
    float animation_counter;
    float animation_time;
    Message_Center msg_center;
    public Ranged_Atk
    (
        Message_Center _msg_center,
        GameObject _player,
        LayerMask _player_layer,
        GameObject _bullet,
        GameObject _vfx,
        Transform _spawn_point,
        Animator _animator,
        float _range,
        float _speed,
        float _spawn_interval,
        float _animation_time
    )
    {
        msg_center = _msg_center;
        player = _player;
        player_layer = _player_layer;
        bullet = _bullet;
        vfx = _vfx;
        spawn_point = _spawn_point;
        animator = _animator;
        range = _range;
        speed = _speed;
        spawn_interval = _spawn_interval;
        animation_time = _animation_time;
    }
    public override void On_Enter()
    {
        base.On_Enter();
        shooted = false;
        Debug.Log("Ranged Atk Entered");
    }
    public override Node_States Tree_Tick()
    {
        return Atk_Tick();
    }
    Node_States Atk_Tick()
    {
        if (msg_center.emergency == Emergency.OBJ_AT_DANGER) return Node_States.Success;
        animator.transform.LookAt(player.transform);
        Vector3 direction = player.transform.position - spawn_point.position;
        direction.Normalize();
        bool inside_range = Physics.Raycast(spawn_point.position, direction, range, player_layer);
        if (!inside_range) return Node_States.Failure;

        float spread = 0.1f;
        Vector3 inaccurate_direction = direction + Random.insideUnitSphere * spread;
        inaccurate_direction.Normalize();
        Shoot(inaccurate_direction);
        if (!animator.GetBool("ranged_atk")) animator.SetBool("ranged_atk", true);
        animation_counter += Time.deltaTime;
        if (animation_counter < animation_time) return Node_States.Running;
        animation_counter = 0f;
        if (animator.GetBool("ranged_atk")) animator.SetBool("ranged_atk", false);
        if (shooted) return Node_States.Success;
        else return Node_States.Failure;

    }
    void Shoot(Vector3 direction)
    {
        if (bullet == null)
        {
            shooted = false;
            return;
        }
        spawn_interval_counter += Time.deltaTime;
        if (spawn_interval <= spawn_interval_counter)
        {
            spawn_interval_counter = 0f;

            GameObject b = Object.Instantiate(bullet, spawn_point.position, Quaternion.identity);
            if (vfx != null) Object.Instantiate(vfx, spawn_point.position, Quaternion.identity);
            Rigidbody rb = b.GetComponent<Rigidbody>();
            if (rb == null) shooted = false;
            if (rb != null) rb.AddForce(direction * speed, ForceMode.VelocityChange);
            shooted = true;

        }

    }
}