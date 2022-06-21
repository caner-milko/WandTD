using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
using wtd.enemy;

namespace wtd.spell.spells
{
	public class ExplodingSpell : ActiveSpell
	{
		[SerializeField, AutoCopyStat("explosionRadius")]
		private Stat explosionRadius = new Stat("explosionRadius", null, 3f);
		[SerializeField, AutoCopyStat(StatNames.DAMAGE)]
		private Stat damage = new Stat(StatNames.DAMAGE, null, 5f);
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speed = new Stat(StatNames.SPEED, null, 5f);

		[SerializeField]
		private CollisionSpellTrigger collTrigger;
		[SerializeField]
		private TimedSpellTrigger timedTrigger;

		private Rigidbody body;
		protected override void OnCast()
		{
			StatUtils.SetupStats(this);

			collTrigger = (CollisionSpellTrigger)castedParent.AddTrigger(collTrigger, true);
			body = collTrigger.GetComponent<Rigidbody>();
			timedTrigger = (TimedSpellTrigger)castedParent.AddTrigger(timedTrigger, true);
			castedParent.SetMainMesh(body.transform);
			body.velocity = castedParent.target.GetVelocityVector(transform.position, speed.Value);
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
			Vector3 pos = castedParent.transform.position;
			float expRad = explosionRadius.Value;
			Collider[] collided = Physics.OverlapSphere(pos, expRad, LayerMask.GetMask("Enemy"));
			foreach (Collider coll in collided)
			{
				float calcDamage = damage.Value;
				//square damage falloff
				calcDamage *= 1 - coll.ClosestPoint(pos).sqrMagnitude / (expRad * expRad);
				StatHolder calcDamageStat = new StatHolder();
				calcDamageStat.AddStat(new Stat("damage", calcDamageStat, calcDamage));
				Enemy enemy = coll.GetComponent<Enemy>();
				if (enemy != null)
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
