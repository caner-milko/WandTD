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

        int a;


        // Start is called before the first frame update
        void Start()
        {
            wand.AddSpell(SpellManager.manager.GetSpellByType("PS_test"));
            //wand.AddSpell(SpellManager.manager.GetSpellByType("PS_multicastTest"));
            wand.AddSpell(SpellManager.manager.GetSpellByType("AS_fire"));
            wand.AddSpell(SpellManager.manager.GetSpellByType("AS_blue"));
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                List<CastedSpell> casted;
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