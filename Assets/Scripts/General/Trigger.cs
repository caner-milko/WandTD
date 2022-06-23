using UnityEngine;
using UnityEngine.Events;
namespace wtd
{
	public abstract class Trigger : MonoBehaviour
	{
		public UnityEvent<Trigger> triggerEvent;

		protected void FireEvent()
		{
			if (triggerEvent != null)
				triggerEvent.Invoke(this);
		}
	}
}
