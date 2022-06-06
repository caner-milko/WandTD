using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.spell
{
	/// <summary>
	/// Each target should have a position and a static unique <see cref="GetTargetType"/>
	/// </summary>
	public interface ISpellTarget
	{
		/// <summary>
		/// Current position of the target
		/// </summary>
		/// <returns></returns>
		public Vector3 GetPosition();

		/// <summary>
		/// Should be unique and static for each implementation<br/>
		/// Naming Format: "ST_camelCase"
		/// </summary>
		/// <returns></returns>
		public string GetTargetType();
	}
}