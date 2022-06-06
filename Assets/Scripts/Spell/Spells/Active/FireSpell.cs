using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell.spells
{
	public class FireSpell : ActiveSpell
	{
		protected override void OnCast()
		{

		}

		protected override void OnRemove()
		{

		}

		protected override void OnTrigger(SpellTriggerData trigger)
		{

		}

		protected override void OnUpdate()
		{
			Vector3 vel = (castedParent.target.GetPosition() - castedParent.transform.position).normalized * 3.0f;
			castedParent.transform.Translate(vel * Time.deltaTime);
		}


	}
}
