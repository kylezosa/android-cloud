using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardsScreen : UIScreen
{
    [SerializeField] private TMP_InputField m_scoreInputField = null;
    public override UIScreenType ScreenType { get => UIScreenType.Leaderboards; }

    public void ShowLeaderboards()
    {
        CuConsole.Log(string.Format("Show Leaderboards."), logArea: CuArea.LeaderboardsScreen);

        // Social.ShowLeaderboardUI();
        GooglePlayManager.Instance.ShowLeaderboardUI(GPGSIds.leaderboard_cloud_leaderboard);
    }

    public void SubmitScore()
    {
        CuConsole.Log(string.Format("Submit Score called."), logArea: CuArea.LeaderboardsScreen);
        
        if (Int32.TryParse(m_scoreInputField.text, out int score))
        {
            Social.ReportScore(score, GPGSIds.leaderboard_cloud_leaderboard, (success) =>
            {
                if (success)
                {
                    CuConsole.Log(string.Format("Submit Score successful!"), logArea: CuArea.LeaderboardsScreen);
                }
                else
                {
                    CuConsole.Log(string.Format("Submit Score failed."), CuLogType.Error, CuArea.LeaderboardsScreen);
                }
            });
        }
        else
        {
            CuConsole.Log(string.Format("Input score is not a number."), CuLogType.Error, CuArea.LeaderboardsScreen);
        }
    }
}
