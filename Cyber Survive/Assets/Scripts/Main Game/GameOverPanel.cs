using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text leaderboardText;
    [SerializeField] TMP_Text creditsText;

    public void SetTexts(string leaderString, int wave, int credits)
    {
        leaderboardText.text = leaderString;
        waveText.text = $"Вы дошли до {wave} волны";
        creditsText.text = $"Заработано за раунд: {credits} кредитов";
    }
    
}
