using UnityEngine;

namespace wtd.spell
{
	public class TimedSpellTrigger : SpellTrigger
	{
		public struct TimedSpellTriggerData
		{
			public readonly TimedSpellTrigger trigger;
			public CastedSpell Casted => trigger.Casted;

			public readonly bool weak;

			public TimedSpellTriggerData(TimedSpellTrigger trigger, bool weak)
			{
				this.trigger = trigger;
				this.weak = weak;
			}
		}
		[field: SerializeField]
		public float Timer { get; private set; } = 3.0f;

		[field: SerializeField]
		public bool WeakTrigger { get; private set; } = false;

		private float curTime = 0.0f;

		private void Update()
		{
			if (curTime >= Timer)
			{
				return;
			}

			curTime += Time.deltaTime;

			if (curTime >= Timer)
			{
				TimedSpellTriggerData spellTriggerData = new(this, WeakTrigger);
				Casted.TimerTrigger(spellTriggerData);
			}
		}
	}
}
