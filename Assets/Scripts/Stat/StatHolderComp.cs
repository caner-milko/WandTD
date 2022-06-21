using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.stat
{
	public class StatHolderComp : MonoBehaviour
	{
		[field: SerializeField]
		public StatHolder statHolder { get; private set; } = new StatHolder();
		// Start is called before the first frame update
		void Awake()
		{
			statHolder.setup();
		}
	}
}
