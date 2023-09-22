using UnityEngine;
using UnityEngine.UI;

public class SoundTest : MonoBehaviour
{
    [SerializeField]
    GameObject m_bgmButtons = null;
    [SerializeField]
    Button m_bgmStopButton = null;

    [SerializeField]
    GameObject m_seButtons = null;
    [SerializeField]
    Button m_seStopButton = null;

    void Start()
    {
        for (int i = 0; i < SoundType.BGMCount; i++)
        {
            var type = (SoundType.BGM)i;
            var button = CreateButton(m_bgmButtons, type.ToString());
            button.onClick.AddListener(
                () =>
                {
                    AudioManager.Instance.PlayBGM(type);
                });
        }
        for (int i = 0; i < SoundType.SECount; i++)
        {
            var type = (SoundType.SE)i;
            var button = CreateButton(m_seButtons, type.ToString());
            button.onClick.AddListener(
                () =>
                {
                    AudioManager.Instance.PlaySE(type);
                });
        }
        m_bgmStopButton.onClick.AddListener(
            () =>
            {
                AudioManager.Instance.StopBgm();
                //AudioManager.Instance.FadeOutBgm(0.5f);
            });
        m_seStopButton.onClick.AddListener(
            () =>
            {
                AudioManager.Instance.StopSE();
            });
    }

    /// <summary>
    /// Buttonを作成
    /// </summary>
    public Button CreateButton(GameObject parent, string nameAndText)
    {
        // Buttonオブジェクト作成
        GameObject obj = CreateUIObject(parent);
        obj.name = nameAndText;

        // RectTransformコンポーネント作成
        RectTransform rect = CreateRectTransform(obj);
        rect.sizeDelta = new Vector2(160f, 30f);

        // デフォルトのImageコンポーネント作成
        Image image = CreateDefaultImage(obj);

        // Buttonコンポーネント作成
        Button button = obj.AddComponent<Button>();
        button.targetGraphic = image;

        // ButtonのTextオブジェクト作成
        GameObject textObj = CreateUIObject(obj);
        textObj.name = "Text";

        // ButtonのTextのRectTransformコンポーネント作成
        CreateRectTransform(textObj);

        // ButtonのTextのTextコンポーネント作成
        Text text = textObj.AddComponent<Text>();
        text.color = Color.black;
        text.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        text.text = nameAndText;
        text.fontSize = 36;
        text.alignment = TextAnchor.MiddleCenter;

        return button;
    }

    /// <summary>
    /// UIオブジェクト作成
    /// </summary>
    private GameObject CreateUIObject(GameObject parent = null)
    {
        GameObject obj = new GameObject();
        if (parent)
            obj.transform.SetParent(parent.transform);
        obj.layer = LayerMask.NameToLayer("UI");
        return obj;
    }

    /// <summary>
    /// UI作成に必要なImageコンポーネント作成
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private Image CreateDefaultImage(GameObject obj)
    {
        Image image = obj.AddComponent<Image>();
        image.sprite = UnityEditor.AssetDatabase.GetBuiltinExtraResource<Sprite>("UI/Skin/UISprite.psd");
        image.type = Image.Type.Sliced;
        return image;
    }

    /// <summary>
    /// RectTransformコンポーネント作成
    /// </summary>
    private RectTransform CreateRectTransform(GameObject obj)
    {
        RectTransform rect = obj.AddComponent<RectTransform>();
        rect.anchorMin = Vector2.zero;
        rect.anchorMax = Vector2.one;
        rect.anchoredPosition = Vector2.zero;
        rect.sizeDelta = Vector2.zero;
        return rect;
    }
}
