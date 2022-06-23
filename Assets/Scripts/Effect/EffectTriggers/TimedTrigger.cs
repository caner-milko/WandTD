using UnityEngine;

namespace wtd.effect.triggers
{
	public class TimedTrigger : EffectTrigger
	{
		[field: SerializeField]
		public float TriggerTime { get; private set; }
		public float CurTime { get; private set; }

		public float timeMultiplier = 1f;

		// Update is called once per frame
		void Update()
		{
			CurTime += UnityEngine.Time.deltaTime;
			if (CurTime > TriggerTime)
			{
				Trigger();
			}
		}

		public override void Renew(Effect newEffect)
		{
			Debug.Log("Timer renewed after " + CurTime + " seconds.");
			this.CurTime = 0;
		}

	}
}
