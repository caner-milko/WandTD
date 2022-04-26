using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower
{
	public class TowerManager : MonoBehaviour
	{
		public static TowerManager instance { get; private set; }

		private void Awake()
		{
			instance = this;
		}

		[SerializeField]
		private Transform towersPrefab;

		[field: SerializeField]
		public List<Tower> Towers { get; private set; } = new List<Tower>();





		public Tower CreateTower(Tower prefab)
		{
			Tower created = GameObject.Instantiate<Tower>(prefab);
			Towers.Add(created);
			return created;
		}

		public void RemoveTower(Tower tower)
		{
			//TODO Drop wand, delete spells casted by tower etc.
			GameObject.Destroy(tower);
			Towers.Remove(tower);
		}

		public Tower GetTowerFromMouse()
		{
			Vector2 mousePos = CameraManager.instance.WorldMousePos;
			RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 1f, LayerMask.GetMask("tower"));
			if (hit.collider == null)
				return null;
			return hit.transform.gameObject.GetComponent<Tower>();
		}

		public Tower GetClosestTower(Vector3 pos)
		{
			Tower closest = null;
			float difSqr = -1f;
			foreach (Tower tower in Towers)
			{
				float curDifSqr = (pos - tower.GetPosition()).sqrMagnitude;
				if (difSqr < 0f || curDifSqr < difSqr)
				{
					difSqr = curDifSqr;
					closest = tower;
				}
			}
			return closest;
		}

	}
}
