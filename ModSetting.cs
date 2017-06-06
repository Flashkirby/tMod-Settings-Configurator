using System;
using System.Collections.Generic;
using System.IO;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Terraria;
using Terraria.ModLoader;
using Terraria.IO;

using FKTModSettings.UI;

namespace FKTModSettings
{
    public class ModSetting
    {
        public readonly Mod mod = null;

        protected Dictionary<string, StoredVariable> name2value;
        protected Queue<string> nameOrder; // Used to order the dictionary by FIFO
        protected Queue<UIPanelSortable> associatedElements;

        public ModSetting(Mod mod)
        {
            this.mod = mod;
            name2value = new Dictionary<string, StoredVariable>();
            nameOrder = new Queue<string>();
            associatedElements = new Queue<UIPanelSortable>();
        }

        internal Queue<StoredVariable> GetStoredVariables()
        {
            Queue<StoredVariable> fifoList = new Queue<StoredVariable>();
            //throw new Exception("keyList generated:" + string.Join(",", nameOrder));
            foreach (string key in nameOrder)
            {
                StoredVariable sv;
                if (name2value.TryGetValue(key, out sv))
                {
                    fifoList.Enqueue(sv);
                }
                else
                {
                    throw new Exception("Missing dictionary order list entry: " + key);
                }
            }
            return fifoList;
        }
        internal Queue<UIPanelSortable> GetUIElements()
        {
            return associatedElements;
        }

        #region public void AddT(Key, DisplayName, Value, ...)
        /// <summary>
        /// Add a comment for the configuration, with custom scale
        /// </summary>
        /// <param name="Comment"></param>
        /// <param name="scale"></param>
        public void AddComment(string Comment, float scale)
        {
            string uID = "c" + Comment.Length + ":" + Comment.GetHashCode();
            if (name2value.ContainsKey(uID)) return;

            StoredVariable sv = new StoredVariable(Comment, scale);
            name2value.Add(uID, sv);
            nameOrder.Enqueue(uID);
        }
        /// <summary>
        /// Add a comment for the configuration
        /// </summary>
        /// <param name="Comment"></param>
        public void AddComment(string Comment)
        {
            AddComment(Comment, 0.9f);
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddBool(string Key, string DisplayName, bool Multiplayer)
        {
            AddValue(Key, DisplayName, 0, 1, Multiplayer, typeof(bool));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddInt(string Key, string DisplayName, int Min, int Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(int));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddUInt(string Key, string DisplayName, uint Min, uint Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(uint));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddUInt(string Key, string DisplayName, short Min, short Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(short));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddUInt(string Key, string DisplayName, ushort Min, ushort Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(ushort));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddLong(string Key, string DisplayName, long Min, long Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(long));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddByte(string Key, string DisplayName, byte Min, byte Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(byte));
        }

        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddFloat(string Key, string DisplayName, float Min, float Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(float));
        }
        /// <summary>
        /// Add a value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Min">Set min range for option</param>
        /// <param name="Max">Set max range for option</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddDouble(string Key, string DisplayName, double Min, double Max, bool Multiplayer)
        {
            AddValue(Key, DisplayName, Min, Max, Multiplayer, typeof(double));
        }

        private void AddValue(string Key, string DisplayName, double Min, double Max, bool Multiplayer, Type valueType)
        {
            if (name2value.ContainsKey(Key)) return;

            StoredVariable sv = new StoredVariable(DisplayName, Multiplayer, valueType);
            sv.SetMinMax(Min, Max);
            name2value.Add(Key, sv);
            nameOrder.Enqueue(Key);
        }
        #endregion

        #region public void Get(Key, Value)
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref bool Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref int Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref uint Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref short Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref ushort Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref long Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref byte Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref float Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }
        /// <summary>
        /// Get and update the values between the key and the ref variable
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Value"></param>
        public void Get(string Key, ref double Value)
        {
            if (!name2value.ContainsKey(Key)) return;
            StoredVariable sv;
            name2value.TryGetValue(Key, out sv);
            if (name2value != null)
            {
                GetConfig(Key, ref Value, sv);
                sv.Update(ref Value);
            }
        }

        internal void GetConfig<T>(string Key, ref T Value, StoredVariable sv)
        {
            if (!_enableAutoConfig) return;
            if (_loadOnceCounter == 0) return;
            try
            {
                _config.Get(Key, ref Value);
                sv.Set(Value);

                _loadOnceCounter--;

                // Need to save  new value in case of conflict
                // if (_loadConflict) { _config.Put(Key, Value); }
                // save once values are stored
                // if (_loadOnceCounter == 0) _config.Save();
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Add this mod to the mod settings configurator menu
        /// </summary>
        /// <returns></returns>
        public bool AddToModSettings()
        {
            foreach(ModSetting ms in TModSettings.modSettings)
            {
                // Can't add duplicate
                if (ms.mod == this.mod) return false;
            }
            TModSettings.modSettings.Add(this);
            return true;
        }

        #region Custom Config
        private Preferences _config;
        private string loadedVersion;
        private uint _loadOnceCounter;
        private bool _loadConflict { get { return loadedVersion.CompareTo(mod.Version.ToString()) != 0; } }
        private bool _enableAutoConfig;
        private string _modSaveName = "";

        public void EnableAutoConfig()
        {
            EnableAutoConfig(mod.Name + "_autogen");

        }
        public void EnableAutoConfig(Mod mod)
        {
            _enableAutoConfig = true;
            _modSaveName = mod.Name;
        }
        public void EnableAutoConfig(string saveFileName)
        {
            _enableAutoConfig = true;
            _modSaveName = saveFileName;
        }

        private string generateConfigPath()
        {
            return Path.Combine(Main.SavePath, "Mod Configs", _modSaveName + ".json");
        }
        internal void LoadConfigFile()
        {
            if (!_enableAutoConfig) return;

            // https://forums.terraria.org/index.php?threads/modders-guide-to-config-files-and-optional-features.48581/
            _config = new Preferences(generateConfigPath());

            loadedVersion = "";
            if (_config.Load())
            {
                _config.Get("version", ref loadedVersion);
            }

            // Set up load counter for checking to assign each variable once
            _loadOnceCounter = 0;
            foreach (StoredVariable sv in GetStoredVariables())
            {
                if (sv.IsComment) continue;
                _loadOnceCounter++;
            }

            // Doens't exist, or wrong version
            if (_loadConflict)
            {
                // If the correct config doesn't exist, if there are variables to be saved
                // then save them ya!
                if (_loadOnceCounter > 0)
                {
                    _config.Put("version", mod.Version.ToString());
                    _config.Save();
                }
            }
        }
        internal void SaveConfigFile()
        {
            if (!_enableAutoConfig) return;
            foreach (KeyValuePair<string, StoredVariable> kvp in name2value)
            {
                if (kvp.Value.IsComment) continue;
                if (kvp.Value.IsBoolean)
                {
                    _config.Put(kvp.Key, kvp.Value.storedBool);
                }
                else
                {
                    _config.Put(kvp.Key, kvp.Value.storedValue);
                }
            }
            _config.Save();
        }
        #endregion
    }
}
