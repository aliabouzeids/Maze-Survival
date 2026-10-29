using UnityEngine;
public class Area
{
    public Vector3 center;
    public float range;
}
public class I_Message
{
    public Transform player_position;
    Transform position;
    public Area area_scanned => Scanned_This();
    public Transform taking_rest_here => position;
    public int friends_around_me => friends_around();
    public LayerMask layer;
    public float friends_check_range = 5f;
    LayerMask player_layer;
    public I_Message(Transform my_transform, LayerMask my_layer, LayerMask _p_l)
    {
        position = my_transform;
        layer = my_layer;
        player_layer = _p_l;
    }

    public bool is_empty()
    {
        return player_position == null &&
        area_scanned.center != Vector3.zero &&
        taking_rest_here == null &&
        layer != 0 &&
        position != null &&
        friends_around_me != 0;
    }
    int friends_around()
    {
        int friends = 0;
        Collider[] cols = Physics.OverlapSphere(position.position, friends_check_range);
        foreach (var col in cols)
        {
            Inhabitants inh = col.gameObject.GetComponent<Inhabitants>();
            Inhabitants me = position.GetComponent<Inhabitants>();
            if (inh != null && me != inh) friends++;

        }
        return friends;
    }
    Area Scanned_This()
    {
        Area area = new Area();
        area.center = position.position;
        area.range = 7f;
        return area;
    }
    public void my_update()
    {
        var direction = GameObject.Find("player").transform.position - position.position;
        direction.Normalize();
        bool player_is_found = Physics.Raycast(position.position, direction, 20, player_layer);
        player_position = player_is_found ? GameObject.Find("player").transform : null;
    }
}