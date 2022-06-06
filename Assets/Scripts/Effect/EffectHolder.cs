using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.effect
{
	public class EffectHolder : MonoBehaviour
	{

		[field: SerializeField]
		public List<Effect> effects { get; private set; } = new List<Effect>();

		[SerializeField]
		private Transform EffectsParent;

		private void Start()
		{
			foreach (Effect effect in effects)
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
			foreach (Effect effect in effects)
			{
				effect.Remove();
			}
		}

		public void AddEffect(Effect effect, bool instantiate)
		{
			if (!effect.canStack)
			{
				List<Effect> list = GetEffectByType<Effect>(effect.effectName);
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
			effects.Add(realEffect);
			realEffect.Add(this);
		}

		public void RemoveEffect(Effect effect)
		{
			effect.Remove();
			effects.Remove(effect);
		}

		public void ClearEffects()
		{
			foreach (Effect effect in effects)
				effect.Remove();
			effects.Clear();
		}

		public List<T> GetEffectByType<T>(string effectName) where T : Effect
		{
			List<T> effectsByType = new List<T>();
			foreach (Effect effect in effects)
				if (effect.effectName == effectName && effect is T)
					effectsByType.Add((T)effect);
			return effectsByType;
		}

	}
}
