using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

[ResourcePath("UI/Indicator/Clock")]
public class ClockUI : UIPanel, ITemporaryUI<TimeSpan,object> {

    public Text timeText;

    public event System.EventHandler<EventArgs<object>> Complete;

    TimeSpan lastTime = new TimeSpan();
    bool initialized = false;

    public void Initialize(TimeSpan time)
    {
        var parent = GameObject.FindGameObjectWithTag(TagLibrary.SubStatus);
        if (parent) {
            transform.SetParent(parent.transform, false);
        }

        if (!initialized) {
            timeText.text = "12:00 am";
            initialized = true;
        }

        StartCoroutine(ChangeToTime(time));
    }

    IEnumerator ChangeToTime(TimeSpan time) {
        var newTime = time;
        if (time < lastTime) {
            time += new TimeSpan(24, 0, 0);
        }

        for (float t = 0; t < 1f; t += Time.deltaTime) {
            var thisTime = new TimeSpan(lastTime.Ticks + (long)((newTime.Ticks - lastTime.Ticks) * t));
            timeText.text = GetStringForTimeSpan(thisTime);
            yield return null;
        }
        lastTime = newTime;
        timeText.text = GetStringForTimeSpan(newTime);
    }

    string GetStringForTimeSpan(TimeSpan time) {
        var s = "";
        if (time.TotalHours > 24) {
            int removedHours = ((int)(time.TotalHours / 24)) * 24;
            time -= new TimeSpan(removedHours, 0, 0);
        }

        if (time.Hours <= 12) {
            int h = time.Hours;
            if (h == 0) {
                h = 12;
            }
            s = string.Format("{0}:{1:D2} am", h, time.Minutes);
        } else {
            s = string.Format("{0}:{1:D2} pm", time.Hours - 12, time.Minutes);
        }
        return s;
    }

}