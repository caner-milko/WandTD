using UnityEngine;
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
			Vector3 dif = CastedParent.Target.GetPosition() - CastedParent.transform.position;
			CastedParent.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f) * (dif.magnitude + 0.05f);
		}

	}
}
