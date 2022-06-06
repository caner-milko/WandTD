using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.spell;

namespace wtd.tower.targets
{
	public class PlayerTTarget : TowerTarget
	{
		// Start is called before the first frame update
		public override bool GetTarget(out ISpellTarget target)
		{
			PlayerTest player = GameManager.instance.Player;
			target = player;
			return (Owner.GetPosition() - player.GetPosition()).sqrMagnitude <= Owner.RangeSqr;
		}
	}
}
