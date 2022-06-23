using UnityEngine;

namespace wtd.wave.events
{
	public class TestWaveEvent : WaveEvent
	{
		public static int lastId = 0;
		public float aliveTime;
		float curAliveTime;
		int id = 0;

		public override void ClearEvent()
		{
			Debug.Log("Cleared test event " + id + " at " + WaveManager.Instance.tickTime + "s");
		}

		public override bool Finished()
		{
			return curAliveTime >= aliveTime;
		}


		public override void SpawnEvent()
		{
			id = lastId++;
			Debug.Log("Spawned test event " + id + " at " + WaveManager.Instance.tickTime + "s");
		}

		public override void DoUpdate()
		{
			curAliveTime += Time.deltaTime;

		}
	}
}
