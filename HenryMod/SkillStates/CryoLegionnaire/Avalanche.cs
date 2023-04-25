using EntityStates;
using RoR2;
using RoR2.Projectile;
using UnityEngine;
using UnityEngine.Networking;

namespace CryoLegionnaire.SkillStates
{
    public class Avalanche : BaseSkillState
    {
        public static float duration = 0.5f;
        public static float initialSpeedCoefficient = 5f;
        public static float finalSpeedCoefficient = 2.5f;
        public static float airControl;
        private float previousAirControl;

        public static float minimumDuration;

        public static string dodgeSoundString = "HenryRoll";
        public static float dodgeFOV = EntityStates.Commando.DodgeState.dodgeFOV;

        private float AvalancheSpeed;
        private Vector3 forwardDirection;
        private Animator animator;
        private Vector3 previousPosition;

        public override void OnEnter()
        {
            base.OnEnter();
            //this.animator = base.GetModelAnimator();

            //if (base.isAuthority && base.inputBank && base.characterDirection)
            //{
            //    this.forwardDirection = ((base.inputBank.moveVector == Vector3.zero) ? base.characterDirection.forward : base.inputBank.moveVector).normalized;
            //}

            //Vector3 rhs = base.characterDirection ? base.characterDirection.forward : this.forwardDirection;
            //Vector3 rhs2 = Vector3.Cross(Vector3.up, rhs);

            //float num = Vector3.Dot(this.forwardDirection, rhs);
            //float num2 = Vector3.Dot(this.forwardDirection, rhs2);

            //this.RecalculateAvalancheSpeed();

            //if (base.characterMotor && base.characterDirection)
            //{
            //    base.characterMotor.velocity.y = 0f;
            //    base.characterMotor.velocity = this.forwardDirection * this.AvalancheSpeed;
            //}

            //Vector3 b = base.characterMotor ? base.characterMotor.velocity : Vector3.zero;
            //this.previousPosition = base.transform.position - b;

            //base.PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", Avalanche.duration);
            //Util.PlaySound(Avalanche.dodgeSoundString, base.gameObject);

            //if (NetworkServer.active)
            //{
            //    base.characterBody.AddTimedBuff(Modules.Buffs.armorBuff, 3f * Avalanche.duration);
            //    base.characterBody.AddTimedBuff(RoR2Content.Buffs.HiddenInvincibility, 0.5f * Avalanche.duration);
            //}

            //Acrid code
            this.crocoDamageTypeController = base.GetComponent<CrocoDamageTypeController>();
            this.previousAirControl = base.characterMotor.airControl;
            base.characterMotor.airControl = Avalanche.airControl;
            Vector3 direction = base.GetAimRay().direction;
            if (base.isAuthority)
            {
                base.characterBody.isSprinting = true;
                direction.y = Mathf.Max(direction.y, Avalanche.minimumY);
                Vector3 a = direction.normalized * Avalanche.aimVelocity * this.moveSpeedStat;
                Vector3 b = Vector3.up * Avalanche.upwardVelocity;
                Vector3 b2 = new Vector3(direction.x, 0f, direction.z).normalized * Avalanche.forwardVelocity;
                base.characterMotor.Motor.ForceUnground();
                base.characterMotor.velocity = a + b + b2;
                this.isCritAuthority = base.RollCrit();
            }
            base.characterBody.bodyFlags |= CharacterBody.BodyFlags.IgnoreFallDamage;
            base.GetModelTransform().GetComponent<AimAnimator>().enabled = true;
            base.PlayAnimation("FullBody, Override", "Roll", "Roll.playbackRate", Avalanche.duration);
            //base.PlayCrossfade("Gesture, Override", "Leap", 0.1f);
            //base.PlayCrossfade("Gesture, AdditiveHigh", "Leap", 0.1f);
            //base.PlayCrossfade("Gesture, Override", "Leap", 0.1f);
            //Util.PlaySound(BaseLeap.leapSoundString, base.gameObject);
            base.characterDirection.moveVector = direction;
            //this.leftFistEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.fistEffectPrefab, base.FindModelChild("MuzzleHandL"));
            //this.rightFistEffectInstance = UnityEngine.Object.Instantiate<GameObject>(this.fistEffectPrefab, base.FindModelChild("MuzzleHandR"));
            if (base.isAuthority)
            {
                base.characterMotor.onMovementHit += this.OnMovementHit;
            }
            Util.PlaySound(Avalanche.soundLoopStartEvent, base.gameObject);
        }
        private void OnMovementHit(ref CharacterMotor.MovementHitInfo movementHitInfo)
        {
            this.detonateNextFrame = true;
        }
        private void RecalculateAvalancheSpeed()
        {
            this.AvalancheSpeed = this.moveSpeedStat * Mathf.Lerp(Avalanche.initialSpeedCoefficient, Avalanche.finalSpeedCoefficient, base.fixedAge / Avalanche.duration);
        }

