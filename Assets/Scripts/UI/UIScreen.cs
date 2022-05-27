using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum UIScreenType
{
    Default,
    Login,
    MainMenu,
    Achievements,
    Leaderboards,
    Save,
    GameStore
}

public class UIScreen : MonoBehaviour
{


    public virtual UIScreenType ScreenType { get => UIScreenType.Default; }
    public bool IsVisible { get; protected set; }

    protected virtual void Awake()
    {
        IsVisible = gameObject.activeSelf;
    }

    public virtual void Show()
    {
        CuConsole.Log(string.Format("Showing : {0}", gameObject.name), logArea: CuArea.UIScreen);
        gameObject.SetActive(true);
        IsVisible = true;
    }
    public virtual void Hide()
    {
        CuConsole.Log(string.Format("Hiding : {0}", gameObject.name), logArea: CuArea.UIScreen);
        gameObject.SetActive(false);
        IsVisible = false;
    }

    public void SetScreen(UIScreenType p_uiScreenType)
    {
        CuConsole.Log(string.Format("Set Screen called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(p_uiScreenType);
    }

    public void GoToMainMenu()
    {
        CuConsole.Log(string.Format("Go To Main Menu called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(UIScreenType.MainMenu);
    }

    public void GoToGameStore()
    {
        CuConsole.Log(string.Format("Go To Game Store called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(UIScreenType.GameStore);
    }

    public void GoToSaveScreen()
    {
        CuConsole.Log(string.Format("Go To Save Screen called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(UIScreenType.Save);
    }

    public void GoToAchievementsScreen()
    {
        CuConsole.Log(string.Format("Go To Achievements called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(UIScreenType.Achievements);
    }

    public void GoToLeaderboardsScreen()
    {
        CuConsole.Log(string.Format("Go To Leaderboards called."), logArea: CuArea.UIScreen);

        CloudTests.Instance.SetScreen(UIScreenType.Leaderboards);
    }

    public void Logout()
    {
        CuConsole.Log(string.Format("Logout called."), logArea: CuArea.UIScreen);

        GooglePlayManager.Instance.Logout();
        CloudTests.Instance.SetScreen(UIScreenType.Login);
    }
}
