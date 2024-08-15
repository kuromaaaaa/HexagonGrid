using UnityEngine;
using UnityEngine.UI;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    PanelClass panel;

    [SerializeField] GameObject selectPanel;
    private void Start()
    {
        Text[] texts = new Text[4];
        for (int i = 0; i < 4; i++)
            texts[i] = selectPanel.transform.GetChild(i).GetComponent<Text>();
        GameObject m = selectPanel.transform.GetChild(4).gameObject;
        GameObject a = selectPanel.transform.GetChild(5).gameObject;

        panel = new PanelClass(texts, m, a);
    }

    public void SelectObject(ObjectSO obj)
    {
        selectPanel.SetActive(true);
        panel.Name.text = $"Name : {obj.name}";
        panel.HP.text = $"HP : {obj.Hp.ToString()}";
        panel.Attack.text = $"ATK : {obj.Attack.ToString()}";
        panel.Step.text = $"SPD : {obj.Step.ToString()}";
        panel.MoveButton.GetComponent<Image>().color = obj.IsMove ? Color.white : new Color(0.5f,0.5f,0.5f);
        panel.AttackButton.GetComponent<Image>().color = obj.IsAttack ? Color.white: new Color(0.5f, 0.5f, 0.5f);
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
    public PanelClass(Text n, Text h, Text a, Text s)
    {
        Name = n;
        HP = h;
        Attack = a;
        Step = s;
    }
    public PanelClass(Text[] texts, GameObject m, GameObject a)
    {
        Name = texts[0];
        HP = texts[1];
        Attack = texts[2];
        Step = texts[3];
        MoveButton = m;
        AttackButton = a;
    }
}