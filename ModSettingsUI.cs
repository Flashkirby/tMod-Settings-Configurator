using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

using FKTModSettings.UI;

namespace FKTModSettings
{
    public class ModSettingsUI : UIState
    {
        public bool Visible = false;

        private UITextPanel<string> _header;

        private UIElement _baseElement;
        private UIPanel _basePanel; //background panel

        private UIPanel _modListPanel; // Left panel, lists mods.
        private UIList _modListContainer; // Container for the left panel
        private UIScrollbar _modListScroll; // Scroll bar for the left panel

        private List<UIModEntry> _modListElements; // Actual mod elements as a list

        private UIPanel _settingsPanel; // Left panel, lists mods.
        private UIList _settingsContainer; // Container for the left panel
        private UIScrollbar _settingsScroll; // Scroll bar for the left panel

        public override void OnInitialize()
        {
            // Start defining the element
            _baseElement = new UIElement();
            _baseElement.Width.Set(0f, 0.8f);
            _baseElement.MaxWidth.Set(700f, 0f);
            _baseElement.Height.Set(0f, 0.8f);
            _baseElement.HAlign = 0.5f;
            _baseElement.VAlign = 0.5f;

            _basePanel = new UIPanel();
            _basePanel.SetPadding(0);
            _basePanel.Width.Set(0f, 1f);
            _basePanel.Height.Set(0f, 1f);
            _basePanel.BackgroundColor = UIColour.darkBackgroundColour;
            _basePanel.BorderColor = UIColour.borderColour;
            _baseElement.Append(_basePanel);

            _header = new UITextPanel<string>("Mod Settings Configurator", 1f, false);
            _header.HAlign = 0.5f;
            _header.Top.Set(-30, 0f);
            _basePanel.Append(_header);

            // Mod List
            _modListPanel = new UIPanel();
            _modListPanel.SetPadding(0);
            _modListPanel.Left.Set(4f, 0f);
            _modListPanel.Top.Set(10, 0f);
            _modListPanel.Width.Set(300, 0f);
            _modListPanel.Height.Set(-20, 1f);
            _modListPanel.BackgroundColor = UIColour.backgroundColour;
            _modListPanel.BorderColor = UIColour.borderColour;
            _basePanel.Append(_modListPanel);

            _modListContainer = new UIList();
            _modListContainer.Left.Set(27f, 0f);
            _modListContainer.ListPadding = 6f;
            _modListContainer.VAlign = 0.5f;
            _modListContainer.Width.Set(-30, 1f);
            _modListContainer.Height.Set(-10, 1f);
            _modListPanel.Append(_modListContainer);

            _modListScroll = new UIScrollbar(); //width about 20);
            _modListScroll.SetView(100f, 1000f);
            _modListScroll.Height.Set(-20f, 1f);
            _modListScroll.Left.Set(4f, 0f);
            _modListScroll.VAlign = 0.5f;
            _modListPanel.Append(_modListScroll);
            _modListContainer.SetScrollbar(_modListScroll);

            // Setting List
            _settingsPanel = new UIPanel();
            _settingsPanel.SetPadding(0);
            _settingsPanel.Left.Set(304f, 0f);
            _settingsPanel.Top.Set(10, 0f);
            _settingsPanel.Width.Set(-308, 1f);
            _settingsPanel.Height.Set(-20, 1f);
            _settingsPanel.BackgroundColor = UIColour.backgroundColour;
            _settingsPanel.BorderColor = UIColour.borderColour;

            _settingsContainer = new UIList();
            _settingsContainer.Left.Set(27f, 0f);
            _settingsContainer.ListPadding = 6f;
            _settingsContainer.VAlign = 0.5f;
            _settingsContainer.Width.Set(-30, 1f);
            _settingsContainer.Height.Set(-10, 1f);
            _settingsPanel.Append(_settingsContainer);

            _settingsScroll = new UIScrollbar(); //width about 20);
            _settingsScroll.SetView(100f, 1000f);
            _settingsScroll.Height.Set(-20f, 1f);
            _settingsScroll.Left.Set(4f, 0f);
            _settingsScroll.VAlign = 0.5f;
            _settingsPanel.Append(_settingsScroll);
            _settingsContainer.SetScrollbar(_settingsScroll);

            base.Append(_baseElement);
            // End of defining the element

            _modListElements = new List<UIModEntry>();
        }
        public void PostModLoad()
        {
            _modListContainer.Clear();
            // Create a UIModEntry for each mod settings config added to this mod.
            foreach (ModSetting modSetting in TModSettings.modSettings)
            {
                UIModEntry entry = new UIModEntry(modSetting);
                entry.OnClick += ModListOnClick;
                _modListContainer.Add(entry);
                _modListElements.Add(entry);

                // Add UI elements for each stored variable in reverse to match order added in code
                Queue<UIPanelSortable> uiElements = modSetting.GetUIElements();
                Queue<StoredVariable> storedvars = modSetting.GetStoredVariables();
                if (storedvars.Count > 0)
                {
                    int count = 0;
                    foreach (StoredVariable sv in storedvars)
                    {
                        if (sv.IsBoolean)
                        {
                            UIBoolSetting element = new UIBoolSetting(sv);
                            element.index = count;
                            uiElements.Enqueue(element);
                        }
                        else if (sv.IsWholeNumbers || sv.IsDecimalNumbers)
                        {
                            UINumberSetting element = new UINumberSetting(sv);
                            element.index = count;
                            uiElements.Enqueue(element);
                        }
                        else if (sv.IsComment)
                        {
                            UITextWrapPanel element = new UITextWrapPanel();
                            element.index = count;
                            element.SetText(sv.DisplayName);
                            uiElements.Enqueue(element);
                        }
                        count++;
                    }
                }
            }
            _modListContainer.UpdateOrder();
        }

        private void ModListOnClick(UIMouseEvent evt, UIElement listeningElement)
        {
            // Make all mod entry buttons that aren't the one activated, non-active 
            // and trigger the mouse up method to reset state accordingly.
            _settingsContainer.Clear();
            _basePanel.RemoveChild(_settingsPanel);

            bool anyActive = false;
            foreach (UIModEntry me in _modListElements)
            {
                if (!me.Equals(listeningElement))
                {
                    // Disable other active mod panels
                    me.Active = false;
                    me.MouseUp(evt);
                }
                else if (me.Active)
                {
                    // Clear and re-add the list based from open mod, sorted by index
                    _settingsContainer.AddRange(me.modSetting.GetUIElements());
                    _settingsContainer.UpdateOrder();
                }

                if (!anyActive) anyActive = me.Active;
            }

            // Restore settings panel if something's open
            if (anyActive)
            {
                _basePanel.Append(_settingsPanel);
            }
        }
        
        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            // Need to set this interface as the 
            // UserInterface.ActiveInstance whilst
            // drawing for some elements to work properly
            // ie. the UIScrollBar
            TModSettings.modUserInterface.Use();

            // Prevent game mouse interactions when hovering over the settings
            Main.LocalPlayer.mouseInterface = _baseElement.IsMouseHovering;
        }

    }

    public class UIColour
    {
        public readonly static Color backgroundColour = new Color(63, 65, 151, 200);
        public readonly static Color darkBackgroundColour = new Color(31, 32, 75, 235);
        public readonly static Color lightBackgroundColour = new Color(73, 94, 171, 255);
        public readonly static Color borderColour = new Color(18, 18, 31, 200);
        public readonly static Color lightborderColour = new Color(89, 116, 213, 255);
    }
}