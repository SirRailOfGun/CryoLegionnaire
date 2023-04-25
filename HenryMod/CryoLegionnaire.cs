using BepInEx;
using CryoLegionnaire.Modules.Survivors;
using CryoLegionnaire.SkillStates;
using R2API;
using R2API.Utils;
using RoR2;
using System.Collections.Generic;
using System.Security;
using System.Security.Permissions;
using UnityEngine;
using UnityEngine.Networking;

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

//rename this namespace
namespace CryoLegionnaire
{
    [BepInDependency("com.bepis.r2api", BepInDependency.DependencyFlags.HardDependency)]
    [NetworkCompatibility(CompatibilityLevel.EveryoneMustHaveMod, VersionStrictness.EveryoneNeedSameModVersion)]
    [BepInPlugin(MODUID, MODNAME, MODVERSION)]

    public class CryoLegionnaire : BaseUnityPlugin
    {
        public static DamageAPI.ModdedDamageType ChillDamageType = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType ThreeChillDamageType = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType FiveChillDamageType = DamageAPI.ReserveDamageType();
        public static DamageAPI.ModdedDamageType ExecuteFrozen = DamageAPI.ReserveDamageType();
        // if you don't change these you're giving permission to deprecate the mod-
        //  please change the names to your own stuff, thanks
        //   this shouldn't even have to be said
        public const string MODUID = "com.srog.CryoLegionnaire";
        public const string MODNAME = "CryoLegionnaire";
        public const string MODVERSION = "0.1.0";

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
            RecalculateStatsAPI.GetStatCoefficients += UpdateStats;
            On.RoR2.CharacterBody.RecalculateStats += CharacterBody_RecalculateStats;
        }
        private void UpdateStats(RoR2.CharacterBody self, RecalculateStatsAPI.StatHookEventArgs args)
        {

            if (NetworkServer.active)
            {
                if (self)
                {
                    if (self.HasBuff(Modules.Buffs.chillDebuffTimer))
                    {
                        if (self.isBoss)
                        {
                            args.moveSpeedReductionMultAdd += self.GetBuffCount(Modules.Buffs.chillDebuff) * .025f;
                            args.attackSpeedReductionMultAdd += self.GetBuffCount(Modules.Buffs.chillDebuff) * .025f;
                        }
                        else
                        {
                            args.moveSpeedReductionMultAdd += self.GetBuffCount(Modules.Buffs.chillDebuff) * .05f;
                            args.attackSpeedReductionMultAdd += self.GetBuffCount(Modules.Buffs.chillDebuff) * .05f;
                        }
                    }
                    else
                    {
                        self.SetBuffCount(Modules.Buffs.chillDebuff.buffIndex, 0);
                    }
                }
            }
        }

        private void CharacterBody_RecalculateStats(On.RoR2.CharacterBody.orig_RecalculateStats orig, CharacterBody self)
        {
            orig(self);

            // a simple stat hook, adds armor after stats are recalculated
        }
        private static void CustomDamageHandler(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (info.HasModdedDamageType(ExecuteFrozen))
            {
                Debug.LogWarning(info.HasModdedDamageType(ExecuteFrozen));
                if (self.body.GetBuffCount(Modules.Buffs.chillDebuff) >= 20 || self.isInFrozenState)
                {
                    Debug.LogWarning(self.body.GetBuffCount(Modules.Buffs.chillDebuff) >= 20);
                    Debug.LogWarning(self.isInFrozenState);
                    GameObject attacker = info.attacker;
                    BlastAttack execute = new BlastAttack
                    {
                        attacker = attacker.gameObject,
                        baseDamage = attacker.GetComponent<CharacterBody>().baseDamage * Modules.StaticValues.executeDamageCoefficient,
                        baseForce = 0f,
                        crit = false,
                        damageType = DamageType.Generic,
                        falloffModel = BlastAttack.FalloffModel.None,
                        procCoefficient = 1f,
                        radius = 5f,
                        position = self.transform.position,
                        attackerFiltering = AttackerFiltering.NeverHitSelf,
                        //impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
                        teamIndex = attacker.GetComponent<CharacterBody>().teamComponent.teamIndex
                    };
                    execute.Fire();
                }
            }
            if (info.HasModdedDamageType(ChillDamageType))
            {
                ApplyChill(orig, self, info);
            }
            if (info.HasModdedDamageType(ThreeChillDamageType))
            {
                for (int i = 0; i < 3; i++)
                {
                    ApplyChill(orig, self, info);
                }
            }
            if (info.HasModdedDamageType(FiveChillDamageType))
            {
                for (int i = 0; i < 5; i++)
                {
                    ApplyChill(orig, self, info);
                }
            }
            orig(self, info);
        }
        private static void ApplyChill(On.RoR2.HealthComponent.orig_TakeDamage orig, HealthComponent self, DamageInfo info)
        {
            if (NetworkServer.active)
            {
                self.body.AddTimedBuff(Modules.Buffs.chillDebuffTimer, 5f);
                var onHurt = self.body.GetComponent<SetStateOnHurt>();
                if (self.body.GetBuffCount(Modules.Buffs.chillDebuff) < 50)
                {
                    self.body.AddBuff(Modules.Buffs.chillDebuff);
                    if (self.body.GetBuffCount(Modules.Buffs.chillDebuff) >= 20 && onHurt.canBeFrozen)
                    {
                        self.body.SetBuffCount(Modules.Buffs.chillDebuff.buffIndex, 0);
                        self.body.ClearTimedBuffs(Modules.Buffs.chillDebuffTimer);
                        onHurt.SetFrozen(5);
                    }
                }
            }
            orig(self, info);
        }
    }
}