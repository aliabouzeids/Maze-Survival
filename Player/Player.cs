using TMPro;
using UnityEngine;
public class Player : MonoBehaviour
{
    public int player_life = 3;
    public int max_player_life;
    public int key_number = 0;
    [SerializeField] float hit_interval = 5f;
    [SerializeField] GameObject hit_vfx;
    [SerializeField] Transform hit_vfx_sp;
    [SerializeField] GameObject bomb_obj;
    [SerializeField] GameObject death_obj;
    [SerializeField] GameObject heart_obj;
    [SerializeField] Transform heart_parent;

    [SerializeReference] TextMeshProUGUI bomb_text;
    [SerializeReference] TextMeshProUGUI key_text;
    public float throw_force = 5f;

    public Transform spawn_point;
    public int bomb_number;
    public float bomb_interval = 2f;

    float timer = 0f;
    public bool dead = false;

    public Found_Item found_item;

    float hit_counter = 0f;
    void Start()
    {
        for (int i = 0; i < player_life; i++) Instantiate(heart_obj, heart_parent);
        if (dead) dead = false;
        if (Time.timeScale == 0f) Time.timeScale = 1f;
        player_life = 3;
    }
    void Update()
    {
        timer += Time.deltaTime;
        hit_counter += Time.deltaTime;
        Die();
        UI();
    }
    public void Throw_Bomb()
    {
        if (bomb_obj == null || !Has_Bomb() || timer <= bomb_interval || dead) return;

        timer = 0f;
        bomb_number -= 1;
        Vector3 direction = Aim_Forward();
        direction.Normalize();

        GameObject throwed_bomb = Instantiate(bomb_obj, spawn_point.position, Quaternion.identity);
        Bomb bomb_code = throwed_bomb.GetComponent<Bomb>();
        Rigidbody rb = throwed_bomb.GetComponent<Rigidbody>();
        rb.AddForce(direction * throw_force, ForceMode.VelocityChange);

    }


    public void Get_Hit()
    {
        if (hit_interval >= hit_counter || dead || player_life <= 0) return;

        player_life -= 1;
        hit_counter = 0f;

        GameObject to_destroy = heart_parent.transform.GetChild(0).gameObject;
        if (to_destroy != null) Destroy(to_destroy);

        if (hit_vfx != null && hit_vfx_sp != null) Instantiate(hit_vfx, hit_vfx_sp.position, Quaternion.identity);
        if (player_life <= 0)
        {
            player_life = 0;
            dead = true;
        }
    }
    public void Get_Life()
    {
        if (player_life >= max_player_life) return;

        Instantiate(heart_obj, heart_parent);
        player_life++;

    }
    void Die()
    {
        if (!dead && Time.timeScale == 0f) Time.timeScale = 1f;
        if (!dead || Time.timeScale == 0f) return;
        if (death_obj != null && !death_obj.activeSelf) death_obj.SetActive(true);
    }
    bool Has_Bomb()
    {
        return (bomb_number > 0) ? true : false;
    }
    Vector3 Aim_Forward()
    {
        Vector2 center = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = Camera.main.ScreenPointToRay(center);
        return ray.direction;
    }
    void OnCollisionEnter(Collision collision)
    {
        Inhabitants inh = collision.gameObject.GetComponent<Inhabitants>();
        if (inh == null) return;
        if (inh.type == Inhabitants_Types.FARMERS || inh.type == Inhabitants_Types.FRIENDLY) return;
        Get_Hit();
    }
    void UI()
    {
        if (bomb_text == null || key_text == null) return;
        bomb_text.text = bomb_number.ToString();
        key_text.text = key_number.ToString();
    }
}