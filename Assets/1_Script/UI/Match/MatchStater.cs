using UnityEngine;
using UnityEngine.UI;

public class MatchStater : MonoBehaviour
{
    [SerializeField] MatchDI match;
    [SerializeField] Button blueButton;
    [SerializeField] Button redButton;

    void Start()
    {
        blueButton.onClick.AddListener(() => ChoiceTeam(Team.Blue));
        redButton.onClick.AddListener(() => ChoiceTeam(Team.Red));
    }

    void ChoiceTeam(Team team)
    {
        match.GameStart(team);
        Destroy(gameObject);
    }
}
