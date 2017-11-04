using UnityEngine;

public class JsonScriptableObject : ScriptableObject
{
    [SerializeField]
    private RankingData m_jsonData = null;
    public RankingData Json
    {
        get { return m_jsonData;  }
        set { m_jsonData = value; }
    }
}
