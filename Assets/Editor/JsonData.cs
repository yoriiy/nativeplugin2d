using UnityEngine;

// ランキングデータ
[System.Serializable]
public class RankingData
{
    [SerializeField]
    public PersonalData[] ranking =  null;
}

// ランキングの中身データ
[System.Serializable]
public class PersonalData
{
    [SerializeField]
    public string name = "太郎さん";    // 名前
    [SerializeField, Range(1, 999999999)]
    public int point = 123456789;       // 得点
}
