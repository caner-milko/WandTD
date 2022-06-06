using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.tower;
using wtd.spell;

namespace wtd
{
	public class TowerTest : MonoBehaviour
	{

		public Tower tower;

		public List<SpellData> spells;

		void Start()
		{
			foreach (SpellData data in spells)
				tower.wand.AddSpell(data);
		}


		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				tower.shoot(out _);
			}
		}
	}
}
