using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using GooglePlayGames.BasicApi.SavedGame;

public class SaveScreen : UIScreen
{
    [SerializeField] private TMP_InputField m_descriptionInputField;
    [SerializeField] private TextMeshProUGUI m_loadGameTextField;
    public override UIScreenType ScreenType { get => UIScreenType.Save; }

    public void SaveGame()
    {
        CuConsole.Log(string.Format("Save Game called."), logArea: CuArea.SaveScreen);

        CloudSaveManager.Instance.SaveGame(m_descriptionInputField.text, (status) =>
        {
            CuConsole.Log(string.Format("Load Game callback: {0}", status), logArea: CuArea.SaveScreen);

            if (status == SavedGameRequestStatus.Success)
            {
                LoadGame();
            }
        });
    }

    public void LoadGame()
    {
        CuConsole.Log(string.Format("Load Game called."), logArea: CuArea.SaveScreen);

        CloudSaveManager.Instance.LoadGame((status) =>
        {
            CuConsole.Log(string.Format("Load Game callback: {0}", status), logArea: CuArea.SaveScreen);

            if (status == SavedGameRequestStatus.Success)
            {
                m_loadGameTextField.text = string.Format("Timestamp: {0}, Save Count: {1}\nDescription: {2}",
                    CloudSaveManager.Instance.State.TimeStamp,
                    CloudSaveManager.Instance.State.SaveCount,
                    CloudSaveManager.Instance.State.Description);
            }
        });
    }

    public override void Show()
    {
        base.Show();

        LoadGame();
    }
}
