using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	public class TimedSpellTrigger : SpellTrigger
	{
		public struct TimedSpellTriggerData
		{
			public readonly TimedSpellTrigger trigger;
			public CastedSpell casted => trigger.casted;

			public readonly bool weak;

			public TimedSpellTriggerData(TimedSpellTrigger trigger, bool weak)
			{
				this.trigger = trigger;
				this.weak = weak;
			}
		}
		[field: SerializeField]
		public float timer { get; private set; } = 3.0f;

		[field: SerializeField]
		public bool weakTrigger { get; private set; } = false;

		private float curTime = 0.0f;

		private void Update()
		{
			if (curTime >= timer)
			{
				return;
			}

			curTime += Time.deltaTime;

			if (curTime >= timer)
			{
				TimedSpellTriggerData spellTriggerData = new TimedSpellTriggerData(this, weakTrigger);
				casted.TimerTrigger(spellTriggerData);
			}
		}
	}
}
