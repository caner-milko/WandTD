using UnityEngine;

namespace wtd.wave
{
	public abstract class SpawnTrigger : MonoBehaviour
	{
		public abstract bool ShouldTrigger();
		public abstract void Trigger();
	}
}
