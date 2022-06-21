using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.stat;
namespace wtd.effect.effects
{
	public class TestEffect : Effect, IStatUser
	{
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speedStat = new Stat(StatNames.SPEED, null, 3);
		protected override void OnAdd()
		{
			Debug.Log("Added effect to " + holder.gameObject.name);
			StatUtils.SetupStats(this);
			speedStat.AddEffector(new StatEffector(0.5f, this, StatEffector.StatEffectorType.PercentAdd)
				, new StatEffector(2f, this, StatEffector.StatEffectorType.Add),
				new StatEffector(1f, this, StatEffector.StatEffectorType.PercentAdd));
		}

		protected override void OnRemove()
		{
			Debug.Log("Removed effect from " + holder.gameObject.name);
			if (speedStat != null)
				speedStat.RemoveEffectorsFromSource(this);
		}

		protected override void OnRenew(Effect newEffect)
		{
			Debug.Log("Renewed effect on " + holder.gameObject.name);
		}

		protected override void OnTrigger()
		{
			Debug.Log("Triggered effect on " + holder.gameObject.name);
			Remove(true);
		}

		protected override void OnUpdate()
		{
			//Debug.Log("Updated effect on " + holder.gameObject.name);
		}

		public StatHolder GetStatHolder()
		{
			return holder.GetComponent<StatHolderComp>().statHolder;
		}

	}
}
