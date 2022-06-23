using UnityEngine;


namespace wtd.spell.targets
{
	class StaticSpellTarget : ISpellTarget
	{
		public Vector3 position;

		public StaticSpellTarget(Vector3 position)
		{
			this.position = position;
		}


		public Vector3 GetPosition()
		{
			return position;
		}

		public string GetTargetType()
		{
			return "ST_static";
		}
	}
}
