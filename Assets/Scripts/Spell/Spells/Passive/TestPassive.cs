using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wand;
namespace wtd.spell.spells
{
	public class TestPassive : PassiveSpell
	{
		protected override void OnCast()
		{

		}

		protected override void OnFixedUpdate()
		{

		}

		protected override void OnRemove()
		{

		}


		protected override void OnUpdate()
		{
			Vector3 dif = castedParent.target.GetPosition() - castedParent.transform.position;
			castedParent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (dif.magnitude + 0.05f);
		}

	}
}
