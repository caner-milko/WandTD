using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.effect.triggers
{
	public class TimedTrigger : EffectTrigger
	{
		[field: SerializeField]
		public float time { get; private set; }
		public float curTime { get; private set; }

		public float timeMultiplier = 1f;

		// Update is called once per frame
		void Update()
		{
			curTime += Time.deltaTime;
			if (curTime > time)
			{
				Trigger();
			}
		}

		public override void Renew(Effect newEffect)
		{
			Debug.Log("Timer renewed after " + curTime + " seconds.");
			this.curTime = 0;
		}

	}
}
