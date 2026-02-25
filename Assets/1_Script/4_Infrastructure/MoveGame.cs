using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MoveGame : MonoBehaviour
{
    [SerializeField] Button moveButton;

    void Start()
    {
        moveButton.onClick.AddListener(() => SceneManager.LoadScene("Battle"));
    }
}
