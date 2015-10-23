using UnityEngine;
using System;
using System.Collections; 
using System.Collections.Generic;

public class DayEventProcessSelector<I, O> : IProcessSelector<I> {

    class EventEntry {
        public int day;
        public bool skipOriginal;
        public IProcessGetter getter;

        public EventEntry(int day, bool skipOriginal, IProcessGetter getter) {
            this.day = day;
            this.skipOriginal = skipOriginal;
            this.getter = getter;
        }
 
    }

    List<EventEntry> events = new List<EventEntry>();

    public DayEventProcessSelector() {    }

    public void AddEvent(int day, bool skipOriginal, IProcessGetter getter) {
        events.Add(new EventEntry(day, skipOriginal, getter));
    }

    public ProcessFactory<I> SelectProcess(ProcessFactory<I> defaultFactory, I inputArgs) {
        int day = PlayerData.Instance.Time.Day;
        foreach (var e in events) {
            if (e.day == day) {
                if (e.skipOriginal) {
                    return new InsertProcessFactory<I, O>(e.getter, null);
                } else {
                    return new InsertProcessFactory<I, O>(e.getter, defaultFactory);
                }
            }
        }
        return null;
    }

}
