

using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utils
{

    public static string ConvertToKMValues(float val, string format = "F0")
    {
        if (val >= 1000 && val < 1000000)//bellow 1M
        {
            return $"{(val / 1000f):F2} K";
        }
        else if (val >= 1000000)//after 1m
        {
            return $"{(val / 1000000f):F2} M";
        }
        return val.ToString(format);
    }


    public static void SetActiveInSuccession<T>(MonoBehaviour obj, float scaleDuration, Vector3 scale, T[] array, int count, float delayBeforWholeActivation = 0f, string soundName = null) where T : Component
    {
        obj.StartCoroutine(COR_Activate());

        IEnumerator COR_Activate()
        {
            yield return new WaitForSeconds(delayBeforWholeActivation);

            for (int i = 0; i < count; i++)
            {
                T t = array[i];
                t.gameObject.SetActive(true);

                t.transform.DOPunchScale(scale, scaleDuration).SetEase(Ease.OutSine);

                //if (soundName != null)
                //{
                //    AudioManager.Instance.Play(soundName, true);
                //}

                yield return new WaitForSeconds(scaleDuration);
            }
        }

    }

    public static void Delay(MonoBehaviour obj, float delay, Action action)
    {
        obj.StartCoroutine(COR_Delay());
        IEnumerator COR_Delay()
        {
            yield return new WaitForSeconds(delay);

            if (obj.gameObject.activeInHierarchy)
            {
                action();
            }
        }
    }

    public static GameObject CreatePrimitive(Vector3 position, PrimitiveType type = PrimitiveType.Sphere, float scale = .1f)
    {
        var test = GameObject.CreatePrimitive(type);

        test.transform.position = position;
        test.transform.localScale = Vector3.one * scale;

        return test;
    }

    public static void Until(MonoBehaviour obj, Func<bool> predicate, Action action)
    {
        obj.StartCoroutine(COR_Until());
        IEnumerator COR_Until()
        {
            yield return new WaitUntil(() =>
            {
                if (!obj.gameObject.activeInHierarchy)
                {
                    return true; //Exits early
                }
                return predicate();
            });

            if (obj.gameObject.activeInHierarchy)
            {
                action();
            }
        }
    }

    public static void IntervalInUpdate(ref float interval, float intervalMax, Action action) // Execute in Update only
    {
        if (interval >= intervalMax)
        {
            interval = 0;
            action();
        }
        else
        {
            interval += Time.deltaTime;
        }

    }

    public static void AddEventTriggerListener(EventTrigger trigger,
                                           EventTriggerType eventType,
                                           System.Action<BaseEventData> callback)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = eventType;
        entry.callback = new EventTrigger.TriggerEvent();
        entry.callback.AddListener(new UnityEngine.Events.UnityAction<BaseEventData>(callback));
        trigger.triggers.Add(entry);
    }




    // This is the extension method.
    // The first parameter takes the "this" modifier
    // and specifies the type for which the method is defined.
}
public static class StringExtension
{
    public static string Bold(this string str) => "<b>" + str + "</b>";
    public static string Color(this string str, string clr) => string.Format("<color={0}>{1}</color>", clr, str);
    public static string Italic(this string str) => "<i>" + str + "</i>";
    public static string Size(this string str, int size) => string.Format("<size={0}>{1}</size>", size, str);


}
