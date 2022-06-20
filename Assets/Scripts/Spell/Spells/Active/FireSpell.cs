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


		protected override void OnUpdate()
		{

		}

		protected override void OnFixedUpdate()
		{
			Vector3 vel = castedParent.target.GetVelocityVector(castedParent.transform.position, speedStat.Value);
			if (vel.sqrMagnitude < 0.001f)
				return;
			castedParent.transform.Translate(vel * Time.deltaTime);
		}


	}
}
