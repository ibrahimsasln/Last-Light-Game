using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Tilemania.UI
{
    public static class UIFactory
    {
        public static Canvas CreateCanvas(string name, int sortingOrder)
        {
            GameObject canvasObject = new GameObject(name, typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));

            Canvas canvas = canvasObject.GetComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = sortingOrder;

            CanvasScaler scaler = canvasObject.GetComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920f, 1080f);
            scaler.matchWidthOrHeight = 0.5f;

            return canvas;
        }

        public static RectTransform CreateRect(string name, Transform parent)
        {
            GameObject gameObject = new GameObject(name, typeof(RectTransform));
            gameObject.transform.SetParent(parent, false);
            return (RectTransform)gameObject.transform;
        }

        public static void Stretch(RectTransform rect)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = Vector2.zero;
            rect.offsetMax = Vector2.zero;
        }

        public static TextMeshProUGUI CreateText(Transform parent, UITheme theme, string text, float size, Color color)
        {
            TextMeshProUGUI label = CreateRect("Text", parent).gameObject.AddComponent<TextMeshProUGUI>();
            if (theme.Font != null)
            {
                label.font = theme.Font;
            }
            label.text = text;
            label.fontSize = size;
            label.color = color;
            label.alignment = TextAlignmentOptions.Center;
            label.raycastTarget = false;
            return label;
        }

        public static Button CreateButton(Transform parent, UITheme theme, string text, float height, UnityAction onClick)
        {
            RectTransform rect = CreateRect("Button", parent);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = height;

            Image background = rect.gameObject.AddComponent<Image>();
            background.color = theme.ButtonColor;

            Button button = rect.gameObject.AddComponent<Button>();
            button.targetGraphic = background;
            button.onClick.AddListener(onClick);

            TextMeshProUGUI label = CreateText(rect, theme, text, theme.ButtonTextSize, theme.TextColor);
            Stretch(label.rectTransform);

            return button;
        }

        public static Slider CreateSlider(Transform parent, UITheme theme, float value, UnityAction<float> onChanged)
        {
            RectTransform rect = CreateRect("Slider", parent);
            rect.gameObject.AddComponent<LayoutElement>().preferredHeight = theme.ButtonHeight * 0.5f;

            Image track = CreateRect("Track", rect).gameObject.AddComponent<Image>();
            track.color = theme.ButtonColor;
            Stretch(track.rectTransform);

            RectTransform fillArea = CreateRect("Fill Area", rect);
            Stretch(fillArea);
            Image fill = CreateRect("Fill", fillArea).gameObject.AddComponent<Image>();
            fill.color = theme.AccentColor;
            Stretch(fill.rectTransform);

            RectTransform handleArea = CreateRect("Handle Area", rect);
            Stretch(handleArea);
            Image handle = CreateRect("Handle", handleArea).gameObject.AddComponent<Image>();
            handle.color = theme.TextColor;
            handle.rectTransform.sizeDelta = new Vector2(theme.ButtonHeight * 0.5f, 0f);

            Slider slider = rect.gameObject.AddComponent<Slider>();
            slider.fillRect = fill.rectTransform;
            slider.handleRect = handle.rectTransform;
            slider.targetGraphic = handle;
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = value;
            slider.onValueChanged.AddListener(onChanged);

            return slider;
        }
    }
}
