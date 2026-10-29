using UnityEngine;



public class UI_Inputs_Features : MonoBehaviour
{
    Found_Item fi;
    void Awake()
    {
        fi = GetComponent<Player>().found_item;
    }
    public void Open_Close_Cursor()
    {
        if (fi.three_d_item != null)
        {
            fi.three_d_item.Pick_Up();
            return;
        }

        if (Cursor.visible)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }


}