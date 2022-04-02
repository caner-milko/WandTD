using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wands.spells
{
    public class BlueSpell : ActiveSpell
    {
        public override bool checkHit(CastedSpell casted, out List<ISpellTarget> hitList)
        {
            hitList = new List<ISpellTarget>();
            return true;
        }

        public override SpellHit Hit(CastedSpell from, ISpellTarget target)
        {
            throw new System.NotImplementedException();
        }

        public override string SpellType()
        {
            return "AS_blue";
        }


        public override void OnTick(CastedSpell casted)
        {
            Vector3 vel = (casted.target.GetPosition() - casted.transform.position).normalized * 1.0f;
            casted.transform.Translate(vel * Time.deltaTime);
        }


    }
}
