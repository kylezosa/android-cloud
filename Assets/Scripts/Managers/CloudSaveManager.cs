using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using GooglePlayGames;
using GooglePlayGames.BasicApi.SavedGame;

public class CloudSaveManager : Singleton<CloudSaveManager>
{
    private SaveState state = new SaveState();

    private bool m_bAttemptingSave = false;
    private bool m_bAttemptingLoad = false;

    // Events
    public Action OnSaveAttemptStart;
    public Action<SavedGameRequestStatus> OnSaveAttemptComplete;

    public Action OnLoadAttemptStart;
    public Action<SavedGameRequestStatus> OnLoadAttemptComplete;

    // Properties
    public SaveState State { get => state; private set => state = value; }
    public bool AttemptingSave { get => m_bAttemptingSave; private set => m_bAttemptingSave = value; }
    public bool AttemptingLoad { get => m_bAttemptingLoad; private set => m_bAttemptingLoad = value; }

    // Methods

    // Serialize/Deserialize
    private byte[] SerializeState()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream())
        {
            binaryFormatter.Serialize(memoryStream, state);
            return memoryStream.GetBuffer();
        }
    }
    private SaveState DeserializeState(byte[] data)
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();

        using (MemoryStream memoryStream = new MemoryStream(data))
        {
            return (SaveState) binaryFormatter.Deserialize(memoryStream);
        }
    }


    // Save/Load
    public void SaveToCloud(Action<SavedGameRequestStatus> callback = null)
    {
        CuConsole.Log("Save to Cloud called.", logArea: CuArea.SaveManager);

        if (AttemptingSave)
        {
            CuConsole.Log("Saving still in progress.", logArea: CuArea.SaveManager);
        }
        else
        {
            AttemptingSave = true;
            OnSaveAttemptStart?.Invoke();

            GooglePlayManager.Instance.OpenCloudSave((status, meta) =>
            {
                OnCloudSave(status, meta);
                callback?.Invoke(status);
            }, () =>
            {
                AttemptingSave = false;
            });
        }
    }
    private void OnCloudSave(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        CuConsole.Log(string.Format("On Cloud Save called: {0}", status.ToString()), logArea: CuArea.SaveManager);

        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = SerializeState();

            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedDescription(string.Format("Last save : {0}", DateTime.Now))
                .Build();

            PlayGamesPlatform platform = (PlayGamesPlatform) Social.Active;
            platform.SavedGame.CommitUpdate(meta, update, data, SaveCallback);

            CuConsole.Log(string.Format("Saving State Count: {0}, Text: {1}", state.SaveCount, state.TimeStamp), logArea: CuArea.SaveManager);
            CuConsole.Log(string.Format("Saved Byte Array Length: {0}", data?.Length), logArea: CuArea.SaveManager);
        }
        else
        {
            AttemptingSave = false;
            OnSaveAttemptComplete?.Invoke(status);
        }
    }
    private void SaveCallback(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        CuConsole.Log(string.Format("Save Callback called: {0}", status.ToString()), logArea: CuArea.SaveManager);
        AttemptingSave = true;

        PlayGamesPlatform platform = (PlayGamesPlatform) Social.Active;

        OnSaveAttemptComplete?.Invoke(status);
    }

    // Load Functions
    public void LoadFromCloud(Action<SavedGameRequestStatus> callback = null)
    {
        CuConsole.Log(string.Format("Load From Cloud called."), logArea: CuArea.SaveManager);

        if (AttemptingLoad)
        {
            CuConsole.Log("Loading still in progress.", logArea: CuArea.SaveManager);
        }
        else
        {
            AttemptingLoad = true;
            GooglePlayManager.Instance.OpenCloudSave((status, meta) =>
            {
                OnCloudLoad(status, meta);
                callback?.Invoke(status);
            }, () =>
            {
                AttemptingLoad = false;
            });
        }
    }
    private void OnCloudLoad(SavedGameRequestStatus status, ISavedGameMetadata meta)
    {
        CuConsole.Log(string.Format("On Cloud Load called: {0}", status.ToString()), logArea: CuArea.SaveManager);

        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform platform = (PlayGamesPlatform) Social.Active;
            platform.SavedGame.ReadBinaryData(meta, LoadCallback);
        }
        else
        {
            AttemptingLoad = false;
            OnLoadAttemptComplete?.Invoke(status);
        }
    }
    private void LoadCallback(SavedGameRequestStatus status, byte[] data)
    {
        CuConsole.Log(string.Format("Load Callback called: {0}, Data: {1}, Length: {2}", status, data, data?.Length), logArea: CuArea.SaveManager);
        AttemptingLoad = false;

        if (data.Length > 0)
        {
            CuConsole.Log(data, logArea: CuArea.SaveManager);
            state = DeserializeState(data);
            CuConsole.Log(string.Format("Loaded State Count: {0}, Text: {1}", state.SaveCount, state.TimeStamp), logArea: CuArea.SaveManager);
        }
        else
        {
            CuConsole.Log(string.Format("No data found."), logArea: CuArea.SaveManager);
        }

        OnLoadAttemptComplete?.Invoke(status);
    }
    public void SaveGame(string description, Action<SavedGameRequestStatus> callback = null)
    {
        CuConsole.Log(string.Format("Save Game called."), logArea: CuArea.CloudTests);

        CloudSaveManager.Instance.State.TimeStamp = DateTime.Now;
        CloudSaveManager.Instance.State.SaveCount++;
        CloudSaveManager.Instance.State.Description = description;

        CloudSaveManager.Instance.SaveToCloud(callback);
    }

    public void LoadGame(Action<SavedGameRequestStatus> callback = null)
    {
        CuConsole.Log(string.Format("Load Game called."), logArea: CuArea.CloudTests);

        CloudSaveManager.Instance.LoadFromCloud(callback);
    }
}
