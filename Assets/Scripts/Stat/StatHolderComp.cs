using UnityEngine;

namespace wtd.stat
{
	public class StatHolderComp : MonoBehaviour
	{
		[field: SerializeField]
		public StatHolder RealHolder { get; private set; } = new StatHolder();
		// Start is called before the first frame update
		void Awake()
		{
			RealHolder.Setup();
		}
	}
}
