using UnityEngine;
using TMPro;

public class ParameterLoader : MonoBehaviour
{
    public Transform parameterArea;   // 指向 Phase1 的 ParameterArea
    public GameObject textPrefab;     // 一个简单的 TMP_Text Prefab

    void Start()
    {
        GenerateParameterTexts();
    }

    void GenerateParameterTexts()
    {
        // 清空旧的子对象  
        foreach (Transform child in parameterArea)
            Destroy(child.gameObject);

        // 从 UIManager 获取最终参数
        int finalE = UIManager.selectedStrategyData.E;
        int finalA = UIManager.selectedStrategyData.A;
        int finalP = UIManager.selectedStrategyData.P;
        int finalC = UIManager.selectedStrategyData.C;

        // 再叠加 starter cards 的效果
        foreach (StarterCard card in UIManager.selectedStarterCards)
        {
            finalE += card.E;
            finalA += card.A;
            finalP += card.P;
            finalC += card.C;
        }

        // 生成文本
        CreateLine($"Emotional Alignment… {finalE}");
        CreateLine($"Authenticity Vector… {finalA}");
        CreateLine($"Performance Bias… {finalP}");
        CreateLine($"Control Compression… {finalC}");
    }

    void CreateLine(string content)
    {
        GameObject t = Instantiate(textPrefab, parameterArea);
        t.GetComponent<TMP_Text>().text = content;
    }
}
