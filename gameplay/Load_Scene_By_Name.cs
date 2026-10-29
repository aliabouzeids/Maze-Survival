using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;



public class Load_Scene_By_Name : MonoBehaviour
{
    [SerializeField] string scene_name;
    [SerializeField] float wait_for = 1f;
    void Awake()
    {
        Button b = GetComponent<Button>();
        if (b != null) b.onClick.AddListener(Load_Scene);
    }
    public void Load_Scene()
    {
        Time.timeScale = 1f;
        
        if (wait_for > 0f) StartCoroutine(Load_After());
        else SceneManager.LoadScene(scene_name);

    }
    IEnumerator Load_After()
    {
        yield return new WaitForSeconds(wait_for);
        SceneManager.LoadScene(scene_name);
    }
}