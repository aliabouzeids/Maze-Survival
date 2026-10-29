using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;


enum camera_type
{
    FIRST_PERSON,
    THIRD_PERSON,
}
public class Player_Movement : MonoBehaviour
{
    public bool mobile = false;
    private int right_finger_id = -1;
    private float half_screen_width;
    private Vector2 right_touch_start_position;
    private Vector2 look_input;
    [SerializeField] float mobile_sensitivity = 0.5f;
    [SerializeField] Movement_Button btn_up;
    [SerializeField] Movement_Button btn_down;
    [SerializeField] Movement_Button btn_left;
    [SerializeField] Movement_Button btn_right;
    [Header("Camera Type")]
    [SerializeField] Transform fp_camera;
    [SerializeField] Transform tp_camera;
    [SerializeField] camera_type cam_type;
    public Vector2 fp_look;
    public Vector2 tp_look;
    Transform main_camera;
    public bool cam_changed = true;
    public Input_System input_system;
    Teleport tp;
    [SerializeField] float move_speed = 1f;
    Vector2 move;
    Vector3 move_direction;
    float move_x;
    float move_y;
    [Header("Jump Settings")]
    [SerializeField] float ground_distance = 0.4f;
    [SerializeField] float ground_offset = 0.5f;
    [SerializeField] bool is_grounded;
    [SerializeField] LayerMask ground_layer;
    [SerializeField] bool jumping;
    public float jump_force = 5f;
    [SerializeField] float air_speed = 0.7f;
    [SerializeField] bool ducking;
    [SerializeField] float duck_speed = 0.5f;
    [SerializeField] float max_air_time = 1.5f;
    float air_time_counter = 0f;
    [SerializeField] bool max_jump_reached = false;
    [SerializeField] float player_y = 0f;
    [SerializeField] float max_jump_reach = 5f;

    Rigidbody rb;
    UI_Inputs_Features ui_features;
    [Header("Wall Interaction Settings")]
    [SerializeField] bool touching_wall;
    [SerializeField] bool colliding_with_wall;
    [SerializeField] float wall_force = 15f;
    [SerializeField] float wall_up_addition = 0.025f;
    [SerializeField] LayerMask wall_mask;
    [SerializeField] float wall_slide_speed = 0.5f;
    [SerializeField] Vector3 wall_hit_position;
    [SerializeField] bool is_grounded_on_wall_for_xyz_time;
    [SerializeField] float xyz_time = 2f;
    float touching_wall_counter = 0f;

    [Header("gravity settings")]
    [SerializeField] float gravity_force = 10f;
    Player player;

    bool is_moving;
    Animator animator;
    AudioSource walk_sound;
    [SerializeField] GameObject jump_sound;
    void Awake()
    {
        walk_sound = GetComponent<AudioSource>();
        animator = GetComponentInChildren<Animator>();
        if (input_system == null) input_system = new Input_System();
        cam_type = new camera_type();
        rb = GetComponent<Rigidbody>();
        ui_features = GetComponent<UI_Inputs_Features>();
        tp = GetComponent<Teleport>();
        player = GetComponent<Player>();

    }
    void Start()
    {
        Set_Camera();
        player_y = transform.position.y;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        half_screen_width = Screen.width / 2f;
    }
    void Input_Values()
    {
        move_x = move.x;
        move_y = move.y;

    }

    void Get_Touch_Input()
    {
        if (!EnhancedTouchSupport.enabled)
            EnhancedTouchSupport.Enable();

        foreach (var t in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            switch (t.phase)
            {
                case UnityEngine.InputSystem.TouchPhase.Began:
                    if (t.screenPosition.x > half_screen_width && right_finger_id == -1)
                    {
                        right_finger_id = t.touchId;
                        right_touch_start_position = t.screenPosition;
                    }
                    break;

                case UnityEngine.InputSystem.TouchPhase.Ended:
                case UnityEngine.InputSystem.TouchPhase.Canceled:
                    if (t.touchId == right_finger_id)
                    {
                        right_finger_id = -1;
                        look_input = Vector2.zero;
                    }
                    break;

                case UnityEngine.InputSystem.TouchPhase.Moved:
                    if (t.touchId == right_finger_id)
                    {
                        look_input = (t.screenPosition - right_touch_start_position) * mobile_sensitivity;
                    }
                    break;

                case UnityEngine.InputSystem.TouchPhase.Stationary:
                    if (t.touchId == right_finger_id)
                        look_input = Vector2.zero;
                    break;
            }
        }

        fp_look = look_input;
        tp_look = look_input;
    }
    void OnEnable()
    {

        input_system.Movement.Move.performed += _ => move = _.ReadValue<Vector2>();
        input_system.Movement.Move.canceled += _ => move = Vector2.zero;
        input_system.FP_Camera.Look.performed += _ => fp_look = _.ReadValue<Vector2>();
        input_system.FP_Camera.Look.performed += _ => tp_look = _.ReadValue<Vector2>();
        input_system.Movement.Duck.performed += _ => ducking = true;
        input_system.Movement.Duck.canceled += _ => ducking = false;
        input_system.Movement.Jump.performed += _ => jumping = true;
        input_system.UI.Camera_Switch.performed += _ => Switch_Camera();
        input_system.Movement.Teleport_Forward.performed += _ => tp.Demon_Steps();
        input_system.Movement.Throw_Bomb.performed += _ => player.Throw_Bomb();
        input_system.UI.Cursor.performed += _ => ui_features.Open_Close_Cursor();
        input_system.Enable();
    }
    void OnDisable()
    {
        input_system.Disable();
    }


