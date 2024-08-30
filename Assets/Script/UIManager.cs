using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    PanelClass panel;

    [SerializeField] GameObject selectPanel;

    [SerializeField] GameObject _fadeIn;
    public GameObject FadeIn { get => _fadeIn; }
    [SerializeField] GameObject _fadeOut;
    public GameObject FadeOut { get => _fadeOut; }
    private void Start()
    {
        Text[] texts = new Text[4];
        for (int i = 0; i < 4; i++)
            texts[i] = selectPanel.transform.GetChild(i).GetComponent<Text>();
        GameObject m = selectPanel.transform.GetChild(4).gameObject;
        GameObject a = selectPanel.transform.GetChild(5).gameObject;
        GameObject t = selectPanel.transform.GetChild(6).gameObject;

        panel = new PanelClass(texts, m, a, t);
    }

    public void SelectObject(ObjectSO obj)
    {
        selectPanel.SetActive(true);
        panel.Name.text = $"Name : {obj.name}";
        panel.HP.text = $"HP : {obj.Hp.ToString()}";
        panel.Attack.text = $"ATK : {obj.Attack.ToString()}";
        panel.Step.text = $"SPD : {obj.Step.ToString()}";
        panel.MoveButton.GetComponent<Image>().color = obj.IsMove ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        panel.AttackButton.GetComponent<Image>().color = obj.IsAttack ? Color.white : new Color(0.5f, 0.5f, 0.5f);
        panel.TurnSkipButton.GetComponent<Image>().color = (!obj.IsMove && !obj.IsAttack) ? new Color(0.5f, 0.5f, 0.5f) : Color.white ;

        panel.MoveButton.transform.GetChild(0).gameObject.SetActive(obj.Type == ObjectType.Player);
        panel.MoveButton.GetComponent<Image>().enabled = obj.Type == ObjectType.Player;
        panel.AttackButton.transform.GetChild(0).gameObject.SetActive(obj.Type == ObjectType.Player);
        panel.AttackButton.GetComponent<Image>().enabled = obj.Type == ObjectType.Player;

    }

    public void SelectCancel()
    {
        selectPanel.SetActive(false);
    }
}

class PanelClass
{
    public Text Name;
    public Text HP;
    public Text Attack;
    public Text Step;
    public GameObject MoveButton;
    public GameObject AttackButton;
    public GameObject TurnSkipButton;

    public PanelClass(Text n, Text h, Text a, Text s)
    {
        Name = n;
        HP = h;
        Attack = a;
        Step = s;
    }
    public PanelClass(Text[] texts, GameObject m, GameObject a, GameObject t)
    {
        Name = texts[0];
        HP = texts[1];
        Attack = texts[2];
        Step = texts[3];
        MoveButton = m;
        AttackButton = a;
        TurnSkipButton = t;
    }
}