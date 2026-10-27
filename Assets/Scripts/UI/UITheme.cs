using TMPro;
using UnityEngine;

namespace Tilemania.UI
{
    [CreateAssetMenu(fileName = "UITheme", menuName = "Tilemania/UI Theme")]
    public class UITheme : ScriptableObject
    {
        [Header("Font")]
        [SerializeField] TMP_FontAsset font;
        [SerializeField] float titleSize = 64f;
        [SerializeField] float textSize = 32f;
        [SerializeField] float buttonTextSize = 32f;

        [Header("Colors")]
        [SerializeField] Color overlayColor = new Color(0f, 0f, 0f, 0.6f);
        [SerializeField] Color panelColor = new Color(0.12f, 0.1f, 0.16f, 0.95f);
        [SerializeField] Color buttonColor = new Color(0.33f, 0.25f, 0.42f, 1f);
        [SerializeField] Color textColor = Color.white;
        [SerializeField] Color accentColor = new Color(1f, 0.84f, 0.3f, 1f);

        [Header("Sprites")]
        [SerializeField] Sprite panelSprite;
        [SerializeField] Sprite buttonSprite;
        [SerializeField, Min(0.01f)] float pixelsPerUnitMultiplier = 1f;

        [Header("Layout")]
        [SerializeField] float windowWidth = 760f;
        [SerializeField] float buttonHeight = 72f;
        [SerializeField] int padding = 40;
        [SerializeField] float spacing = 16f;

        public TMP_FontAsset Font => font;
        public float TitleSize => titleSize;
        public float TextSize => textSize;
        public float ButtonTextSize => buttonTextSize;
        public Color OverlayColor => overlayColor;
        public Color PanelColor => panelColor;
        public Color ButtonColor => buttonColor;
        public Color TextColor => textColor;
        public Color AccentColor => accentColor;
        public Sprite PanelSprite => panelSprite;
        public Sprite ButtonSprite => buttonSprite;
        public float PixelsPerUnitMultiplier => pixelsPerUnitMultiplier;
        public float WindowWidth => windowWidth;
        public float ButtonHeight => buttonHeight;
        public int Padding => padding;
        public float Spacing => spacing;
    }
}
