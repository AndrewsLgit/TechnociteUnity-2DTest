using UnityEngine;

[CreateAssetMenu(fileName = "ScoreSO", menuName = "Scriptable Objects/ScoreSO")]
public class ScoreSO : ScriptableObject
{
    public int m_score;
    public int m_maxScore;
    [SerializeField] private int _scoreIncrement = 10;

    public int Score => m_score;

    public void IncreaseScore()
    {
        m_score += _scoreIncrement;
        m_maxScore = m_maxScore < m_score ? m_score : m_maxScore;
    }
}
