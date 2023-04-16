using BepInEx;
using CryoLegionnaire.Modules.Survivors;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace CryoLegionnaire
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]
    [R2APISubmoduleDependency(new string[]
    {
        "PrefabAPI",
        "LanguageAPI",
        "SoundAPI",
        "UnlockableAPI"
    })]

    public class CryoLegionnaire : BaseUnityPlugin
    {
        public static DamageAPI.ModdedDamageType ChillDamageType = DamageAPI.ReserveDamageType();
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.srog.CryoLegionnaire";
        public const string MODNAME = "CryoLegionnaire";
        public const string MODVERSION = "0.0.1";

        // a prefix for name tokens to prevent conflicts- please capitalize all name tokens for convention
        public const string DEVELOPER_PREFIX = "SROG";

        public static CryoLegionnaire instance;

        public static PluginInfo PInfo { get; private set; }
        private void Awake()
        {
            instance = this;
            PInfo = Info;

            Log.Init(Logger);
            Modules.Assets.Initialize(); // load assets and read config
            Modules.Config.ReadConfig();
            Modules.States.RegisterStates(); // register states for networking
            Modules.Buffs.RegisterBuffs(); // add and register custom buffs/debuffs
            Modules.Projectiles.RegisterProjectiles(); // add and register custom projectiles
            Modules.Tokens.AddTokens(); // register name tokens
            Modules.ItemDisplays.PopulateDisplays(); // collect item display prefabs for use in our display rules

            // survivor initialization
            new MyCharacter().Initialize();

            // now make a content pack and add it- this part will change with the next update
            new Modules.ContentPacks().Initialize();

            Hook();
        }

        private void Hook()
        {
            // run hooks here, disabling one is as simple as commenting out the line
            On.RoR2.HealthComponent.TakeDamage += CustomDamageHandler;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
            if (self)
            {
                if (self.HasBuff(Modules.Buffs.chillDebuff))
                {
                    if (self.isBoss)
                    {
                        self.moveSpeed = self.baseMoveSpeed * (self.GetBuffCount(Modules.Buffs.chillDebuff) / 40);
                        self.attackSpeed = self.baseAttackSpeed * (self.GetBuffCount(Modules.Buffs.chillDebuff) / 40);
                    }
                    else
                    {
                        self.moveSpeed = self.baseMoveSpeed * (self.GetBuffCount(Modules.Buffs.chillDebuff) / 20);
                        self.attackSpeed = self.baseAttackSpeed * (self.GetBuffCount(Modules.Buffs.chillDebuff) / 20);
                    }
                    if (self.moveSpeed <= 0)
                    {
                        self.moveSpeed = 0;
                    }
                    if (self.attackSpeed <= 0)
                    {
                        self.attackSpeed = 0;
                    }
                }
            }
        }
        private static void CustomDamageHandler(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (info.HasModdedDamageType(ChillDamageType))
            {
                var onHurt = self.body.GetComponent<SetStateOnHurt>();
                if (onHurt)
                {
                    if (self.body.GetBuffCount(Modules.Buffs.chillDebuff) < 50)
                    {
                        self.body.AddTimedBuff(Modules.Buffs.chillDebuff, 3f);
                        if (self.body.GetBuffCount(Modules.Buffs.chillDebuff) >= 20 && onHurt.canBeFrozen)
                        {
                            self.body.ClearTimedBuffs(Modules.Buffs.chillDebuff);
                            onHurt.SetFrozen(10);
                        }
                    }
                    else
                    {
                    }
                }
            }
            //if (info.HasModdedDamageType(StunCrownDamageType))
            //{
            //    var onHurt = self.body.GetComponent<SetStateOnHurt>();
            //    if (onHurt)
            //    {
            //        onHurt.SetStun(StaticValues.StunCrownStun);
            //    }
            //}
            //if (self.body.HasBuff(Modules.Buffs.lockOnBuff))
            //{
            //    info.crit = true;
            //    self.body.ClearTimedBuffs(Modules.Buffs.lockOnBuff);
            //}
            //if (info.HasModdedDamageType(LockOnDamageType))
            //{
            //    if (self.body)
            //    {
            //        self.body.AddTimedBuff(Modules.Buffs.lockOnBuff, 60f);
            //    }
            //}
            orig(self, info);
        }
    }
}