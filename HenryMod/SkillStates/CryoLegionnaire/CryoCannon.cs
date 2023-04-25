using CryoLegionnaire.SkillStates.BaseStates;
using RoR2;
using UnityEngine;
using R2API;
using EntityStates;

namespace CryoLegionnaire.SkillStates
{
    public class Icethrower : BaseState
    {
        [SerializeField]
        public GameObject IcethrowerEffectPrefab;
        [SerializeField]
        public float maxDistance;

        public static GameObject impactEffectPrefab;
        public static GameObject tracerEffectPrefab;
        public static float radius;
        public static float baseEntryDuration = 1f;
        public static float baseIcethrowerDuration = 2f;
        public static float totalDamageCoefficient = 1.2f;
        public static float procCoefficientPerTick;
        public static float tickFrequency;
        public static float force = 20f;
        public static string startAttackSoundString;
        public static string endAttackSoundString;
        public static float ignitePercentChance;
        public static float recoilForce;
        private float tickDamageCoefficient;
        private float IcethrowerStopwatch;
        private float stopwatch;
        private float entryDuration;
        private float IcethrowerDuration;
        private bool hasBegunIcethrower;
        private ChildLocator childLocator;
        private Transform leftIcethrowerTransform;
        private Transform rightIcethrowerTransform;
        private Transform leftMuzzleTransform;
        private Transform rightMuzzleTransform;
        private bool isCrit;
        private const float IcethrowerEffectBaseDistance = 16f;

        public override void OnEnter()
        {
            base.OnEnter();
            this.stopwatch = 0f;
            this.entryDuration = Icethrower.baseEntryDuration / this.attackSpeedStat;
            this.IcethrowerDuration = Icethrower.baseIcethrowerDuration;
            Transform modelTransform = base.GetModelTransform();
            if (base.characterBody)
            {
                base.characterBody.SetAimTimer(this.entryDuration + this.IcethrowerDuration + 1f);
            }
            if (modelTransform)
            {
                this.childLocator = modelTransform.GetComponent<ChildLocator>();
                this.leftMuzzleTransform = this.childLocator.FindChild("MuzzleLeft");
                this.rightMuzzleTransform = this.childLocator.FindChild("MuzzleRight");
            }
            int num = Mathf.CeilToInt(this.IcethrowerDuration * Icethrower.tickFrequency);
            this.tickDamageCoefficient = Icethrower.totalDamageCoefficient / (float)num;
            if (base.isAuthority && base.characterBody)
            {
                this.isCrit = Util.CheckRoll(this.critStat, base.characterBody.master);
            }
            base.PlayAnimation("Gesture, Additive", "PrepIcethrower", "Icethrower.playbackRate", this.entryDuration);
        }

        // Token: 0x06000C04 RID: 3076 RVA: 0x00032208 File Offset: 0x00030408
        public override void OnExit()
        {
            Util.PlaySound(Icethrower.endAttackSoundString, base.gameObject);
            base.PlayCrossfade("Gesture, Additive", "ExitIcethrower", 0.1f);
            if (this.leftIcethrowerTransform)
            {
                EntityState.Destroy(this.leftIcethrowerTransform.gameObject);
            }
            if (this.rightIcethrowerTransform)
            {
                EntityState.Destroy(this.rightIcethrowerTransform.gameObject);
            }
            base.OnExit();
        }

        // Token: 0x06000C05 RID: 3077 RVA: 0x0003227C File Offset: 0x0003047C
        private void FireGauntlet(string muzzleString)
        {
            Ray aimRay = base.GetAimRay();
            if (base.isAuthority)
            {
                new BulletAttack
                {
                    owner = base.gameObject,
                    weapon = base.gameObject,
                    origin = aimRay.origin,
                    aimVector = aimRay.direction,
                    minSpread = 0f,
                    damage = this.tickDamageCoefficient * this.damageStat,
                    force = Icethrower.force,
                    muzzleName = muzzleString,
                    hitEffectPrefab = Icethrower.impactEffectPrefab,
                    isCrit = this.isCrit,
                    radius = Icethrower.radius,
                    falloffModel = BulletAttack.FalloffModel.None,
                    stopperMask = LayerIndex.world.mask,
                    procCoefficient = Icethrower.procCoefficientPerTick,
                    maxDistance = this.maxDistance,
                    smartCollision = true,
                    damageType = (Util.CheckRoll(Icethrower.ignitePercentChance, base.characterBody.master) ? DamageType.IgniteOnHit : DamageType.Generic)
                }.Fire();
                if (base.characterMotor)
                {
                    base.characterMotor.ApplyForce(aimRay.direction * -Icethrower.recoilForce, false, false);
                }
            }
        }

        // Token: 0x06000C06 RID: 3078 RVA: 0x000323B4 File Offset: 0x000305B4
        public override void FixedUpdate()
        {
            base.FixedUpdate();
            this.stopwatch += Time.fixedDeltaTime;
            if (this.stopwatch >= this.entryDuration && !this.hasBegunIcethrower)
            {
                this.hasBegunIcethrower = true;
                Util.PlaySound(Icethrower.startAttackSoundString, base.gameObject);
                base.PlayAnimation("Gesture, Additive", "Icethrower", "Icethrower.playbackRate", this.IcethrowerDuration);
                if (this.childLocator)
                {
                    Transform transform = this.childLocator.FindChild("MuzzleLeft");
                    Transform transform2 = this.childLocator.FindChild("MuzzleRight");
                    if (transform)
                    {
                        this.leftIcethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.IcethrowerEffectPrefab, transform).transform;
                    }
                    if (transform2)
                    {
                        this.rightIcethrowerTransform = UnityEngine.Object.Instantiate<GameObject>(this.IcethrowerEffectPrefab, transform2).transform;
                    }
                    if (this.leftIcethrowerTransform)
                    {
                        this.leftIcethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.IcethrowerDuration;
                    }
                    if (this.rightIcethrowerTransform)
                    {
                        this.rightIcethrowerTransform.GetComponent<ScaleParticleSystemDuration>().newDuration = this.IcethrowerDuration;
                    }
                }
                this.FireGauntlet("MuzzleCenter");
            }
            if (this.hasBegunIcethrower)
            {
                this.IcethrowerStopwatch += Time.deltaTime;
                float num = 1f / Icethrower.tickFrequency / this.attackSpeedStat;
                if (this.IcethrowerStopwatch > num)
                {
                    this.IcethrowerStopwatch -= num;
                    this.FireGauntlet("MuzzleCenter");
                }
                this.UpdateIcethrowerEffect();
            }
            if (this.stopwatch >= this.IcethrowerDuration + this.entryDuration && base.isAuthority)
            {
                this.outer.SetNextStateToMain();
                return;
            }
        }

        // Token: 0x06000C07 RID: 3079 RVA: 0x00032568 File Offset: 0x00030768
        private void UpdateIcethrowerEffect()
        {
            Ray aimRay = base.GetAimRay();
            Vector3 direction = aimRay.direction;
            Vector3 direction2 = aimRay.direction;
            if (this.leftIcethrowerTransform)
            {
                this.leftIcethrowerTransform.forward = direction;
            }
            if (this.rightIcethrowerTransform)
            {
                this.rightIcethrowerTransform.forward = direction2;
            }
        }

        // Token: 0x06000C08 RID: 3080 RVA: 0x0000B4B7 File Offset: 0x000096B7
        public override InterruptPriority GetMinimumInterruptPriority()
        {
            return InterruptPriority.Skill;
        }
    }
}