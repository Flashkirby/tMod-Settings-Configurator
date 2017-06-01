using System;
using System.Collections.Generic;

using Terraria;

namespace FKTModSettings
{
    public class StoredVariable
    {
        internal double storedValue;
        internal bool storedBool
        {
            get { return storedValue == 0d ? false : true; }
            set { storedValue = value ? 1d : 0d; }
        }
        internal Type storedType;
        
        public bool IsBoolean { get { return storedType == typeof(bool); } }
        public bool IsComment { get { return storedType == typeof(string); } }
        public bool IsWholeNumbers
        {
            get {
                return
                  storedType == typeof(int) ||
                  storedType == typeof(uint) ||
                  storedType == typeof(short) ||
                  storedType == typeof(ushort) ||
                  storedType == typeof(long) ||
                  storedType == typeof(byte);
            }
        }
        public bool IsDecimalNumbers
        {
            get
            {
                return
                  storedType == typeof(float) ||
                  storedType == typeof(double);
            }
        }


        internal double valMin = double.MinValue;
        internal double valMax = double.MaxValue;
        private bool HasChanged;

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
            storedType = typeof(string);
        }
        /// <summary>
        /// Store mod settings for bool
        /// </summary>
        /// <param name="displayName">The name shown in the menu</param>
        /// <param name="multiplayer">Use multiplayer behaviour eg. hide from clients</param>
        /// <param name="Value">Initial value to set to</param>
        public StoredVariable(string displayName, bool multiplayer, Type variableType)
        {
            DisplayName = displayName;
            Multiplayer = multiplayer;
            storedType = variableType;

            if (!IsBoolean &&
                !IsComment &&
                !IsWholeNumbers &&
                !IsDecimalNumbers)
            {
                throw new NotSupportedException("Variable type " + variableType.ToString() + " for " + displayName + " is not supported. ");
            }
        }

        #endregion

        public bool SetMinMax(double Min, double Max)
        {
            if (IsBoolean || IsComment) return false;

            if (Min > Max) // RECURSION CAN'T GO WRONG - famous last words
            {
                return SetMinMax(Max, Min);
            }
            else
            {
                valMin = Min;
                valMax = Max;
            }

            return true;
        }

        private void CheckType(Type t)
        {
            if (t != storedType) Main.NewTextMultiline("Wrong type provided for " + DisplayName + ", value given is " + t.ToString() + " but was expecting " + storedType.ToString(), false, Microsoft.Xna.Framework.Color.Red);
        }
        #region Updates
        public void Update(ref bool Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = storedBool; HasChanged = false; }
            else { storedBool = Value; }
        }

        public void Update(ref int Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (int)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref uint Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (uint)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref short Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (short)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref ushort Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (ushort)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref long Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (long)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref byte Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (byte)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }

        public void Update(ref float Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (float)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        public void Update(ref double Value)
        {
            CheckType(Value.GetType());

            if (HasChanged) { Value = (double)storedValue; HasChanged = false; }
            else { storedValue = Value; }
        }
        #endregion

        #region Set
        public void Set(object Value)
        {
            try
            {
                Value = Convert.ChangeType(Value, storedType);
                CheckType(Value.GetType());
                if (InMultiplayer) return;
                HasChanged = true;

                if (storedType == typeof(bool))
                {
                    storedBool = (bool)Value;
                    return;
                }
                else if (storedType == typeof(int))
                {
                    storedValue = Math.Max(Math.Min((int)Value, (int)valMax), (int)valMin);
                    return;
                }
                else if (storedType == typeof(uint))
                {
                    storedValue = Math.Max(Math.Min((uint)Value, (uint)valMax), (uint)valMin);
                    return;
                }
                else if (storedType == typeof(short))
                {
                    storedValue = Math.Max(Math.Min((short)Value, (short)valMax), (short)valMin);
                    return;
                }
                else if (storedType == typeof(ushort))
                {
                    storedValue = Math.Max(Math.Min((ushort)Value, (ushort)valMax), (ushort)valMin);
                    return;
                }
                else if (storedType == typeof(long))
                {
                    storedValue = Math.Max(Math.Min((long)Value, (long)valMax), (long)valMin);
                    return;
                }
                else if (storedType == typeof(byte))
                {
                    storedValue = Math.Max(Math.Min((byte)Value, (byte)valMax), (byte)valMin);
                    return;
                }
                else if (storedType == typeof(float))
                {
                    storedValue = Math.Max(Math.Min((float)Value, (float)valMax), (float)valMin);
                    return;
                }
                else if (storedType == typeof(double))
                {
                    storedValue = Math.Max(Math.Min((double)Value, (double)valMax), (double)valMin);
                    return;
                }
            }
            catch { }
            // Don't reach here unless it can't find anything
            throw new InvalidCastException("Wrong type provided for " + DisplayName + ", value given is " + Value.GetType().ToString() + " but was expecting " + storedType.ToString());
        }
        #endregion
    }
}
