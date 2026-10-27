using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Tilemania.UI
{
    public class MenuWindow
    {
        readonly UITheme theme;
        readonly GameObject root;
        readonly RectTransform content;
        Button firstButton;

        public event Action ButtonClicked;

        public MenuWindow(Transform canvas, UITheme theme)
        {
            this.theme = theme;

            RectTransform overlay = UIFactory.CreateRect("Menu Overlay", canvas);
            UIFactory.Stretch(overlay);
            overlay.gameObject.AddComponent<Image>().color = theme.OverlayColor;
            root = overlay.gameObject;

            content = UIFactory.CreateRect("Window", overlay);
            content.sizeDelta = new Vector2(theme.WindowWidth, 0f);
            UIFactory.AddImage(content.gameObject, theme, theme.PanelSprite, theme.PanelColor);

            VerticalLayoutGroup layout = content.gameObject.AddComponent<VerticalLayoutGroup>();
            layout.padding = new RectOffset(theme.Padding, theme.Padding, theme.Padding, theme.Padding);
            layout.spacing = theme.Spacing;
            layout.childAlignment = TextAnchor.UpperCenter;
            layout.childControlWidth = true;
            layout.childControlHeight = true;
            layout.childForceExpandWidth = true;
            layout.childForceExpandHeight = false;

            content.gameObject.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            root.SetActive(false);
        }

        public void Open(string title)
        {
            Clear();
            root.SetActive(true);
            AddText(title, theme.TitleSize, theme.AccentColor);
        }

        public void Close()
        {
            root.SetActive(false);
        }

        public TextMeshProUGUI AddText(string text)
        {
            return AddText(text, theme.TextSize, theme.TextColor);
        }

        public TextMeshProUGUI AddText(string text, float size, Color color)
        {
            return UIFactory.CreateText(content, theme, text, size, color);
        }

        public Button AddButton(string label, Action onClick, bool interactable = true, float heightMultiplier = 1f)
        {
            Button button = UIFactory.CreateButton(content, theme, label, theme.ButtonHeight * heightMultiplier, () =>
            {
                ButtonClicked?.Invoke();
                onClick();
            });
            button.interactable = interactable;

            if (firstButton == null && interactable)
            {
                firstButton = button;
                EventSystem.current?.SetSelectedGameObject(button.gameObject);
            }
            return button;
        }

        public Slider AddSlider(float value, Action<float> onChanged)
        {
            return UIFactory.CreateSlider(content, theme, value, newValue => onChanged(newValue));
        }

        private void Clear()
        {
            firstButton = null;
            foreach (Transform child in content)
            {
                child.gameObject.SetActive(false);
                Object.Destroy(child.gameObject);
            }
        }
    }
}
