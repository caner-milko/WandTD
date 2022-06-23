using UnityEngine;

namespace wtd.wave
{
	public abstract class WaveEvent : MonoBehaviour
	{

		public SpawnTrigger trigger;
		public bool spawned = false;
		public bool finished = false;
		public abstract void SpawnEvent();

		public abstract bool Finished();

		public virtual bool IsActive()
		{
			return spawned && !finished;
		}


		public abstract void ClearEvent();

		public abstract void DoUpdate();

	}
}
