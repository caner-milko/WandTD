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
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <returns>Direction to target from <paramref name="from"/></returns>
		public Vector3 GetDirection(Vector3 from);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <returns>Distance to target from <paramref name="from"/>, or -1 if the distance is infinite</returns>
		public float DistanceToSqr(Vector3 from);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="from"></param>
		/// <param name="speed"></param>
		/// <returns><see cref="GetDirection(Vector3)"/> multiplied by speed or sqrt(<see cref="DistanceToSqr(Vector3)"/>), whichever is lower</returns>
		public Vector3 GetVelocityVector(Vector3 from, float speed);

		/// <summary>
		/// Should be unique and static for each implementation<br/>
		/// Naming Format: "ST_camelCase"
		/// </summary>
		/// <returns></returns>
		public string GetTargetType();

	}
}
