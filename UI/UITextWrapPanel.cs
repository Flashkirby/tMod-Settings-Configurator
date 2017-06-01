using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FKTModSettings.UI
{
    internal class UITextWrapPanel : UIPanelSortable
    {
        private UIText uiText;
        private int _maxLines = 100;

        private string _text = "";
        private float _textScale = 1f;
        private string[] _textInLines;
        public string Text
        {
            get { return _text; }
        }
        public int TextHeight
        {
            get
            {
                if (_text == "") return 0;
                int noOfLines = 0;
                string[] array = Utils.WordwrapString(_text, Main.fontMouseText, (int)Width.Pixels, _maxLines, out noOfLines);
                noOfLines++;
                return noOfLines * 30;
            }
        }

        public UITextWrapPanel()
        {
            Width.Set(0, 1f);
            MinHeight.Set(22f, 0f);

            _text = "";
            _textInLines = new string[1];
            uiText = new UIText(_text, 0.9f, false);
            base.Append(uiText);
        }

        public void SetText(string text)
        {
            _text = text;
            RecalculateTextWrap();
        }
        public void SetScale(float scale)
        {
            _textScale = scale;
            RecalculateTextWrap();
        }
        public void SetTextScale(string text, float scale)
        {
            _text = text;
            _textScale = scale;
            RecalculateTextWrap();
        }

        public override void Recalculate()
        {
            base.Recalculate();
            RecalculateTextWrap();
        }

        private void RecalculateTextWrap()
        {
            // Resize text
            string finalText = "";
            int noOfLines = 0;
            int paragraphLines = _text.Count(s => s == '\n');
            int textWidth = Math.Max(150, (int)GetOuterDimensions().ToRectangle().Width);
            if (_text.Length > 0)
            {
                _textInLines = Utils.WordwrapString(_text, Main.fontMouseText, textWidth, _maxLines, out noOfLines);

                foreach (string line in _textInLines)
                {
                    finalText += line + "\n";
                }
            }
            noOfLines++;

            uiText.SetText(finalText, _textScale, false);
            Height.Set(16f + 
                25f * _textScale * (noOfLines +
                paragraphLines), 
                0f);
        }
    }
}
