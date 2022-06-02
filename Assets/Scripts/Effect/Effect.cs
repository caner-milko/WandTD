using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.effect
{
	public abstract class Effect : MonoBehaviour
	{
		[field: SerializeField]
		public string effectName { get; private set; }

		[field: SerializeField]
		public bool canStack { get; private set; }

		public EffectHolder holder { get; private set; }

		[field: SerializeField]
		public EffectTrigger trigger { get; private set; }

		private void Start()
		{
			if (trigger == null)
				trigger = GetComponent<EffectTrigger>();
		}

		public void Add(EffectHolder holder)
		{
			this.holder = holder;
			OnAdd();
		}

		public void Remove(bool removeFromHolder = false)
		{
			if (removeFromHolder)
				holder.RemoveEffect(this);
			else
				OnRemove();
			Destroy(gameObject);
		}

		private void Update()
		{
			if (holder == null)
			{
				return;
			}
			OnUpdate();
		}

		public void Trigger()
		{
			OnTrigger();
		}

		public void Renew(Effect newEffect)
		{
			OnRenew(newEffect);
			if (trigger != null)
				trigger.Renew(newEffect);
		}

		protected abstract void OnAdd();

		protected abstract void OnUpdate();

		protected abstract void OnTrigger();

		protected abstract void OnRemove();

		protected abstract void OnRenew(Effect newEffect);
	}
}
