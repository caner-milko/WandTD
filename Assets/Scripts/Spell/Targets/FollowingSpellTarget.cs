using UnityEngine;

namespace wtd.spell.targets
{
	public class FollowingSpellTarget : ISpellTarget
	{
		// Start is called before the first frame update
		public Transform following;

		public FollowingSpellTarget(Transform following)
		{
			this.following = following;
		}

		public Vector3 GetPosition()
		{
			return following.position;
		}

		public string GetTargetType()
		{
			return "ST_following";
		}


	}
}
