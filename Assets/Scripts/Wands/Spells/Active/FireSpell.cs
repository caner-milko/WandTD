using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands.spells
{
    public class FireSpell : ActiveSpell
    {
        public override bool checkHit(CastedSpell casted, out List<SpellTarget> hitList)
        {
            hitList = new List<SpellTarget>();
            return true;
        }

        public override SpellHit Hit(CastedSpell from, SpellTarget target)
        {
            throw new System.NotImplementedException();
        }

        public override string SpellType()
        {
            return "AS_fire";
        }


        public override void OnTick(CastedSpell casted)
        {
            Vector3 vel = (casted.target.GetPosition() - casted.transform.position).normalized * 3.0f;
            casted.transform.Translate(vel * Time.deltaTime);
        }


    }
}
