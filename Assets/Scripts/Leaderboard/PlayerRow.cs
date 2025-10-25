using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Leaderboard.BestResultPopup
{
    public class PlayerRow : MonoBehaviour
    {
        [SerializeField] private Image m_Bg;
        
        [SerializeField] private Image m_TypeBg;
        [SerializeField] private Image m_AvatarImage;

        [SerializeField] private TMP_Text m_Nick;

        [SerializeField] private TMP_Text m_Points;
        
        private static readonly Dictionary<TypeLeague, Color> TypeColors = new Dictionary<TypeLeague, Color>
        {
            { TypeLeague.Diamond, new Color(0.725f, 0.849f, 1.0f) },   // Diamond (#B9F2FF)
            { TypeLeague.Gold,    new Color(1.0f, 0.875f, 0.369f) },   // Gold (#FFDF5E)
            { TypeLeague.Silver,  new Color(0.863f, 0.863f, 0.902f) }, // Silver (#DCDCE6)
            { TypeLeague.Bronze,  new Color(0.804f, 0.498f, 0.196f) }, // Bronze (#CD7F32)
            { TypeLeague.Default, new Color(0f, 0f, 0f, 0f) }  // Default (#F5F5F5)
        };
        
        private static readonly Dictionary<TypeLeague, float> TypeScales= new Dictionary<TypeLeague, float>
        {
            { TypeLeague.Diamond, 1.3f },
            { TypeLeague.Gold,    1.2f },
            { TypeLeague.Silver,  1.1f },
            { TypeLeague.Bronze,  1f },
            { TypeLeague.Default, 1f }
        };

        public void SetData(PlayerModel playerModel)
        {
            TypeLeague typeLeague = (TypeLeague)System.Enum.Parse(typeof(TypeLeague), playerModel.type);
            m_Nick.text = playerModel.name;
            m_Points.text = playerModel.score.ToString();
            m_TypeBg.color = TypeColors[typeLeague];
            transform.localScale = new Vector3(1, TypeScales[typeLeague], 1);
            ShowAvatar(playerModel.avatar);
        }

        private async void ShowAvatar(string url)
        {
            Sprite avatar = await LoadAvatarAsync(url);
            m_AvatarImage.sprite = avatar;
        }

        private async UniTask<Sprite> LoadAvatarAsync(string url)
        {
            using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(url))
            {
                await request.SendWebRequest().ToUniTask();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"Failed to load avatar: {request.error} ({url})");
                    return null;
                }

                Texture2D texture = DownloadHandlerTexture.GetContent(request);
                Sprite sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
                
                return sprite;
            }
        }
    }
    
    public enum TypeLeague
    {
        Diamond, Gold, Silver, Bronze, Default
    }
}

