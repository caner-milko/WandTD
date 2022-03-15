using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using wtd.wands;

namespace wtd.wands.targets
{
    public class FollowingSpellTarget : SpellTarget
    {
        // Start is called before the first frame update
        public Transform following;

        public FollowingSpellTarget(Transform following)
        {
            this.following = following;
        }

        public Vector3 GetPosition()
        {
            return following.position;
        }

        public string GetTargetType()
        {
            return "ST_following";
        }
    }
}
