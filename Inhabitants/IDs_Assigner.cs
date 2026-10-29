using UnityEngine;
using System.Collections.Generic;


public class IDs_Assigner : MonoBehaviour
{
    int id = 0;
    public int group_number = 5;
    public Dictionary<int, Message_Center> centers = new Dictionary<int, Message_Center>();

    public bool initialized = false;

    public void Assign()
    {
        if (initialized) return;
        GameObject center_container = new GameObject();
        List<Inhabitants> inhabitants = new List<Inhabitants>();
        inhabitants.AddRange(FindObjectsByType<Inhabitants>(FindObjectsSortMode.None));
        List<Inhabitants> aggressive_inhabitants = new List<Inhabitants>();
        List<Inhabitants> guardians_inhabitants = new List<Inhabitants>();
        List<Inhabitants> friendly_inhabitants = new List<Inhabitants>();
        List<Inhabitants> farmers_inhabitants = new List<Inhabitants>();

        foreach (Inhabitants i in inhabitants)
        {
            if (i.type == Inhabitants_Types.FARMERS) farmers_inhabitants.Add(i);
            else if (i.type == Inhabitants_Types.FRIENDLY) friendly_inhabitants.Add(i);
            else if (i.type == Inhabitants_Types.AGGRESSIVE) aggressive_inhabitants.Add(i);
            else if (i.type == Inhabitants_Types.GUARDIAN) guardians_inhabitants.Add(i);
        }
        ///=====================================================================================
        ///=====================================================================================
        ///============================CONNECTING THEM UP=======================================
        ///=====================================================================================
        ///=====================================================================================

        for (int i = 0; i < farmers_inhabitants.Count; i += group_number)
        {
            Message_Center msg_center = center_container.AddComponent<Message_Center>();
            if (msg_center.our_inhabitants == null) msg_center.our_inhabitants = new List<Inhabitants>();
            msg_center.id = id;
            Add_Center(id, msg_center);
            for (int j = 0; j < group_number; j++)
            {
                if (i + j >= farmers_inhabitants.Count) break;
                msg_center.our_inhabitants.Add(farmers_inhabitants[i + j]);
                msg_center.group_number = group_number;
                msg_center.source = msg_center.our_inhabitants[0].transform;
                farmers_inhabitants[i + j].msg_center = msg_center;
                farmers_inhabitants[i + j].grp_id = id;
                farmers_inhabitants[i + j].id = msg_center.our_inhabitants.Count - 1;

            }
            id++;
            Debug.Log($"Assigned ${msg_center} to ${id}");
        }
        for (int i = 0; i < aggressive_inhabitants.Count; i += group_number)
        {
            Message_Center msg_center = center_container.AddComponent<Message_Center>();
            if (msg_center.our_inhabitants == null) msg_center.our_inhabitants = new List<Inhabitants>();
            msg_center.id = id;
            Add_Center(id, msg_center);
            for (int j = 0; j < group_number; j++)
            {
                if (i + j >= aggressive_inhabitants.Count) break;
                msg_center.our_inhabitants.Add(aggressive_inhabitants[i + j]);
                msg_center.group_number = group_number;
                msg_center.source = msg_center.our_inhabitants[0].transform;
                aggressive_inhabitants[i + j].msg_center = msg_center;
                aggressive_inhabitants[i + j].grp_id = id;
                aggressive_inhabitants[i + j].id = msg_center.our_inhabitants.Count - 1;
            }
            id++;
        }
        for (int i = 0; i < friendly_inhabitants.Count; i += group_number)
        {
            Message_Center msg_center = center_container.AddComponent<Message_Center>();
            if (msg_center.our_inhabitants == null) msg_center.our_inhabitants = new List<Inhabitants>();
            msg_center.id = id;
            Add_Center(id, msg_center);
            for (int j = 0; j < group_number; j++)
            {
                if (i + j >= friendly_inhabitants.Count) break;
                msg_center.our_inhabitants.Add(friendly_inhabitants[i + j]);
                msg_center.group_number = group_number;
                msg_center.source = msg_center.our_inhabitants[0].transform;
                friendly_inhabitants[i + j].msg_center = msg_center;
                friendly_inhabitants[i + j].grp_id = id;
                friendly_inhabitants[i + j].id = msg_center.our_inhabitants.Count - 1;
            }
            id++;
        }
        for (int i = 0; i < guardians_inhabitants.Count; i += group_number)
        {
            Message_Center msg_center = center_container.AddComponent<Message_Center>();
            if (msg_center.our_inhabitants == null) msg_center.our_inhabitants = new List<Inhabitants>();
            msg_center.id = id;
            Add_Center(id, msg_center);
            for (int j = 0; j < group_number; j++)
            {
                if (i + j >= guardians_inhabitants.Count) break;
                msg_center.group_number = group_number;
                msg_center.our_inhabitants.Add(guardians_inhabitants[i + j]);
                msg_center.source = msg_center.our_inhabitants[0].transform;
                guardians_inhabitants[i + j].msg_center = msg_center;
                guardians_inhabitants[i + j].grp_id = id;
                guardians_inhabitants[i + j].id = msg_center.our_inhabitants.Count - 1;
            }
            id++;
        }
        initialized = true;

    }
    void Add_Center(int _id, Message_Center _center)
    {
        centers.Add(_id, _center);
    }
}