using UnityEngine;

namespace wtd.effect
{
	public abstract class Effect : MonoBehaviour
	{
		[field: SerializeField]
		public string EffectName { get; private set; }

		[field: SerializeField]
		public bool CanStack { get; private set; }

		public EffectHolder Holder { get; private set; }

		[field: SerializeField]
		public EffectTrigger EffTrigger { get; private set; }

		private void Start()
		{
			if (EffTrigger == null)
				EffTrigger = GetComponent<EffectTrigger>();
		}

		public void Add(EffectHolder holder)
		{
			this.Holder = holder;
			OnAdd();
		}

		public void Remove(bool removeFromHolder = false)
		{
			if (removeFromHolder)
				Holder.RemoveEffect(this);
			else
				OnRemove();
			Destroy(gameObject);
		}

		private void Update()
		{
			if (Holder == null)
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
			if (EffTrigger != null)
				EffTrigger.Renew(newEffect);
		}

		protected abstract void OnAdd();

		protected abstract void OnUpdate();

		protected abstract void OnTrigger();

		protected abstract void OnRemove();

		protected abstract void OnRenew(Effect newEffect);
	}
}
