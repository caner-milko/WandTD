using UnityEngine;

namespace wtd.effect
{
	public abstract class EffectTrigger : MonoBehaviour
	{
		[field: SerializeField]
		Effect effect;
		private void OnValidate()
		{
			if (effect == null)
				effect = GetComponent<Effect>();
		}
		public void Trigger()
		{
			effect.Holder.RemoveEffect(effect);
		}

		public abstract void Renew(Effect newEffect);
	}
}
