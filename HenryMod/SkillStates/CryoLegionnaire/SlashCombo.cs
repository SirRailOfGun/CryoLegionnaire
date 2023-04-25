using CryoLegionnaire.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using R2API;

namespace CryoLegionnaire.SkillStates
{
    public class SlashCombo : BaseMeleeAttack
    {
        public override void OnEnter()
        {
            this.hitboxName = "Sword";

            this.damageType = DamageType.Generic;
            this.damageCoefficient = Modules.StaticValues.swordDamageCoefficient;
            this.procCoefficient = 1f;
            this.pushForce = 300f;
            this.bonusForce = Vector3.zero;
            this.baseDuration = 1f;
            this.attackStartTime = 0.2f;
            this.attackEndTime = 0.4f;
            this.baseEarlyExitTime = 0.4f;
            this.hitStopDuration = 0.012f;
            this.attackRecoil = 0.5f;
            this.hitHopVelocity = 4f;

            this.swingSoundString = "HenrySwordSwing";
            this.hitSoundString = "";
            this.muzzleString = swingIndex % 2 == 0 ? "SwingLeft" : "SwingRight";
            this.swingEffectPrefab = Modules.Assets.swordSwingEffect;
            this.hitEffectPrefab = Modules.Assets.swordHitImpactEffect;

            this.impactSound = Modules.Assets.swordHitSoundEvent.index;

            base.OnEnter();
        }
        //private void FireGauntlet(string muzzleString)
        //{
        //    Ray aimRay = base.GetAimRay();
        //    if (base.isAuthority)
        //    {
        //        new BulletAttack
        //        {
        //            owner = base.gameObject,
        //            weapon = base.gameObject,
        //            origin = aimRay.origin,
        //            aimVector = aimRay.direction,
        //            minSpread = 0f,
        //            damage = this.tickDamageCoefficient * this.damageStat,
        //            force = Flamethrower.force,
        //            muzzleName = muzzleString,
        //            hitEffectPrefab = Flamethrower.impactEffectPrefab,
        //            isCrit = this.isCrit,
        //            radius = Flamethrower.radius,
        //            falloffModel = BulletAttack.FalloffModel.None,
        //            stopperMask = LayerIndex.world.mask,
        //            procCoefficient = Flamethrower.procCoefficientPerTick,
        //            maxDistance = this.maxDistance,
        //            smartCollision = true,
        //            damageType = (Util.CheckRoll(Flamethrower.ignitePercentChance, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic)
        //        }.Fire();
        //        if (base.characterMotor)
        //        {
        //            base.characterMotor.ApplyForce(aimRay.direction * -Flamethrower.recoilForce, false, false);
        //        }
        //    }
        //}
        protected override void PlayAttackAnimation()
        {
            base.PlayAttackAnimation();
        }

        protected override void PlaySwingEffect()
        {
            base.PlaySwingEffect();
        }

        protected override void OnHitEnemyAuthority()
        {
            base.OnHitEnemyAuthority();
        }

        protected override void SetNextState()
        {
            int index = this.swingIndex;
            if (index == 0) index = 1;
            else index = 0;

            this.outer.SetNextState(new SlashCombo
            {
                swingIndex = index
            });
        }

        public override void OnExit()
        {
            base.OnExit();
        }
    }
}