        public override void FixedUpdate()
        {
            //base.FixedUpdate();
            //this.RecalculateAvalancheSpeed();

            //if (base.characterDirection) base.characterDirection.forward = this.forwardDirection;
            //if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = Mathf.Lerp(Avalanche.dodgeFOV, 60f, base.fixedAge / Avalanche.duration);

            //Vector3 normalized = (base.transform.position - this.previousPosition).normalized;
            //if (base.characterMotor && base.characterDirection && normalized != Vector3.zero)
            //{
            //    Vector3 vector = normalized * this.AvalancheSpeed;
            //    float d = Mathf.Max(Vector3.Dot(vector, this.forwardDirection), 0f);
            //    vector = this.forwardDirection * d;
            //    vector.y = 0f;

            //    base.characterMotor.velocity = vector;
            //}
            //this.previousPosition = base.transform.position;

            //if (base.isAuthority && base.fixedAge >= Avalanche.duration)
            //{
            //    this.outer.SetNextStateToMain();
            //    return;
            //}
            base.FixedUpdate();
            if (base.isAuthority && base.characterMotor)
            {
                base.characterMotor.moveDirection = base.inputBank.moveVector;
                if (base.fixedAge >= Avalanche.minimumDuration && (this.detonateNextFrame || (base.characterMotor.Motor.GroundingStatus.IsStableOnGround && !base.characterMotor.Motor.LastGroundingStatus.IsStableOnGround)))
                {
                    this.DoImpactAuthority();
                    this.outer.SetNextStateToMain();
                }
            }
        }
        protected virtual void DoImpactAuthority()
        {
            if (Avalanche.landingSound)
            {
                EffectManager.SimpleSoundEffect(Avalanche.landingSound.index, base.characterBody.footPosition, true);
            }
        }
        protected BlastAttack.Result DetonateAuthority()
        {
            Vector3 footPosition = base.characterBody.footPosition;
            EffectManager.SpawnEffect(this.blastEffectPrefab, new EffectData
            {
                origin = footPosition,
                scale = Avalanche.blastRadius
            }, true);
            return new BlastAttack
            {
                attacker = base.gameObject,
                baseDamage = this.damageStat * this.blastDamageCoefficient,
                baseForce = this.blastForce,
                bonusForce = this.blastBonusForce,
                crit = this.isCritAuthority,
                damageType = DamageType.Generic,
                falloffModel = BlastAttack.FalloffModel.None,
                procCoefficient = Avalanche.blastProcCoefficient,
                radius = Avalanche.blastRadius,
                position = footPosition,
                attackerFiltering = AttackerFiltering.NeverHitSelf,
                impactEffect = EffectCatalog.FindEffectIndexFromPrefab(this.blastImpactEffectPrefab),
                teamIndex = base.teamComponent.teamIndex
            }.Fire();
        }
        protected void DropAcidPoolAuthority()
        {
            Vector3 footPosition = base.characterBody.footPosition;
            FireProjectileInfo fireProjectileInfo = new FireProjectileInfo
            {
                projectilePrefab = Avalanche.projectilePrefab,
                crit = this.isCritAuthority,
                force = 0f,
                damage = this.damageStat,
                owner = base.gameObject,
                rotation = Quaternion.identity,
                position = footPosition
            };
            ProjectileManager.instance.FireProjectile(fireProjectileInfo);
        }




        public override void OnExit()
        {
            if (base.cameraTargetParams) base.cameraTargetParams.fovOverride = -1f;
            base.OnExit();

            base.characterMotor.disableAirControlUntilCollision = false;
        }

        public override void OnSerialize(NetworkWriter writer)
        {
            base.OnSerialize(writer);
            writer.Write(this.forwardDirection);
        }

        public override void OnDeserialize(NetworkReader reader)
        {
            base.OnDeserialize(reader);
            this.forwardDirection = reader.ReadVector3();
        }

        // Token: 0x04001662 RID: 5730

        // Token: 0x04001663 RID: 5731
        public static float blastRadius;

        // Token: 0x04001664 RID: 5732
        public static float blastProcCoefficient;

        // Token: 0x04001665 RID: 5733
        [SerializeField]
        public float blastDamageCoefficient;

        // Token: 0x04001666 RID: 5734
        [SerializeField]
        public float blastForce;

        // Token: 0x04001667 RID: 5735
        public static string leapSoundString;

        // Token: 0x04001668 RID: 5736
        public static GameObject projectilePrefab;

        // Token: 0x04001669 RID: 5737
        [SerializeField]
        public Vector3 blastBonusForce;

        // Token: 0x0400166A RID: 5738
        [SerializeField]
        public GameObject blastImpactEffectPrefab;

        // Token: 0x0400166B RID: 5739
        [SerializeField]
        public GameObject blastEffectPrefab;

        // Token: 0x0400166D RID: 5741
        public static float aimVelocity;

        // Token: 0x0400166E RID: 5742
        public static float upwardVelocity;

        // Token: 0x0400166F RID: 5743
        public static float forwardVelocity;

        // Token: 0x04001670 RID: 5744
        public static float minimumY;

        // Token: 0x04001671 RID: 5745
        public static float minYVelocityForAnim;

        // Token: 0x04001672 RID: 5746
        public static float maxYVelocityForAnim;

        // Token: 0x04001673 RID: 5747
        public static float knockbackForce;

        // Token: 0x04001674 RID: 5748
        [SerializeField]
        public GameObject fistEffectPrefab;

        // Token: 0x04001675 RID: 5749
        public static string soundLoopStartEvent;

        // Token: 0x04001676 RID: 5750
        public static string soundLoopStopEvent;

        // Token: 0x04001677 RID: 5751
        public static NetworkSoundEventDef landingSound;

        // Token: 0x04001679 RID: 5753
        private GameObject leftFistEffectInstance;

        // Token: 0x0400167A RID: 5754
        private GameObject rightFistEffectInstance;

        // Token: 0x0400167B RID: 5755
        protected bool isCritAuthority;

        // Token: 0x0400167C RID: 5756
        protected CrocoDamageTypeController crocoDamageTypeController;

        // Token: 0x0400167D RID: 5757
        private bool detonateNextFrame;
    }
}