using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
namespace wtd.spell.spells
{
	public class FireSpell : ActiveSpell
	{
		[ReadOnly, SerializeField]
		private Stat speedStat;
		protected override void OnCast()
		{
			speedStat = castedParent.statHolder.GetStat(StatName.SPEED);
		}

		protected override void OnRemove()
		{

		}

		protected override void OnTrigger(SpellTriggerData trigger)
		{

		}

		protected override void OnUpdate()
		{
			Vector3 vel = (castedParent.target.GetPosition() - castedParent.transform.position).normalized * speedStat.Value;
			castedParent.transform.Translate(vel * Time.deltaTime);
		}

		protected override void OnFixedUpdate()
		{
			Vector3 vel = (castedParent.target.GetPosition() - castedParent.transform.position).normalized * speedStat.Value;
			castedParent.transform.Translate(vel * Time.deltaTime);
		}


	}
}
