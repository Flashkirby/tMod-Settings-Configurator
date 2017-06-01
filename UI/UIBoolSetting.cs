using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace FKTModSettings.UI
{
    internal class UIBoolSetting : UIPanelSortable
    {
        private Color _defaultBackground;
        private Color _defaultBorder;

        StoredVariable storedVar;
        UITextPanel<string> booleanSetting;
        public UIBoolSetting(StoredVariable variable)
        {
            this.storedVar = variable;
            Width.Set(0, 1f);
            Height.Set(40f, 0f);

            UIText title = new UIText(storedVar.DisplayName, 0.9f, false);
            title.MaxWidth.Set(-10f, 1f);

            booleanSetting = new UITextPanel<string>("FILLER", 0.8f, false);
            booleanSetting.Top.Set(-18, 0.5f);
            booleanSetting.Left.Set(-46, 1f);
            booleanSetting.MinWidth.Set(60f, 0f);
            _defaultBackground = booleanSetting.BackgroundColor;
            _defaultBorder = booleanSetting.BorderColor;
            booleanSetting.OnMouseOver += boxOnMouseOver;
            booleanSetting.OnClick += boxOnClick;
            booleanSetting.OnMouseUp += boxOnMouseUpOut;
            booleanSetting.OnMouseOut += boxOnMouseUpOut;

            base.Append(booleanSetting);
            base.Append(title);
        }

        private void boxOnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(Terraria.ID.SoundID.MenuTick);
            booleanSetting.BackgroundColor = UIColour.lightBackgroundColour;
            booleanSetting.BorderColor = UIColour.lightborderColour;
        }

        private void boxOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            storedVar.Set(!storedVar.storedBool);

            booleanSetting.BackgroundColor = UIColour.darkBackgroundColour;
            booleanSetting.BorderColor = UIColour.lightborderColour;
            Main.PlaySound(Terraria.ID.SoundID.Unlock);
        }

        private void boxOnMouseUpOut(UIMouseEvent evt, UIElement listeningElement)
        {
            booleanSetting.BorderColor = _defaultBorder;
            Update(Main._drawInterfaceGameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (storedVar.storedBool)
            {
                booleanSetting.SetText("True");
                booleanSetting.BackgroundColor = UIColour.darkBackgroundColour;
            }
            else
            {
                booleanSetting.SetText("False");
                booleanSetting.BackgroundColor = _defaultBackground;
            }
            if (booleanSetting.IsMouseHovering)
            {
                booleanSetting.BorderColor = UIColour.lightborderColour;
            }
            else
            {
                booleanSetting.BorderColor = _defaultBorder;
            }

            if (storedVar.InMultiplayer)
            { BackgroundColor = UIColour.darkBackgroundColour; }
            else
            { BackgroundColor = _defaultBackground; }
        }
    }
}
