using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Leaderboard.BestResultPopup;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;
using Utils.PopupsUtils;

namespace Leaderboard
{
    public class LeaderboardPopupController : BasePopup
    {
        [SerializeField]
        private List<PlayerRow> m_BestResult;
        
        [SerializeField]
        private RectTransform m_PlayerList;
        
        [SerializeField]
        private GameObject m_Body;
        
        private const string FilePath = "Leaderboard";
        private PlayerModel[] _players;
    

        public override async Task Init(object param)
        {
            await LoadLeaderboardAsync();
            m_Body.SetActive(true);
            ShowPopup();
        }
        
        private async UniTask LoadLeaderboardAsync()
        {
            string key = "LeaderboardData";
            AsyncOperationHandle<TextAsset> handle = Addressables.LoadAssetAsync<TextAsset>(key);
            await handle.ToUniTask();

            if (handle.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"Failed to load Addressable asset: {key}");
                return;
            }

            var jsonFile = handle.Result;
            if (jsonFile == null)
            {
                Debug.LogError("Loaded asset is null");
                return;
            }

            try
            {
                _players = JsonConvert.DeserializeObject<PlayerListWrapper>(jsonFile.text).leaderboard;
                ShowLeaderboard();
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Error parsing leaderboard JSON: {ex.Message}");
            }
            finally
            {
                Addressables.Release(handle);
            }
        }

        private void ShowLeaderboard()
        {
            if (_players == null)
            {
                Debug.LogError("No players data loaded");
                return;
            }

            foreach (var player in _players)
            {
                Debug.Log($"👤 Name: {player.name} | Score: {player.score} | Type: {player.type}");
            }

            SetView();
        }


        private void SetView()
        {
            SetExistResult();
            TurnOffEmptyResult();
        }

        private void SetExistResult()
        {
            for (int i = 0; i < _players.Length; i++)
            {
                {
                    m_BestResult[i].SetData(_players[i]);
                }
            }
            
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_PlayerList);
        }

        private void TurnOffEmptyResult()
        {
            for (int i = _players.Length; i < m_BestResult.Count; i++)
            {
                m_BestResult[i].gameObject.SetActive(false);
            }
        }
    }
}