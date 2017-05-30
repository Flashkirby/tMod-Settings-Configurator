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
    internal class UIModEntry : UITextPanel<string>
    {
        private Color _defaultBackground;
        private Color _defaultBorder;

        public bool Active = false;
        public ModSetting modSetting = null;

        public UIModEntry(ModSetting modSetting) : base (modSetting.mod.DisplayName, 1f, false)
        {
            this.modSetting = modSetting;
            Width.Set(0, 1f);
            _defaultBackground = BackgroundColor;
            _defaultBorder = BorderColor;
            OnMouseOver += boxOnMouseOver;
            OnClick += boxOnClick;
            OnMouseUp += boxOnMouseUpOut;
            OnMouseOut += boxOnMouseUpOut;
        }

        private void boxOnMouseOver(UIMouseEvent evt, UIElement listeningElement)
        {
            Main.PlaySound(Terraria.ID.SoundID.MenuTick);
            if(!Active)
            {
                BackgroundColor = UIColour.lightBackgroundColour;
            }
            BorderColor = UIColour.lightborderColour;
        }

        private void boxOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            BackgroundColor = UIColour.darkBackgroundColour;
            BorderColor = UIColour.lightborderColour;

            Active = !Active;

            if (Active)
            {
                BackgroundColor = UIColour.darkBackgroundColour;
                Main.PlaySound(Terraria.ID.SoundID.MenuOpen);
            }
            else
            {
                BackgroundColor = _defaultBackground;
                Main.PlaySound(Terraria.ID.SoundID.MenuClose);
            }
        }

        private void boxOnMouseUpOut(UIMouseEvent evt, UIElement listeningElement)
        {
            BorderColor = _defaultBorder;
            Update(Main._drawInterfaceGameTime);
        }

        public override void Update(GameTime gameTime)
        {
            if (!IsMouseHovering)
            {
                if (Active)
                {
                    BackgroundColor = UIColour.darkBackgroundColour;
                }
                else
                {
                    BackgroundColor = _defaultBackground;
                }
            }
            else
            {
                BorderColor = UIColour.lightborderColour;
            }
        }

        public override int CompareTo(object obj)
        {
            if (obj.GetType() == typeof(UIModEntry))
            {
                UIModEntry other = (UIModEntry)obj;
                return modSetting.mod.DisplayName.CompareTo(other.modSetting.mod.DisplayName);
            }
            return base.CompareTo(obj);
        }
    }
}
