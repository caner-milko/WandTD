using UnityEngine;
using wtd.enemy;
using wtd.stat;

namespace wtd.spell.spells
{
	public class ExplodingSpell : ActiveSpell
	{
		[SerializeField, AutoCopyStat("explosionRadius")]
		private Stat explosionRadius = new("explosionRadius", null, 3f);
		[SerializeField, AutoCopyStat(StatNames.DAMAGE)]
		private Stat damage = new(StatNames.DAMAGE, null, 5f);
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speed = new(StatNames.SPEED, null, 5f);

		[SerializeField]
		private CollisionSpellTrigger collTrigger;
		[SerializeField]
		private TimedSpellTrigger timedTrigger;

		private Rigidbody body;
		protected override void OnCast()
		{
			((IStatUser)this).SetupStats();

			collTrigger = (CollisionSpellTrigger)CastedParent.AddTrigger(collTrigger, true);
			body = collTrigger.GetComponent<Rigidbody>();
			timedTrigger = (TimedSpellTrigger)CastedParent.AddTrigger(timedTrigger, true);
			CastedParent.SetMainMesh(body.transform);
			body.velocity = CastedParent.Target.GetVelocityVector(transform.position, speed.Value);
		}

		protected override void OnFixedUpdate()
		{
			body.velocity = body.velocity.normalized * speed.Value;
		}

		protected override void OnRemove()
		{

		}

		protected override void OnUpdate()
		{

		}

		public override bool HitTrigger(CollisionSpellTrigger.CollisionSpellTriggerData triggerData)
		{
			CreateExplosion();
			return true;
		}

		public override bool TimerTrigger(TimedSpellTrigger.TimedSpellTriggerData triggerData)
		{
			CreateExplosion();
			return true;
		}

		private void CreateExplosion()
		{
			Vector3 pos = CastedParent.transform.position;
			float expRad = explosionRadius.Value;
			Collider[] collided = Physics.OverlapSphere(pos, expRad, LayerMask.GetMask("Enemy"));
			foreach (Collider coll in collided)
			{
				float calcDamage = damage.Value;
				//square damage falloff
				calcDamage *= 1 - coll.ClosestPoint(pos).sqrMagnitude / (expRad * expRad);
				StatHolder calcDamageStat = new();
				calcDamageStat.AddStat(new Stat("damage", calcDamageStat, calcDamage));
				if (coll.TryGetComponent(out Enemy enemy))
				{
					enemy.InflictDamage(calcDamageStat);
					Debug.Log("Inflicted " + calcDamage + " on " + enemy.name + ".");
				}
				else
				{
					Debug.Log("Couldn't detect as enemy");
				}
			}
		}

	}
}
