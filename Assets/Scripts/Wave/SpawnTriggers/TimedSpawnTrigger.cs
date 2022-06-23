namespace wtd.wave.triggers
{
	public class TimedSpawnTrigger : SpawnTrigger
	{
		public float spawnTime;

		public override bool ShouldTrigger()
		{
			return WaveManager.Instance.tickTime >= spawnTime;
		}

		public override void Trigger()
		{
		}
	}
}
