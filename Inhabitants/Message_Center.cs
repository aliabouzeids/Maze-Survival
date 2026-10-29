using UnityEngine;
using System.Collections.Generic;

public enum Emergency
{
    NOTHING,
    OBJ_AT_DANGER,
}
public class Message_Center : MonoBehaviour
{
    public Transform source;
    Transform player;
    public Emergency emergency;
    float emergency_distance = 10f;
    public Dictionary<int, I_Message> pending_messages;
    public List<Inhabitants> our_inhabitants;
    public int id;
    public int group_number;

    void Awake()
    {
        player = GameObject.Find("player").transform;
        pending_messages = new Dictionary<int, I_Message>();
    }
    public void Process_Messages(int who)
    {
        List<int> messages_processed = new List<int>();
        for (int x = 0; x < pending_messages.Count; x++)
        {
            if (who == x) continue;
            I_Message msg = pending_messages[x];
            foreach (Inhabitants i in our_inhabitants)
            {
                if (msg.is_empty())
                {

                    i.player_known_position = msg.player_position;
                    if (i.type == Inhabitants_Types.FARMERS || i.type == Inhabitants_Types.FRIENDLY) continue;
                    float distance = Vector3.Distance(player.position, source.position);
                    if (distance <= emergency_distance && emergency != Emergency.OBJ_AT_DANGER) emergency = Emergency.OBJ_AT_DANGER;

                }
            }
            messages_processed.Add(x);
        }
        foreach (int key in messages_processed)
        {
            pending_messages.Remove(key);
        }
    }
}