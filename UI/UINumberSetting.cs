using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;
using Microsoft.Xna.Framework.Graphics;

namespace FKTModSettings.UI
{
    internal class UINumberSetting : UIPanelSortable
    {
        private Color _defaultBackground;
        private Color _defaultBorder;

        StoredVariable storedVar;
        UITextPanel<string> numberBox;
        UIValueBar numberBar;

        private bool Focus;
        private ushort focusTick = 0;

        private bool useDecimalSystem = false;
        private double decimalIncrement = 200d;

        public UINumberSetting(StoredVariable variable)
        {
            this.storedVar = variable;
            Width.Set(0, 1f);
            Height.Set(60f, 0f);

            UIText title = new UIText(storedVar.DisplayName, 0.9f, false);
            title.MaxWidth.Set(-10f, 1f);

            numberBox = new UITextPanel<string>("000000", 0.8f, false);
            numberBox.Top.Set(-18, 0.5f);
            numberBox.Left.Set(-110, 1f);
            numberBox.Width.Set(120f, 0f);
            _defaultBackground = numberBox.BackgroundColor;
            _defaultBorder = numberBox.BorderColor;
            numberBox.OnMouseOver += BoxMouseOver;
            numberBox.OnClick += BoxOnClick;

            useDecimalSystem = false;
            int min = (int)(storedVar.valMin - 0.5f);
            int max = (int)storedVar.valMax;
            if(min - max < 200 && storedVar.IsDecimalNumbers)
            {
                // set up to allow normalised increments
                useDecimalSystem = true;
                min = (int)(min * decimalIncrement - 0.5f);
                max = (int)(max * decimalIncrement);
            }
            numberBar = new UIValueBar(min, max);
            numberBar.Top.Set(-20f, 1f);
            numberBar.Left.Set(-6f, 0f);
            numberBar.OnMouseUp += BarMouseUp;

            base.Append(numberBar);
            base.Append(numberBox);
            base.Append(title);
        }

        #region Number Typing

        private void BoxMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(Terraria.ID.SoundID.MenuTick);
            numberBox.BorderColor = UIColour.lightborderColour;
        }

        private void BoxOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            if (Main.drawingPlayerChat) return;
            if (Focus)
            {
                SetValueFromText();
                return;
            }

            Focus = true;
            focusTick = 0;
            Main.blockInput = true;
            
            Main.PlaySound(Terraria.ID.SoundID.MenuOpen);
            numberBox.BackgroundColor = UIColour.darkBackgroundColour;
            numberBox.BorderColor = UIColour.lightborderColour;
        }

        private void UpdateInput()
        {
            Main.hasFocus = true;
            Main.chatRelease = false;
            if (focusTick < 25)
            {
                focusTick++;
            }
            else
            {
                if (numberBox.BorderColor == UIColour.lightborderColour)
                { numberBox.BorderColor = _defaultBorder; }
                else
                { numberBox.BorderColor = UIColour.lightborderColour; }
                focusTick = 0;
            }

            // Check for changes
            string input = Main.GetInputText(numberBox.Text);
            if (Main.inputText == Main.oldInputText) return; // no change

            // Setup input
            if (storedVar.IsWholeNumbers)
            {
                long parsedValue = (long)storedVar.storedValue;
                // Allow changing text, only when valid
                if (long.TryParse(input, out parsedValue) || input == "-")
                {
                    numberBox.SetText(input);
                    Main.PlaySound(Terraria.ID.SoundID.MenuTick);
                }
            }
            else if (storedVar.IsDecimalNumbers)
            {
                float parsedValue = (float)storedVar.storedValue;
                // Allow changing text, only when valid
                if (float.TryParse(input, out parsedValue) || input == "-")
                {
                    numberBox.SetText(input);
                    Main.PlaySound(Terraria.ID.SoundID.MenuTick);
                }
            }

            // Also allow empty
            if (input == "")
            {
                numberBox.SetText("");
                Main.PlaySound(Terraria.ID.SoundID.MenuTick);
            }

            if (Main.inputTextEnter || Main.inputTextEscape)
            {
                SetValueFromText();
            }
        }

        private void SetValueFromText()
        {
            Focus = false;
            Main.hasFocus = false;
            Main.blockInput = false;

            numberBox.BorderColor = _defaultBorder;
            numberBox.BackgroundColor = _defaultBackground;
            Main.PlaySound(Terraria.ID.SoundID.MenuClose);

            if (numberBox.Text.Length == 0 || numberBox.Text == "-") numberBox.SetText("0");
            if (storedVar.IsWholeNumbers)
            {
                long parsedValue = (long)storedVar.storedValue;
                if (long.TryParse(numberBox.Text, out parsedValue))
                {
                    if ((long)storedVar.storedValue != parsedValue) storedVar.Set(parsedValue); // SET
                }
            }
            else if (storedVar.IsDecimalNumbers)
            {
                float parsedValue = (float)storedVar.storedValue;
                if (float.TryParse(numberBox.Text, out parsedValue))
                {
                    if (storedVar.storedValue != parsedValue) storedVar.Set(parsedValue); // SET
                }
            }
        }

        private void BarMouseUp(UIMouseEvent evt, UIElement listeningElement)
        {
            double nValue = numberBar.Value;
            if (useDecimalSystem) nValue /= decimalIncrement;

            if (storedVar.IsWholeNumbers) storedVar.Set((long)nValue); // SET
            if (storedVar.IsDecimalNumbers) storedVar.Set((float)nValue); // SET
        }

        #endregion

        public override void Update(GameTime gameTime)
        {
            if (!numberBar.Dragging)
            {
                double boxValue = storedVar.storedValue;
                if (useDecimalSystem && storedVar.IsDecimalNumbers) boxValue *= decimalIncrement;

                numberBar.Value = (int)Math.Round(boxValue);
            }
            if (Focus)
            {
                if (!TModSettings.modSettingsUI.Visible || numberBar.Dragging)
                { SetValueFromText(); }
                else
                { UpdateInput(); }
            }
            else
            {
                numberBox.SetText("" + storedVar.storedValue);
                if (!numberBox.IsMouseHovering)
                {
                    numberBox.BorderColor = _defaultBorder;
                }
            }

            if (storedVar.InMultiplayer)
            { BackgroundColor = UIColour.darkBackgroundColour; }
            else
            { BackgroundColor = _defaultBackground; }

            base.Update(gameTime);
        }
    }
}
