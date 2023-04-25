using RoR2;
using System.Collections.Generic;
using UnityEngine;
using R2API;

namespace CryoLegionnaire.Modules
{
    public static class Buffs
    {
        // armor buff gained during roll
        internal static BuffDef armorBuff;
        internal static BuffDef chillDebuff;
        internal static BuffDef chillDebuffTimer;

        internal static void RegisterBuffs()
        {
            armorBuff = AddNewBuff("HenryArmorBuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/HiddenInvincibility").iconSprite, 
                Color.white, 
                false, 
                false);

            chillDebuff = AddNewBuff("CryoChillDebuff",
                LegacyResourcesAPI.Load<BuffDef>("BuffDefs/Overheat").iconSprite,
                Color.white,
                true,
                true);
            chillDebuffTimer = AddNewBuff("CryoChillDebuff",
                new Sprite(),
                Color.white,
                false,
                true);
        }

        // simple helper method
        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
        {
            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
            buffDef.name = buffName;
            buffDef.buffColor = buffColor;
            buffDef.canStack = canStack;
            buffDef.isDebuff = isDebuff;
            buffDef.eliteDef = null;
            buffDef.iconSprite = buffIcon;

            Modules.Content.AddBuffDef(buffDef);

            return buffDef;
        }
    }
}