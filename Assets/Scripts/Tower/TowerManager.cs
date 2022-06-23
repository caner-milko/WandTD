using System.Collections.Generic;
using UnityEngine;

namespace wtd.tower
{
	public class TowerManager : MonoBehaviour
	{
		public static TowerManager Instance { get; private set; }

		private void Awake()
		{
			Instance = this;
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
			Ray mouseRay = CameraManager.Instance.MouseRay;
			if (!Physics.Raycast(mouseRay, out RaycastHit hit, 100f, LayerMask.GetMask("tower")))
				return null;
			return hit.transform.gameObject.GetComponent<Tower>();
		}

		//based on position
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

		public Collider[] GetHittingTowersAsCollider(Vector3 pos, float radius)
		{
			return Physics.OverlapSphere(pos, radius, LayerMask.GetMask("tower"));
		}

		public Tower[] GetHittingTowers(Vector3 pos, float radius)
		{
			Collider[] colliders = GetHittingTowersAsCollider(pos, radius);
			Tower[] towers = new Tower[colliders.Length];
			for (int i = 0; i < colliders.Length; i++)
				towers[i] = colliders[i].GetComponent<Tower>();
			return towers;
		}

		public Tower GetClosestTower(Vector3 pos, float radius)
		{
			Collider[] colliders = GetHittingTowersAsCollider(pos, radius);
			float minDistSqr = -1f;
			Collider selected = null;
			foreach (Collider collider in colliders)
			{
				float distSqr = (collider.ClosestPoint(pos) - pos).sqrMagnitude;
				if (selected == null || distSqr < minDistSqr)
				{
					minDistSqr = distSqr;
					selected = collider;
				}
			}
			return selected.GetComponent<Tower>();
		}

		public bool IsCloseToTower(Vector3 pos, float radius, Collider collider)
		{
			Collider[] colliders = GetHittingTowersAsCollider(pos, radius);
			return new HashSet<Collider>(colliders).Contains(collider);
		}

		public bool IsCloseToTower(Vector3 pos, float radius, Tower tower)
		{
			return IsCloseToTower(pos, radius, tower.GetComponent<Collider>());
		}
	}
}
