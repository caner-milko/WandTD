using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;
using wtd.wands.targets;
using wtd.wands.spells;

namespace wtd
{
    public class Player : MonoBehaviour
    {
        public Wand wand;

        // alternate between targeting types, used for test purposes
        int a;



        void Start()
        {
            // Added manually since there is no wand editing yet
            wand.AddSpell(SpellManager.instance.GetSpellByType("PS_test"));
            //wand.AddSpell(SpellManager.manager.GetSpellByType("PS_multicastTest"));
            wand.AddSpell(SpellManager.instance.GetSpellByType("AS_fire"));
            wand.AddSpell(SpellManager.instance.GetSpellByType("AS_blue"));
            wand.AddSpell(SpellManager.instance.GetSpellByType("AS_blue"));
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                //output casted spell list
                List<CastedSpell> casted;
                //a target is required to shoot a spell
                ISpellTarget target;
                if (a++ % 2 == 0)
                {
                    target = new StaticSpellTarget(new Vector3(0.0f, 0.0f, 0.0f));
                }
                else
                {
                    target = new FollowingSpellTarget(transform);
                }
                wand.Shoot(target, out casted);
            }

            //basic movement
            Vector3 vel = new Vector3(0.0f, 0.0f, 0.0f);

            if (Input.GetKey(KeyCode.W))
            {
                vel.y++;
            }

            if (Input.GetKey(KeyCode.D))
            {
                vel.x++;
            }

            if (Input.GetKey(KeyCode.S))
            {
                vel.y--;
            }

            if (Input.GetKey(KeyCode.A))
            {
                vel.x--;
            }

            vel.Normalize();


            transform.Translate(vel * 5.0f * Time.deltaTime);

        }
    }
}