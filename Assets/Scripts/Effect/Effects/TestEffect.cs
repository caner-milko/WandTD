using UnityEngine;
using wtd.stat;
namespace wtd.effect.effects
{
	public class TestEffect : Effect, IStatUser
	{
		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speedStat = new(StatNames.SPEED, null, 3);
		protected override void OnAdd()
		{
			((IStatUser)this).SetupStats();

			speedStat.AddEffector(new StatEffector(0.5f, this, StatEffector.StatEffectorType.PercentAdd)
				, new StatEffector(2f, this, StatEffector.StatEffectorType.Add),
				new StatEffector(1f, this, StatEffector.StatEffectorType.PercentAdd));
		}

		protected override void OnRemove()
		{
			if (speedStat != null)
				speedStat.RemoveEffectorsFromSource(this);
		}

		protected override void OnRenew(Effect newEffect)
		{
			Debug.Log("Renewed effect on " + Holder.gameObject.name);
		}

		protected override void OnTrigger()
		{
			Debug.Log("Triggered effect on " + Holder.gameObject.name);
			Remove(true);
		}

		protected override void OnUpdate()
		{
			//Debug.Log("Updated effect on " + holder.gameObject.name);
		}

		public StatHolder GetStatHolder()
		{
			return Holder.GetComponent<StatHolderComp>().RealHolder;
		}

	}
}
