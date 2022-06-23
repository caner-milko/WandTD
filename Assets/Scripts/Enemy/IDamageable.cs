using wtd.stat;
namespace wtd
{
	public interface IDamageable
	{

		public float GetHealth();

		//TODO add inflictor
		public float InflictDamage(StatHolder damageStats);

		public float Heal(StatHolder healStats);
	}
}
