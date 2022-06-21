using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
namespace wtd.spell.spells
{
	public class FireSpell : ActiveSpell
	{
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speedStat = new Stat(StatNames.SPEED, null, 3);
		protected override void OnCast()
		{
			StatUtils.SetupStats(this);
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
