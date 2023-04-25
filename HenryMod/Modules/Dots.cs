//using R2API;
//using RoR2;
//using System.Collections.Generic;
//using UnityEngine;

//namespace CryoLegionnaire.Modules
//{
//    public static class Dots
//    {
//        // armor buff gained during roll
//        public static DotController chillDot;

//        internal static void RegisterDots()
//        {
//        }

//        // simple helper method
//        internal static BuffDef AddNewBuff(string buffName, Sprite buffIcon, Color buffColor, bool canStack, bool isDebuff)
//        {
//            BuffDef buffDef = ScriptableObject.CreateInstance<BuffDef>();
//            buffDef.name = buffName;
//            buffDef.buffColor = buffColor;
//            buffDef.canStack = canStack;
//            buffDef.isDebuff = isDebuff;
//            buffDef.eliteDef = null;
//            buffDef.iconSprite = buffIcon;

//            Modules.Content.AddBuffDef(buffDef);

//            return buffDef;
//        }
//    }
//}