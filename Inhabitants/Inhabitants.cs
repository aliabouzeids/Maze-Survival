using UnityEngine;
using System.Collections.Generic;
using TMPro;
//building the tree at the end...


public enum Inhabitants_Types
{
    AGGRESSIVE,
    GUARDIAN,
    FRIENDLY,
    FARMERS,
}
public class Inhabitants : MonoBehaviour
{
    Sequence root;
    Idle idle;
    Patrol patrol;
    Chase chase;
    Go_Back go_back;
    Melee_Atk melee_atk;
    Ranged_Atk ranged_atk;
    Send_Message send_msg;
    Check_Friend check_friend;
    public Inhabitants_Types type;
    Transform my_position;
    GameObject player;
    Path_Agent agent;
    public Message_Center msg_center;
    Animator animator;
    [SerializeField] LayerMask player_layer;


    [SerializeField] float idle_time = 3f;
    [SerializeField] float atk_range = 30f;
    [SerializeField] float sight_range = 50f;
    [SerializeField] float melee_animation_atk_time = 2f;
    [SerializeField] List<Transform> patrol_points;
    [SerializeField] GameObject ranged_atk_obj;
    [SerializeField] GameObject ranged_atk_vfx;
    [SerializeField] GameObject death_vfx;
    [SerializeField] Transform spawn_point;
    [SerializeField] float ranged_obj_speed;
    [SerializeField] float spawn_interval = 1f;
    [SerializeField] float ranged_atk_time = 2f;
    public int grp_id;
    public int id;
    [SerializeField] I_Message message;
    public bool dead = false;
    public Transform player_known_position;
    IDs_Assigner id_assigner;
    float hello_timer = 0f;
    [SerializeField] float hello_interval = 5f;
    [SerializeField] GameObject hello;
    [SerializeField] Points s_points;
    void Awake()
    {
        message = new I_Message(transform, gameObject.layer, player_layer);
        id_assigner = GameObject.Find("Assigner").GetComponent<IDs_Assigner>();
        agent = GetComponent<Path_Agent>();
        animator = GetComponent<Animator>();
        player = GameObject.Find("player");
        my_position = transform;
        player_layer = player.layer;


    }
    void Start()
    {
        patrol_points = new List<Transform>();
        patrol_points.AddRange(s_points.points);
        if (!id_assigner.initialized) id_assigner.Assign();
        if (type == Inhabitants_Types.AGGRESSIVE) Aggressive_Tree();
        else if (type == Inhabitants_Types.FARMERS) Farmers_Tree();
        else if (type == Inhabitants_Types.GUARDIAN) Guardians_Tree();
        else if (type == Inhabitants_Types.FRIENDLY) Friendly_Tree();

    }
    void Update()
    {
        Brain();
    }
    void Brain()
    {
        root.Tree_Tick();
        //here ill add the chats and some hello for when they see each other;
        hello_timer += Time.deltaTime;
        message.my_update();
        if (message.friends_around_me > 0 && hello_timer > hello_interval)
        {
            Say_Hello();
        }
        if (dead) Die();
    }
    void Aggressive_Tree()
    {
        idle = new Idle(msg_center, animator, idle_time);
        patrol = new Patrol(msg_center, player, sight_range, player_layer, agent, animator, patrol_points, 3, player_known_position);
        chase = new Chase(msg_center, player, agent, atk_range, sight_range, player_layer, player_known_position);
        ranged_atk = new Ranged_Atk(msg_center, player, player_layer, ranged_atk_obj, ranged_atk_vfx, spawn_point, animator, atk_range, ranged_obj_speed, spawn_interval, ranged_atk_time);
        go_back = new Go_Back(msg_center, transform, agent);
        send_msg = new Send_Message(id, message, msg_center);
        check_friend = new Check_Friend(id, msg_center);
        root = new Sequence(new List<Node> { go_back, send_msg, idle, patrol, chase, ranged_atk });
    }
    void Friendly_Tree()
    {
        idle = new Idle(msg_center, animator, idle_time);
        patrol = new Patrol(msg_center, player, sight_range, player_layer, agent, animator, patrol_points, 3, player_known_position);
        chase = new Chase(msg_center, player, agent, atk_range, sight_range, player_layer, player_known_position);
        go_back = new Go_Back(msg_center, transform, agent);
        send_msg = new Send_Message(id, message, msg_center);
        check_friend = new Check_Friend(id, msg_center);
        root = new Sequence(new List<Node> { send_msg, idle, patrol, chase });//chase the player to confuse him hahaha
    }
    void Farmers_Tree()
    {
        idle = new Idle(msg_center, animator, idle_time);
        patrol = new Patrol(msg_center, player, sight_range, player_layer, agent, animator, patrol_points, 3, player_known_position);
        send_msg = new Send_Message(id, message, msg_center);
        check_friend = new Check_Friend(id, msg_center);
        root = new Sequence(new List<Node> { send_msg, idle, patrol });//just walk around the map aimlessly and greets each other
    }
    void Guardians_Tree()
    {
        patrol = new Patrol(msg_center, player, sight_range, player_layer, agent, animator, patrol_points, 3, player_known_position);
        chase = new Chase(msg_center, player, agent, atk_range, sight_range, player_layer, player_known_position);
        ranged_atk = new Ranged_Atk(msg_center, player, player_layer, ranged_atk_obj, ranged_atk_vfx, spawn_point, animator, atk_range, ranged_obj_speed, spawn_interval, ranged_atk_time);
        go_back = new Go_Back(msg_center, transform, agent);
        send_msg = new Send_Message(id, message, msg_center);
        check_friend = new Check_Friend(id, msg_center);
        root = new Sequence(new List<Node> { go_back, send_msg, patrol, chase, ranged_atk });
    }
    void OnCollisionEnter(Collision collision)
    {

    }
    void Say_Hello()
    {
        if (hello == null) return;
        hello_timer = 0f;
        GameObject h = Instantiate(hello, transform.position, Quaternion.identity, transform);
        TextMeshProUGUI greets = h.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        greets.text = Greets();
    }

    void Die()
    {
        if (death_vfx != null) Instantiate(death_vfx, transform.position, Quaternion.identity);

        Destroy(gameObject);//initiale way to die;
    }
    string Greets()
    {
        int i = Random.Range(0, 5);
        return i == 0 ? "Hello Brother" : i == 1 ? "Hello Sister" : i == 2 ? "Hey There how's it going" : i == 3 ? "Have a good day" : i == 4 ? "Nice Day isnt?" : "Hello";
    }
}