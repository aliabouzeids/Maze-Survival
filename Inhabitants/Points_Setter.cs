using UnityEngine;


public class Points_Setter : MonoBehaviour
{
    public Transform[] points_to_set;
    public Points points;
    void Awake()
    {
        points.points = points_to_set;
    }
}