using UnityEngine;

public class Item_Three_D : MonoBehaviour
{
    public Item_Data data;

    [SerializeField] Found_Item fi;
    bool picked_up = false;
    public GameObject ui_item_prefab;
    Transform inventory;
    GameObject player;
    void Start()
    {
        player = GameObject.Find("player");
        inventory = GameObject.FindWithTag("Inventory").transform;
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        fi = player.GetComponent<Player>().found_item;
    }

    public void Pick_Up()
    {
        if (picked_up) return;
        picked_up = true;
        Instantiate(ui_item_prefab, inventory);
        if (fi != null) fi.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject != player || fi == null) return;
        fi.three_d_item = this;
        if (!fi.gameObject.activeSelf) fi.gameObject.SetActive(true);

    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject != player || fi == null) return;
        fi.three_d_item = null;
        fi.gameObject.SetActive(false);
    }

}