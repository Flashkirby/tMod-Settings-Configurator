using System;
using System.Collections.Generic;

using Terraria;

namespace FKTModSettings
{
    public class StoredVariable
    {
        internal bool storedBool;
        internal int storedInt;
        internal float storedFloat;
        internal double storedDouble;

        internal float valMin = 0;
        internal float valMax = 100f;
        private bool HasChanged;
        internal byte ValueType;

        public readonly string DisplayName;

        internal bool Multiplayer;
        internal bool InMultiplayer
        {
            get { return Multiplayer && Main.netMode != 0; }
        }

        #region Constructors

        /// <summary>
        /// Store mod settings for comments
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        public StoredVariable(string comment)
        {
            DisplayName = comment;
            ValueType = byte.MaxValue; // No value
        }
        /// <summary>
        /// Store mod settings for bool
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        /// <param name="multiplayer">Use multiplayer behaviour eg. hide from clients</param>
        /// <param name="Value">Initial value to set to</param>
        public StoredVariable(string displayName, bool multiplayer, bool Value)
        {
            DisplayName = displayName;

            storedBool = Value;
            Multiplayer = multiplayer;
            ValueType = 0; // Boolean
        }
        /// <summary>
        /// Store mod settings for int
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        /// <param name="multiplayer">Use multiplayer behaviour eg. hide from clients</param>
        /// <param name="Value">Initial value to set to</param>
        public StoredVariable(string displayName, bool multiplayer, int Value)
        {
            DisplayName = displayName;

            storedInt = Value;
            Multiplayer = multiplayer;
            ValueType = 1; // Int32
        }
        /// <summary>
        /// Store mod settings for float
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        /// <param name="multiplayer">Use multiplayer behaviour eg. hide from clients</param>
        /// <param name="Value">Initial value to set to</param>
        public StoredVariable(string displayName, bool multiplayer, float Value)
        {
            DisplayName = displayName;

            storedFloat = Value;
            Multiplayer = multiplayer;
            ValueType = 2; // Single
        }
        /// <summary>
        /// Store mod settings for double
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        /// <param name="multiplayer">Use multiplayer behaviour eg. hide from clients</param>
        /// <param name="Value">Initial value to set to</param>
        public StoredVariable(string displayName, bool multiplayer, double Value)
        {
            DisplayName = displayName;

            storedDouble = Value;
            Multiplayer = multiplayer;
            ValueType = 3; // Double
        }

        #endregion

        public bool IsBool { get { return ValueType == 0; } }
        public bool IsInt { get { return ValueType == 1; } }
        public bool IsFloat { get { return ValueType == 2; } }
        public bool IsDouble { get { return ValueType == 3; } }
        public bool IsComment { get { return ValueType == byte.MaxValue; } }

        public void SetMinMax(float Min, float Max)
        {
            if (IsBool) throw new InvalidCastException("Stored value is not the specified type. ");

            valMin = Min;
            valMax = Max;
        }

        public void UpdateBool(ref bool Value)
        {
            if (IsBool)
            {
                if (HasChanged) { Value = storedBool; HasChanged = false; }
                else { storedBool = Value; }
            }
            else throw new InvalidCastException("Stored value is not the specified type. ");
        }
        public void UpdateInt(ref int Value)
        {
            if (IsInt)
            {
                if (HasChanged) { Value = storedInt; HasChanged = false; }
                else { storedInt = Value; }
            }
            else throw new InvalidCastException("Stored value is not the specified type. ");
        }
        public void UpdateFloat(ref float Value)
        {
            if (IsFloat)
            {
                if (HasChanged) { Value = storedFloat; HasChanged = false; }
                else { storedFloat = Value; }
            }
            else throw new InvalidCastException("Stored value is not the specified type. ");
        }
        public void UpdateDouble(ref double Value)
        {
            if (IsDouble)
            {
                if (HasChanged) { Value = storedDouble; HasChanged = false; }
                else { storedDouble = Value; }
            }
            else throw new InvalidCastException("Stored value is not the specified type. ");
        }

        public void SetBool(bool Value)
        {
            if (InMultiplayer) return;
            storedBool = Value;
            HasChanged = true;
        }
        public void SetInt(int Value)
        {
            if (InMultiplayer) return;
            storedInt = Math.Max(Math.Min(Value, (int)valMax), (int)valMin);
            HasChanged = true;
        }
        public void SetFloat(float Value)
        {
            if (InMultiplayer) return;
            storedFloat = Math.Max(Math.Min(Value, valMax), valMin);
            HasChanged = true;
        }
        public void SetDouble(double Value)
        {
            if (InMultiplayer) return;
            storedDouble = Math.Max(Math.Min(Value, (double)valMax), (double)valMin);
            HasChanged = true;
        }
    }
}
