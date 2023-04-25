using EntityStates;
using RoR2;
using UnityEngine;
using R2API;

namespace CryoLegionnaire.SkillStates
{
    public class ChillOut : BaseSkillState
    {
        public static float damageCoefficient = Modules.StaticValues.burstDamageCoefficient;
        public static float procCoefficient = 1f;
        public static float baseDuration = 0.6f;
        public static float force = 800f;
        public static float recoil = 3f;
        public static float range = 256f;
        public static GameObject tracerEffectPrefab = RoR2.LegacyResourcesAPI.Load<GameObject>("Prefabs/Effects/Tracers/TracerGoldGat");

        private float duration;
        private float fireTime;
        private bool hasFired;
        private string muzzleString;

        public override void OnEnter()
        {
            base.OnEnter();
            this.duration = ChillOut.baseDuration / this.attackSpeedStat;
            this.fireTime = 0.2f * this.duration;
            base.characterBody.SetAimTimer(2f);
            this.muzzleString = "Muzzle";

            base.PlayAnimation("LeftArm, Override", "ShootGun", "ShootGun.playbackRate", 1.8f);
        }

        public override void OnExit()
        {
            base.OnExit();
        }

        private void Fire()
        {
            if (!this.hasFired)
            {
                this.hasFired = true;

                base.characterBody.AddSpreadBloom(1.5f);
                EffectManager.SimpleMuzzleFlash(EntityStates.Commando.CommandoWeapon.FirePistol2.muzzleEffectPrefab, base.gameObject, this.muzzleString, false);
                Util.PlaySound("HenryShootPistol", base.gameObject);

                if (base.isAuthority)
                {
                    Ray aimRay = base.GetAimRay();
                    base.AddRecoil(-1f * ChillOut.recoil, -2f * ChillOut.recoil, -0.5f * ChillOut.recoil, 0.5f * ChillOut.recoil);

                    BlastAttack cryoBlast = new BlastAttack();
                    cryoBlast.baseDamage = damageCoefficient * this.damageStat;
                    cryoBlast.AddModdedDamageType(CryoLegionnaire.ThreeChillDamageType);
                    cryoBlast.procCoefficient = 1f;
                    cryoBlast.radius = 17f;
                    cryoBlast.baseForce = 50f;
                    cryoBlast.position = base.transform.position;
                    cryoBlast.attacker = base.gameObject;
                    cryoBlast.teamIndex = TeamComponent.GetObjectTeam(cryoBlast.attacker);
                    cryoBlast.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    cryoBlast.Fire();

                    BlastAttack cryoBlastOuter = new BlastAttack();
                    cryoBlastOuter.baseDamage = 0f;
                    cryoBlastOuter.procCoefficient = 0f;
                    cryoBlastOuter.AddModdedDamageType(CryoLegionnaire.FiveChillDamageType);
                    cryoBlastOuter.radius = 34f;
                    cryoBlastOuter.position = base.transform.position;
                    cryoBlastOuter.attacker = base.gameObject;
                    cryoBlastOuter.teamIndex = TeamComponent.GetObjectTeam(cryoBlast.attacker);
                    cryoBlastOuter.attackerFiltering = AttackerFiltering.NeverHitSelf;
                    cryoBlastOuter.Fire();
                }
            }
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (base.fixedAge >= this.fireTime)
            {
                this.Fire();
            }

            if (base.fixedAge >= this.duration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.PrioritySkill;
        }
    }
}