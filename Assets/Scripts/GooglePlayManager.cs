using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GooglePlayManager : Singleton<GooglePlayManager>
{

    private bool m_bIsLoggedIn;

    protected override void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    private void Start()
    {
        Login();
    }

    public void Login()
    {
        CustomConsole.Log("Login Pressed");

        PlayGamesPlatform.Instance.Authenticate(SignInInteractivity.CanPromptOnce, (result) =>
        {
            CustomConsole.Log(string.Format("Authenticate Result: {0}", result.ToString()));
            switch (result)
            {
                case SignInStatus.Success:
                    m_bIsLoggedIn = true;
                    break;
                default:
                    m_bIsLoggedIn = false;
                    break;
            }
        });
    }

    public void Logout()
    {
        CustomConsole.Log("Logout Pressed");
    }

    public void SaveGame()
    {
        CustomConsole.Log("Save Game Pressed");
    }

    public void LoadGame()
    {
        CustomConsole.Log("Load game Pressed");
    }
}
