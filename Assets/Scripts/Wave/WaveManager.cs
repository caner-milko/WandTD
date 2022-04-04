using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace wtd.wave
{
    public class WaveManager : MonoBehaviour
    {
        public static WaveManager Instance;

        public Transform PlannedEventsParent;
        public Transform ActiveEventsParent;
        public Transform FinishedEventsParent;
        public bool takePlannedChilds = true;

        public List<WaveEvent> plannedEvents = new List<WaveEvent>();
        List<WaveEvent> activeEvents = new List<WaveEvent>();
        List<WaveEvent> finishedEvents = new List<WaveEvent>();

        public bool tick = false;

        public float tickTime = 0.0f;

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            if (takePlannedChilds)
            {
                WaveEvent[] wevents = PlannedEventsParent.GetComponentsInChildren<WaveEvent>();
                foreach (WaveEvent wevent in wevents)
                {
                    if (!plannedEvents.Contains(wevent))
                        plannedEvents.Add(wevent);
                }
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (!tick)
                return;
            tickTime += Time.deltaTime;

            foreach (WaveEvent wevent in plannedEvents)
            {
                if (!wevent.trigger.ShouldTrigger())
                    continue;
                wevent.trigger.Trigger();
                wevent.SpawnEvent();
                wevent.spawned = true;
                wevent.gameObject.transform.parent = ActiveEventsParent;
                activeEvents.Add(wevent);
            }

            plannedEvents.RemoveAll((wevent) =>
            {
                return wevent.spawned;
            });
            foreach (WaveEvent wevent in activeEvents)
            {
                if (wevent.Finished())
                {
                    wevent.ClearEvent();
                    wevent.finished = true;
                    wevent.gameObject.transform.parent = FinishedEventsParent;
                    finishedEvents.Add(wevent);
                }
                else
                {
                    wevent.DoUpdate();
                }
            }
            activeEvents.RemoveAll((wevent) =>
            {
                return wevent.finished;
            });
        }

        public void StartWave()
        {
            tick = true;
        }

    }
}
