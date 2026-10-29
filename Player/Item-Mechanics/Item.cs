using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class Item : MonoBehaviour
{
    public Item_Data data;
    public GameObject three_d_item;
    Player player;
    void Awake() => player = GameObject.Find("player").GetComponent<Player>();
    void Start()
    {
        Button b = GetComponent<Button>();
        b.onClick.AddListener(Use_Item);
        Button throw_b = transform.GetChild(2).GetComponent<Button>();
        throw_b.onClick.AddListener(Throw_Item);
    }

    public void Throw_Item()
    {
        Vector3 pos = player.transform.position + new Vector3(0f, 0f, 9f);
        Instantiate(three_d_item, pos, Quaternion.identity);
        Destroy(gameObject);
    }
    public void Use_Item()
    {
        if (data.type == Item_Data.Item_Type.LIFE) player.Get_Life();
        else if (data.type == Item_Data.Item_Type.BOMB) player.bomb_number++;
        else if (data.type == Item_Data.Item_Type.KEY) player.key_number++;
        Destroy(gameObject);
    }
    void Update()
    {
        Image icon = transform.GetChild(0).GetComponent<Image>();
        if (icon != null) icon.sprite = data.icon;
        TextMeshProUGUI _name = transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (_name != null) _name.text = data.item_name;

    }
}