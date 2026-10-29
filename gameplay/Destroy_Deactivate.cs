using UnityEngine;



enum for_what
{
    DESTROY,
    DEACTIVATE,
}


public class Destroy_Deactivate : MonoBehaviour
{
    [SerializeField] for_what role;
    float timer = 0f;
    [SerializeField] float time_interval = 3f;
    void Awake()
    {
        timer = 0f;
    }
    void Update()
    {
        Do_Role();
    }
    void Do_Role()
    {
        timer += Time.deltaTime;

        if (timer <= time_interval) return;
        time_interval = 0f;
        if (role == for_what.DESTROY) Destroy(gameObject);
        else if (role == for_what.DEACTIVATE) gameObject.SetActive(false);
    }
}