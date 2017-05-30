using System;

using Terraria.ModLoader;

namespace FKTModSettings
{
    /// <summary>
    /// Handy dandy class that has all the cool kid functions you need to know
    /// </summary>
    public static class ModSettingsAPI
    {
        /// <summary>
        /// Call this during your mod's Load method
        /// </summary>
        /// <param name="mod"></param>
        public static ModSetting CreateModSettingConfig(Mod mod)
        {
            ModSetting settings = new ModSetting(mod);
            settings.AddToModSettings();
            return settings;
        }

        /// <summary>
        /// Call with your mod to get the ModSettings
        /// </summary>
        /// <param name="mod"></param>
        /// <returns></returns>
        public static ModSetting GetModSettingOrNull(Mod mod)
        {
            return TModSettings.GetModSetting(mod);
        }

        /// <summary>
        /// Call with your mod to get the ModSettings
        /// </summary>
        /// <param name="mod"></param>
        /// <param name="modSetting"></param>
        /// <returns></returns>
        public static bool TryGetModSetting(Mod mod, out ModSetting modSetting)
        {
            modSetting = TModSettings.GetModSetting(mod);
            if(modSetting != null)
            {
                return true;
            }
            return false;
        }
    }
}
