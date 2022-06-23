using System.Collections.Generic;
using UnityEngine;

namespace wtd.effect
{
	public class EffectHolder : MonoBehaviour
	{

		[field: SerializeField]
		public List<Effect> Effects { get; private set; } = new List<Effect>();

		[SerializeField]
		private Transform EffectsParent;

		private void Start()
		{
			foreach (Effect effect in Effects)
			{
				effect.Add(this);
			}
			if (EffectsParent == null || !EffectsParent)
			{
				bool found = false;
				foreach (Transform tf in transform)
					if (tf.gameObject.name == "Effects")
					{
						EffectsParent = tf;
						found = true;
					}
				if (!found)
				{
					EffectsParent = new GameObject("Effects").transform;
					EffectsParent.parent = transform;
				}
			}
			foreach (Effect effect in EffectsParent.GetComponentsInChildren<Effect>())
			{
				AddEffect(effect, false);
			}
		}

		private void OnDestroy()
		{
			foreach (Effect effect in Effects)
			{
				effect.Remove();
			}
		}

		public void AddEffect(Effect effect, bool instantiate)
		{
			if (!effect.CanStack)
			{
				List<Effect> list = GetEffectByType<Effect>(effect.EffectName);
				if (list.Count > 0)
				{
					Effect eff = list[0];
					eff.Renew(effect);
					return;
				}
			}
			Effect realEffect = effect;
			if (instantiate)
			{
				realEffect = GameObject.Instantiate<Effect>(effect, EffectsParent);
			}
			Effects.Add(realEffect);
			realEffect.Add(this);
		}

		public void RemoveEffect(Effect effect)
		{
			effect.Remove();
			Effects.Remove(effect);
		}

		public void ClearEffects()
		{
			foreach (Effect effect in Effects)
				effect.Remove();
			Effects.Clear();
		}

		public List<T> GetEffectByType<T>(string effectName) where T : Effect
		{
			List<T> effectsByType = new();
			foreach (Effect effect in Effects)
				if (effect.EffectName == effectName && effect is T t)
					effectsByType.Add(t);
			return effectsByType;
		}

	}
}
