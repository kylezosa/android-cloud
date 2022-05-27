using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsScreen : UIScreen
{
    public override UIScreenType ScreenType { get => UIScreenType.Achievements; }

    public void ShowAchievements()
    {
        CuConsole.Log(string.Format("Show Achievements called."), logArea: CuArea.AchievementsScreen);

        Social.ShowAchievementsUI();
    }

    public void EarnAchievement()
    {
        CuConsole.Log(string.Format("Earn Achievement called."), logArea: CuArea.AchievementsScreen);

        Social.ReportProgress(GPGSIds.achievement_achieve_achievement, 100.0d, (success) =>
        {
            if (success)
            {
                CuConsole.Log(string.Format("Earn Achievement successful!"), logArea: CuArea.AchievementsScreen);
            }
            else
            {
                CuConsole.Log(string.Format("Earn Achievement failed."), CuLogType.Error, CuArea.AchievementsScreen);
            }
        });
    }

    public void IncrementAchievement()
    {
        CuConsole.Log(string.Format("Increment Achievement called."), logArea: CuArea.AchievementsScreen);

        GooglePlayManager.Instance.IncrementAchievement(GPGSIds.achievement_incremental_achievement, 10, (success) =>
        {
            if (success)
            {
                CuConsole.Log(string.Format("Increment Achievement successful!"), logArea: CuArea.AchievementsScreen);
            }
            else
            {
                CuConsole.Log(string.Format("Increment Achievement failed."), CuLogType.Error, CuArea.AchievementsScreen);
            }
        });
    }
}
