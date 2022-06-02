using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.effect.effects
{
	public class TestEffect : Effect
	{
		protected override void OnAdd()
		{
			Debug.Log("Added effect to " + holder.gameObject.name);
		}

		protected override void OnRemove()
		{
			Debug.Log("Removed effect from " + holder.gameObject.name);
		}

		protected override void OnRenew(Effect newEffect)
		{
			Debug.Log("Renewed effect on " + holder.gameObject.name);
		}

		protected override void OnTrigger()
		{
			Debug.Log("Triggered effect on " + holder.gameObject.name);
			Remove(true);
		}

		protected override void OnUpdate()
		{
			//Debug.Log("Updated effect on " + holder.gameObject.name);
		}
	}
}
