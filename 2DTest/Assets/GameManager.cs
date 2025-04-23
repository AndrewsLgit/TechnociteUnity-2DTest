using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Private

    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private ScoreSO _scoreSO;

    #endregion

    #region Main Methods
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //_scoreSO = Instantiate(_scoreSO);
        //_scoreSO.m_score = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    #endregion

    #region Utils

    public void OnScore()
    {
        _scoreSO.IncreaseScore();
        _scoreText.text = $"Score: {_scoreSO.Score}";
    }

    #endregion
}
