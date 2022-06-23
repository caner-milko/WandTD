using System.Collections.Generic;
using UnityEngine;
using wtd.spell;
using wtd.tower;

namespace wtd
{
	public class TowerTest : MonoBehaviour
	{

		public Tower tower;

		public List<Spell> spells;

		void Start()
		{
			foreach (Spell spell in spells)
				tower.wand.AddSpell(spell);
		}


		// Update is called once per frame
		void Update()
		{
			if (Input.GetKeyDown(KeyCode.Space))
			{
				tower.Shoot(out _);
			}
		}
	}
}
