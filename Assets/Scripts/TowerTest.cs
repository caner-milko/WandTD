using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.tower;
using wtd.wands;

namespace wtd
{
    public class TowerTest : MonoBehaviour
    {

        public Tower tower;

        void Start()
        {
            // Added manually since there is no wand editing yet
            tower.wand.AddSpell(SpellManager.instance.GetSpellByType("PS_test"));
            //wand.AddSpell(SpellManager.manager.GetSpellByType("PS_multicastTest"));
            tower.wand.AddSpell(SpellManager.instance.GetSpellByType("AS_fire"));
            tower.wand.AddSpell(SpellManager.instance.GetSpellByType("AS_blue"));
            tower.wand.AddSpell(SpellManager.instance.GetSpellByType("AS_blue"));
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
