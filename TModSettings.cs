using System;
using System.Collections.Generic;

using Microsoft.Xna.Framework;

using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.UI;

namespace FKTModSettings
{
    public class TModSettings : Mod
    {
        internal static List<Mod> Mods;

        // UIs
        internal static UserInterface modUserInterface;
        internal static ModSettingsUI modSettingsUI;

        // Supported Mods
        internal static List<ModSetting> modSettings;

        // Hotkey
        ModHotKey ControlModSettings = null;

        public override void Load()
        {
            Mods = new List<Mod>();
            if (Main.dedServ) return;

            modSettingsUI = new ModSettingsUI();
            modUserInterface = new UserInterface();
            modUserInterface.SetState(modSettingsUI);

            modSettings = new List<ModSetting>();

            ControlModSettings = RegisterHotKey("Open Mod Settings", "Oem8"); // The ` key

            modSettingsUI.Activate();
            
            ModSetting setting = ModSettingsAPI.CreateModSettingConfig(this);
            setting.EnableAutoConfig(this);
            setting.AddComment("This is a barebones framework for modifying supported in-game settings! ");
            setting.AddBool("isday", "Day Time", true);
            setting.AddDouble("dayticks", "Current Time", 0, Main.dayLength, true);
            setting.AddFloat("modifyWidth", "Modify Config Width", 700, 2000, false);
            setting.AddComment("Version " + this.Version);
        }
        public override void Unload()
        {
            modSettings = null;
        }
        public override void PreSaveAndQuit()
        {
            foreach (ModSetting ms in modSettings)
            {
                ms.SaveConfigFile();
            }
        }

        public override void PostSetupContent()
        {
            if (Main.dedServ) return;

            foreach(ModSetting ms in modSettings)
            {
                ms.LoadConfigFile();
            }
            modSettingsUI.PostModLoad();

            // Add menu button for hero/cheat
            Mod herosMod = ModLoader.GetMod("HEROsMod");
            Mod cheatSheet = ModLoader.GetMod("CheatSheet");
            try
            {
                HeroCheatIntegration(herosMod, cheatSheet);
            }
            catch { }
        }

        #region Hero/Cheat methods
        private void HeroCheatIntegration(Mod herosMod, Mod cheatSheet)
        {
            if (herosMod != null)
            {
                herosMod.Call(
                    "AddPermission",
                    "AccessModSettingsConfig",
                    "Access Mod Settings Configurator"
                );

                herosMod.Call(
                    "AddSimpleButton",
                    "AccessModSettingsConfig",
                    GetTexture("ModMenuButton"),
                    (Action)SettingsMenuPressed,
                    (Action<bool>)PermissionChanged,
                    (Func<string>)ModTooltip
                );
            }
            else if (cheatSheet != null)
            {
                CheatSheet.CheatSheetInterface.RegisterButton(
                    cheatSheet,
                    GetTexture("ModMenuButton"),
                    SettingsMenuPressed,
                    ModTooltip);
            }
        }
        public string ModTooltip()
        {
            return modSettingsUI.Visible ? "Close Mod Settings Configurator" : "Open Mod Settings Configurator";
        }

        public void PermissionChanged(bool hasPermission)
        {
            if (!hasPermission)
            {
                if (modSettingsUI.Visible) CloseModSettingsMenu();
            }
        }
        #endregion

        public override void ModifyInterfaceLayers(List<MethodSequenceListItem> layers)
        {
            //All this stuff is jankyily adapted from ExampleMod
            //This is getting the mouse layer, and adding the UI just underneath it
            int MouseTextIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Cursor"));
            if (MouseTextIndex != -1)
            {
                layers.Insert(MouseTextIndex, new MethodSequenceListItem(
                    "TModSettingsPanel",
                    delegate
                    {
                        if (modSettingsUI.Visible)
                        {
                            modUserInterface.Update(Main._drawInterfaceGameTime);
                            modSettingsUI.Draw(Main.spriteBatch);
                        }
                        return true;
                    },
                    null)
                );
            }
        }

        public override void PostUpdateInput()
        {
            ModSetting setting;
            if (ModSettingsAPI.TryGetModSetting(this, out setting))
            {
                setting.Get("isday", ref Main.dayTime);
                setting.Get("dayticks", ref Main.time);
                setting.Get("modifyWidth", ref modSettingsUI.configWidth);
            }

            if (Main.dedServ) return;

            if (Main.gameMenu || ControlModSettings == null || modSettingsUI == null) return;

            if (Main.playerInventory && modSettingsUI.Visible) CloseModSettingsMenu();

            if(ControlModSettings.JustPressed)
            {
                SettingsMenuPressed();
            }
        }

        private void SettingsMenuPressed()
        {
            if (!modSettingsUI.Visible)
            {
                OpenModSettingsMenu();
            }
            else
            {
                CloseModSettingsMenu();
            }
        }
        private void OpenModSettingsMenu()
        {
            if (Main.dedServ) return;

            Main.playerInventory = false;
            Main.editChest = false;
            Main.npcChatText = "";
            Main.PlaySound(Terraria.ID.SoundID.MenuOpen);

            modSettingsUI.Visible = true;
            modSettingsUI.Recalculate();
        }
        private void CloseModSettingsMenu()
        {
            if (Main.dedServ) return;

            Main.PlaySound(Terraria.ID.SoundID.MenuClose);

            Main.blockInput = false;
            modSettingsUI.Visible = false;
        }

        public static ModSetting GetModSetting(Mod mod)
        {
            if (Main.gameMenu || modSettings == null) return null;
            if (!Main.dedServ && modSettingsUI == null) return null;
            foreach (ModSetting ms in modSettings)
            {
                if (ms.mod == mod)
                {
                    return ms;
                }
            }
            return null;
        }

        public static void AddMod(Mod mod)
        {
            if (!Mods.Contains(mod)) Mods.Add(mod);
        }
    }
}