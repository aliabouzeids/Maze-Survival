using UnityEngine;


public class Finish_Round : MonoBehaviour
{
    [SerializeField] GameObject finish_round;
    [SerializeField] GameObject closed_text;
    [SerializeField] Transform parent;
    Player player;
    void Awake() => player = GameObject.Find("player").GetComponent<Player>();
    public void Open()
    {
        if (player.key_number <= 0)
        {
            Instantiate(closed_text, parent);
            return;
        }
        finish_round.SetActive(true);
    }




}