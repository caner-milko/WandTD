using UnityEngine;
using wtd.stat;
namespace wtd.spell.spells
{
	public class BlueSpell : ActiveSpell
	{
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speedStat = new(StatNames.SPEED, null, 3);
		protected override void OnCast()
		{
			((IStatUser)this).SetupStats();
		}



		protected override void OnRemove()
		{
		}


		protected override void OnUpdate()
		{
		}
		protected override void OnFixedUpdate()
		{
			Vector3 vel = CastedParent.Target.GetVelocityVector(CastedParent.transform.position, speedStat.Value);
			if (vel.sqrMagnitude < 0.001f)
				return;
			CastedParent.transform.Translate(vel * Time.deltaTime);
		}

	}
}
