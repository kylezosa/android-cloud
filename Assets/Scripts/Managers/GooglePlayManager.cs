using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.Purchasing;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class GooglePlayManager : Singleton<GooglePlayManager>
{
    private bool m_bAttemptingLogin = false;

    public Action<bool, string> OnLoginAttemptComplete;
    public Action OnLogout;

    public const string CLOUD_SAVE_NAME = "CloudTest_SaveName";

    // Properties
    public bool AttemptingLogin { get => m_bAttemptingLogin; private set => m_bAttemptingLogin = value; }

    protected override void Awake()
    {
        base.Awake();

        PlayGamesClientConfiguration.Builder builder = new PlayGamesClientConfiguration.Builder();

        PlayGamesClientConfiguration config = builder
            .EnableSavedGames()
            .Build();

        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }
    public void Login(Action<bool> callback = null)
    {
        Login((success, message) =>
        {
            callback?.Invoke(success);
        });
    }
    public void Login(Action<bool, string> callback = null)
    {
        CuConsole.Log("Login Called", logArea: CuArea.GooglePlayManager);

        if (AttemptingLogin)
        {
            CuConsole.Log("Login is still being attempted.", logArea: CuArea.GooglePlayManager);
        }
        else
        {
            try
            {
                if (!Social.localUser.authenticated)
                {
                    AttemptingLogin = true;

                    CuConsole.Log("Attempting to Login.", logArea: CuArea.GooglePlayManager);

                    Social.localUser.Authenticate((success, message) =>
                    {
                        AttemptingLogin = false;
                        if (success)
                        {
                            CuConsole.Log(string.Format("Login Successful: {0}", message), logArea: CuArea.GooglePlayManager);
                            CloudSaveManager.Instance.LoadFromCloud();
                        }
                        else
                        {
                            CuConsole.Log(string.Format("Login Failed: {0}", message), logArea: CuArea.GooglePlayManager);
                        }

                        callback?.Invoke(success, message);
                        OnLoginAttemptComplete?.Invoke(success, message);
                    });
                }
                else
                {
                    CuConsole.Log("Player is already logged in.", logArea: CuArea.GooglePlayManager);
                }
            }
            catch (Exception e)
            {
                AttemptingLogin = false;
                CuConsole.Log(e, logArea: CuArea.GooglePlayManager);
            }
        }
    }
    public void Logout()
    {
        CuConsole.Log("Logout Called", logArea: CuArea.GooglePlayManager);

        if (Social.localUser.authenticated)
        {
            CuConsole.Log("Logging out", logArea: CuArea.GooglePlayManager);

            PlayGamesPlatform.Instance.SignOut();
            OnLogout?.Invoke();
        }
        else
        {
            CuConsole.Log("No player is logged in.", logArea: CuArea.GooglePlayManager);
        }
    }
    public void OpenCloudSave(Action<SavedGameRequestStatus, ISavedGameMetadata> callback, Action errorCallback = null)
    {
        CuConsole.Log("Open Cloud Save called.", logArea: CuArea.GooglePlayManager);

        if (!Social.localUser.authenticated)
        {
            CuConsole.Log("Player is not authenticated.", logArea: CuArea.GooglePlayManager);
        }

        if (!PlayGamesClientConfiguration.DefaultConfiguration.EnableSavedGames)
        {
            CuConsole.Log("Save Games is not Enabled.", logArea: CuArea.GooglePlayManager);
        }

        try
        {
            PlayGamesPlatform platform = (PlayGamesPlatform)Social.Active;

            platform.SavedGame.OpenWithAutomaticConflictResolution(
                CLOUD_SAVE_NAME,
                DataSource.ReadNetworkOnly,
                ConflictResolutionStrategy.UseLongestPlaytime,
                callback
                );
        }
        catch (Exception e)
        {
            errorCallback?.Invoke();
            CuConsole.Log(e, CuLogType.Error, CuArea.GooglePlayManager);
        }
    }
    public void LoadAchievements()
    {
    }

    public void IncrementAchievement(string id, int steps, Action<bool> callback)
    {
        CuConsole.Log(string.Format("Increment Achievement called."), logArea: CuArea.GooglePlayManager);

        PlayGamesPlatform.Instance.IncrementAchievement(id, steps, callback);
    }

    public void ShowLeaderboardUI(string id)
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI(id);
    }
}
