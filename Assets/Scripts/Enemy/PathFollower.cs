using UnityEngine;
using wtd.stat;
namespace wtd.enemy
{
	[RequireComponent(typeof(StatHolderComp))]
	public class PathFollower : MonoBehaviour, IStatUser
	{
		private StatHolderComp statHolderComp;

		[SerializeField, AutoCopyStat(StatNames.SPEED)]
		private Stat speedStat = new(StatNames.SPEED, null, 3);

		[field: SerializeField]
		public EnemyPath Path { get; private set; }
		public int lastPoint = -1;
		public bool Finished => lastPoint >= Path.Positions.Count - 1;
		public Vector3 NextPosition
		{
			get
			{
				return Finished ? Path.Positions[lastPoint] : Path.Positions[lastPoint + 1];
			}
		}

		[SerializeField]
		private bool loop = false;

		private void Awake()
		{
			statHolderComp = GetComponent<StatHolderComp>();
			((IStatUser)this).SetupStats();
		}

		void Start()
		{
			if (Path != null && Path.Positions.Count > 0)
			{
				lastPoint = 0;
				transform.position = Path.Positions[0];
			}
		}

		// Update is called once per frame
		void FixedUpdate()
		{
			if (!Finished)
			{
				if (CheckNext())
				{
					lastPoint++;
				}
				TranslateToNext();
			}
			else if (loop)
			{
				lastPoint = -1;
			}
		}

		private void TranslateToNext()
		{
			Vector3 dif = NextPosition - transform.position;
			float speed = Mathf.Max(speedStat.Value, dif.magnitude);


			dif = speed * Time.fixedDeltaTime * dif.normalized;
			transform.Translate(dif);
		}

		private bool CheckNext()
		{
			Vector3 dif = NextPosition - transform.position;
			return dif.sqrMagnitude <= 0.03f;
		}

		public StatHolder GetStatHolder()
		{
			return statHolderComp.RealHolder;
		}
	}
}