    void Update()
    {
        is_moving = move_x != 0 || move_y != 0;

        Set_Camera();
        if (mobile) Get_Touch_Input();

        Input_Values();

        Check_Ground();


    }
    void FixedUpdate()
    {
        if (jumping) Jump();
        if (is_moving) Move();
        else
        {
            if (walk_sound.isPlaying || !is_grounded) walk_sound.Stop();
            animator.SetBool("walk", false);
        }
    }

    ///======================================================================================================
    ///================================MOVEMENT FUNCTIONS====================================================
    ///======================================================================================================








    void Move()
    {
        if (animator != null) animator.SetBool("walk", true);
        if (!walk_sound.isPlaying && is_grounded) walk_sound.Play();
        move_direction = Camera.main.transform.forward * move_y;
        move_direction += Camera.main.transform.right * move_x;
        move_direction.Normalize();
        move_direction.y = 0;

        float current_move_speed = !is_grounded ? move_speed * air_speed
                                                : ducking ? move_speed * duck_speed
                                                          : move_speed;

        rb.AddForce(move_direction * current_move_speed, ForceMode.Acceleration);//wayyyy better than changing the velocity of the rb!!!
    }
    public void Jump()
    {
        if (jump_sound != null) Instantiate(jump_sound);
        if (!is_grounded && touching_wall)
        {
            Wall_Jump();
        }
        else if (is_grounded)
        {
            if (animator != null) animator.SetTrigger("jump");
            rb.AddForce(Vector3.up * jump_force, ForceMode.VelocityChange);
        }
        jumping = false;
    }
    void Check_Ground()
    {
        Vector3 player_foots = transform.position - new Vector3(0f, ground_offset, 0f);
        is_grounded_on_wall_for_xyz_time = touching_wall && touching_wall_counter < xyz_time;
        if (is_grounded_on_wall_for_xyz_time) rb.useGravity = false;
        else rb.useGravity = true;
        is_grounded = Physics.CheckSphere(player_foots, ground_distance, ground_layer);
        max_jump_reached = transform.position.y >= player_y + max_jump_reach;
        if (is_grounded)
        {
            player_y = transform.position.y;
            air_time_counter = 0f;
            touching_wall_counter = 0f;
        }
        if (max_jump_reached) Gravity_Acceleration_When_Max_Distance_Reached_While_In_Air();
        touching_wall = Physics.Raycast(transform.position, transform.right, 0.6f, wall_mask)
           || Physics.Raycast(transform.position, -transform.right, 0.6f, wall_mask)
           || Physics.Raycast(transform.position, transform.forward, 0.6f, wall_mask)
           || Physics.Raycast(transform.position, -transform.forward, 0.6f, wall_mask)
           || colliding_with_wall;
        if (touching_wall) touching_wall_counter += Time.deltaTime;
        else touching_wall_counter = 0;
        if (!is_grounded && touching_wall)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * wall_slide_speed, rb.linearVelocity.z);
        }

    }


    void OnDrawGizmos()
    {
        Vector3 player_foots = transform.position - new Vector3(0f, ground_offset, 0f);

        Gizmos.color = Color.black;
        Gizmos.DrawSphere(player_foots, ground_distance);
    }

    void Wall_Jump()
    {
        float front_dot = Vector3.Dot(wall_hit_position, transform.forward);
        float right_dot = Vector3.Dot(wall_hit_position, transform.right);

        Vector3 jump_wall_dir = Vector3.zero;

        if (front_dot < -0.7f) jump_wall_dir = -main_camera.forward;
        else if (front_dot > 0.7f) jump_wall_dir = main_camera.forward;
        else if (right_dot > 0.7f) jump_wall_dir = main_camera.right;
        else if (right_dot < -0.7f) jump_wall_dir = -main_camera.right;

        jump_wall_dir += Vector3.up * wall_up_addition;
        jump_wall_dir.Normalize();

        rb.AddForce(jump_wall_dir * wall_force, ForceMode.Impulse);
        // Shake_Camera();
        jumping = false;
    }

    void Gravity_Acceleration_When_Max_Distance_Reached_While_In_Air()
    {
        air_time_counter += Time.deltaTime;
        if (is_grounded || touching_wall || air_time_counter < max_air_time) return;
        rb.AddForce(Vector3.down * gravity_force, ForceMode.Acceleration);


    }

    ///======================================================================================================
    ///================================CAMERA FUNCTIONS======================================================
    ///======================================================================================================



    void Set_Camera()
    {
        if (fp_camera == null || tp_camera == null) return;
        if (!cam_changed) return;


        main_camera = Camera.main.transform;
        tp_camera.gameObject.SetActive(cam_type == camera_type.THIRD_PERSON);
        fp_camera.gameObject.SetActive(cam_type == camera_type.FIRST_PERSON);

        cam_changed = false;
        string camera_type_st = cam_type == camera_type.FIRST_PERSON ? "First Person" : "Third Person";
        Debug.Log("Camera Mode has changed to " + camera_type_st);
    }

    public void Switch_Camera()
    {
        if (cam_type == camera_type.THIRD_PERSON)
        {
            cam_type = camera_type.FIRST_PERSON;
            cam_changed = true;
        }
        else
        {
            cam_type = camera_type.THIRD_PERSON;
            cam_changed = true;
        }
    }




    ///======================================================================================================
    ///================================COLLISION FUNCTIONS===================================================
    ///======================================================================================================


    void OnCollisionEnter(Collision collision)
    {
        wall_hit_position = collision.GetContact(0).normal;
        colliding_with_wall = true;
    }
    void OnCollisionStay(Collision collision)
    {
        wall_hit_position = collision.GetContact(0).normal;
        colliding_with_wall = true;

    }
    void OnCollisionExit(Collision collision)
    {
        colliding_with_wall = false;
    }
}
