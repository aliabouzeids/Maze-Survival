using UnityEngine;


public class First_Person_Camera : MonoBehaviour
{
    Player_Movement p_movement;
    Transform player;

    public Vector2 look;
    float look_x;
    float look_y;
    public float sensitivity;
    public float min_look;
    public float max_look;

    void Awake()
    {
        player = GameObject.Find("player").transform;
        p_movement = player.GetComponent<Player_Movement>();
    }
    void Update()
    {
        Set_Input();
    }
    void LateUpdate()
    {
        Look();
    }
    void Set_Input()
    {
        look = p_movement.fp_look;
    }
    void Look()
    {
        look_x += look.x * sensitivity;
        player.rotation = Quaternion.Euler(0, look_x, 0);

        look_y -= look.y * sensitivity;
        look_y = Mathf.Clamp(look_y, min_look, max_look);
        transform.localRotation = Quaternion.Euler(look_y, 0, 0);
    }

}