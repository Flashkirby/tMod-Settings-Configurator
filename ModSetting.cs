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
        /// Add a boolean value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Value">Initial value</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddBool(string Key, string DisplayName, bool Multiplayer)
        {
            if (name2value.ContainsKey(Key)) return;
            nameOrder.Enqueue(Key);

            StoredVariable sv = new StoredVariable(DisplayName, Multiplayer, false);
            name2value.Add(Key, sv);
        }
        /// <summary>
        /// Add an integer value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Value">Initial value</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddInt(string Key, string DisplayName, int Min, int Max, bool Multiplayer)
        {
            if (name2value.ContainsKey(Key)) return;
            nameOrder.Enqueue(Key);

            StoredVariable sv = new StoredVariable(DisplayName, Multiplayer, 0);
            sv.SetMinMax((float)Min, (float)Max);
            name2value.Add(Key, sv);
        }
        /// <summary>
        /// Add a float value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Value">Initial value</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddFloat(string Key, string DisplayName, float Min, float Max, bool Multiplayer)
        {
            if (name2value.ContainsKey(Key)) return;
            nameOrder.Enqueue(Key);

            StoredVariable sv = new StoredVariable(DisplayName, Multiplayer, 0f);
            sv.SetMinMax((float)Min, (float)Max);
            name2value.Add(Key, sv);
        }
        /// <summary>
        /// Add a float value to configuration
        /// </summary>
        /// <param name="Key">Key to reference this value later</param>
        /// <param name="DisplayName">The display name used in the mod menu</param>
        /// <param name="Value">Initial value</param>
        /// <param name="Multiplayer">Should this value be server-side? (Hidden for clients) </param>
        public void AddDouble(string Key, string DisplayName, double Min, double Max, bool Multiplayer)
        {
            if (name2value.ContainsKey(Key)) return;
            nameOrder.Enqueue(Key);

            StoredVariable sv = new StoredVariable(DisplayName, Multiplayer, 0d);
            sv.SetMinMax((float)Min, (float)Max);
            name2value.Add(Key, sv);
        }
        
        public void AddComment(string Comment)
        {
            string uID = "c" + Comment.Length + ":" + Comment.GetHashCode();
            if (name2value.ContainsKey(uID)) return;
            nameOrder.Enqueue(uID);

            StoredVariable sv = new StoredVariable(Comment);
            name2value.Add(uID, sv);
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
            if(name2value != null)
            {
                sv.UpdateBool(ref Value);
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
                sv.UpdateInt(ref Value);
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
                sv.UpdateFloat(ref Value);
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
                sv.UpdateDouble(ref Value);
            }
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
    }
}
