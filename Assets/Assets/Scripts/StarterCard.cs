using UnityEngine;

public enum CardType
{
    Starter,     // 起始卡
    Attribute,   // A 类 属性卡
    Burst,       // B 类 爆发卡
    System       // C 类 规则卡
}

[CreateAssetMenu(fileName = "NewStarterCard", menuName = "SyntheticAffect/Starter Card")]
public class StarterCard : ScriptableObject
{
    [Header("Basic Info")]
    public string cardName;

    [Header("Shop Info")]
    public CardType type = CardType.Attribute; // 默认先给 Attribute
    public int price = 3;                     // 默认商店价 

    [TextArea(2, 4)]
    public string description;

    [Header("Parameter Changes (E / A / P / C)")]
    public int E;
    public int A;
    public int P;
    public int C;

    [Header("Visual")]
    public Sprite cardImage;
}
