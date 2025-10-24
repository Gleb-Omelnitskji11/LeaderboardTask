using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    private const string FilePath = "Leaderboard";
    private PlayerModel[] _players;
    
    private async void Start()
    {
        _players = await LoadLeaderboardAsync();

        if (_players == null)
        {
            Debug.LogError("No players data loaded");
            return;
        }

        foreach (var player in _players)
        {
            Debug.Log($"👤 Name: {player.playerName} | Score: {player.score} | Type: {player.type}");
        }
    }

    private async Task<PlayerModel[]> LoadLeaderboardAsync()
    {
        ResourceRequest request = Resources.LoadAsync<TextAsset>(FilePath);

        while (!request.isDone)
            await Task.Yield(); // чекаємо наступного кадру без блокування

        if (request.asset == null)
        {
            Debug.LogError($"Resources/{FilePath}.json not exists");
            return null;
        }

        TextAsset jsonFile = request.asset as TextAsset;
        if (jsonFile == null)
        {
            Debug.LogError("Wrong format");
            return null;
        }

        string wrappedJson = "{\"players\":" + jsonFile.text + "}";
        PlayerModel[] data = JsonConvert.DeserializeObject<PlayerModel[]>(wrappedJson);

        return data;
    }
}
