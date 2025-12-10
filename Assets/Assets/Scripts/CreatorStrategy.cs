using UnityEngine;

[CreateAssetMenu(fileName = "NewStrategy", menuName = "SyntheticAffect/Creator Strategy")]
public class CreatorStrategy : ScriptableObject
{
    [Header("Basic Info")]
    public string strategyName;
    [TextArea(2, 4)]
    public string description;

    [Header("Initial Emotional Parameters")]
    public int E;   // Expressiveness
    public int A;   // Authenticity
    public int P;   // Productivity
    public int C;   // Creativity
}
