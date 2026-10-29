using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Found_Item : MonoBehaviour
{
    public Item_Three_D three_d_item;
    [SerializeField] Image icon;
    [SerializeField] TextMeshProUGUI item_name;
    Button item_b;

    void Start()
    {
        item_b = transform.GetChild(0).GetComponent<Button>();
        item_b.onClick.AddListener(() => three_d_item.Pick_Up());
    }
    void Update()
    {
        if (three_d_item == null) return;
        icon = item_b.transform.GetChild(0).GetComponent<Image>();
        item_name = item_b.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
        if (icon != null) icon.sprite = three_d_item.data.icon;
        if (item_name != null) item_name.text = three_d_item.data.item_name;
    }
}