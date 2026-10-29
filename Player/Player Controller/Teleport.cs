using UnityEngine;


public class Teleport : MonoBehaviour
{
    [SerializeField] float steps_number = 5f;
    bool teleportable = true;
    [SerializeField] float offset = 3f;
    public void Demon_Steps()
    {
        Vector3 to = transform.position + transform.forward * steps_number; ;
        bool is_on_ground = Physics.Raycast(transform.position, Vector3.down, 10f, ~0);
        bool is_safe = !is_on_ground || (Physics.Raycast(to, Vector3.down, 10f, ~0) && Physics.Raycast(to + new Vector3(0, 0, 5f), Vector3.down, 10f, ~0));
        teleportable = !Physics.Raycast(transform.position, transform.forward, steps_number + offset, ~0) && is_safe;
        if (teleportable) TP(to);

    }



    public void TP(Vector3 to)
    {
        gameObject.SetActive(false);
        transform.position = to;
        gameObject.SetActive(true);
    }
}