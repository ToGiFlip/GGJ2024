using System.Globalization;
using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [SerializeField] private GameObject _hideIfNoScore;

    [SerializeField] private TextMeshProUGUI _scoreValue;
    [SerializeField] private TextMeshProUGUI _averageValue;

    private void Awake()
    {
        if (ScoreManager.LastScore == null)
        {
            _hideIfNoScore.SetActive(false);
            return;
        }

        _scoreValue.text = ScoreManager.LastScore.Value.ToString(CultureInfo.InvariantCulture);
        _averageValue.text = ScoreManager.AverageScore.ToString(CultureInfo.InvariantCulture);
        _hideIfNoScore.SetActive(true);
    }
}