using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;

public static class UIUtil {

    const float UIMoveSpeed = 2000f;

    public static void GenerateChildren<T>(IEnumerable<T> collection, Transform parent, Func<T, GameObject> getChildInstance) {
        GenerateChildren<T>(collection, null, parent, getChildInstance);
    }

    public static void GenerateChildren<T>(IEnumerable<T> collection, List<GameObject> instances, Transform parent, Func<T, GameObject> getChildInstance)
    {
        if (instances != null) {
            foreach (var i in instances) {
                GameObject.Destroy(i);
            }
            instances.Clear();
        }

        foreach (var item in collection)
        {
            var instance = getChildInstance(item);
            instance.transform.SetParent(parent, false);
            if (instances != null) {
                instances.Add(instance);
            }
        }
    }

    public static bool MouseOverUI() {
        if (EventSystem.current == null) {
            return false;
        }

        var raycastResults = new List<RaycastResult>();
        var eventData = new PointerEventData(EventSystem.current);
        eventData.position = Input.mousePosition;
        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }

    public static void FadeIn(this UIMonoBehaviour ui)
    {
        ui.canvasGroup.alpha = Mathf.MoveTowards(ui.canvasGroup.alpha, 1f, 5f * Time.deltaTime);
    }

    public static void FadeOut(this UIMonoBehaviour ui)
    {
        ui.canvasGroup.alpha = Mathf.MoveTowards(ui.canvasGroup.alpha, 0, Time.deltaTime);
    }

    public static IEnumerator FadeInAndOutRoutine(CanvasGroup canvasGroup, float duration0 = 0.5f, float duration1 = 3f, float duration2 = 0.5f) {
        canvasGroup.alpha = 0;
        yield return CoroutineManager.Instance.StartCoroutine(FadeInRoutine(canvasGroup, duration0));

        yield return new WaitForSeconds(duration1);

        yield return CoroutineManager.Instance.StartCoroutine(FadeOutRoutine(canvasGroup, duration2));
    }

    public static IEnumerator FadeInRoutine(CanvasGroup canvasGroup, float duration = 0.5f) {
        for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
            if (!canvasGroup) {
                yield break;
            }
            
            canvasGroup.alpha = t;

            yield return null;
        }
        if (canvasGroup) {
            canvasGroup.alpha = 1f;
        }
    }

    public static IEnumerator FadeOutRoutine(CanvasGroup canvasGroup, float duration = 0.5f) {
        for (float t = 0; t < 1f; t += Time.deltaTime / duration) {
            if (!canvasGroup) {
                yield break;
            }
            
            canvasGroup.alpha = 1f - t;

            yield return null;
        }
        canvasGroup.alpha = 0;
    }

    static Vector3[] corners = new Vector3[4];
    public static Vector2 GetCenter(this RectTransform t) {
        //Debug.Log(t.position + "; " + t.rect.center);
        t.GetWorldCorners(corners);
        return (Vector2)(corners[0] + corners[2]) * 0.5f;
    }

    public static Rect GetScreenRect(this RectTransform t) {
        t.GetWorldCorners(corners);
        return new Rect((Vector2)corners[0], (Vector2)corners[2]);
    }

    public static Coroutine CloseOnPlayerMove(this ITemporaryUI ui) {
        return (ui as MonoBehaviour).StartCoroutine(CloseOnPlayerMoveRoutine(ui));
    }

    public static IEnumerator CloseOnPlayerMoveRoutine(this ITemporaryUI ui) {
        yield return null;
        var player = PlayerManager.Instance.PlayerGameObject.transform;
        var originalPos = player.position;
        //  Debug.Log("orig pos: " + originalPos);
        while (Vector3.Distance(player.position, originalPos) < 4f) {
            yield return null;
        }
        //Debug.Log("closing");
        ui.Close();
    }

    public static void AnimateMoveItemFromList(RectTransform sourceItem, RectTransform targetParent) {
        CoroutineManager.Instance.StartCoroutine(AnimateMoveItemFromListCoroutine(sourceItem, targetParent));
    }

    public static IEnumerator AnimateMoveItemFromListCoroutine(RectTransform sourceItem, RectTransform targetParent) {
        var empty = new GameObject("_empty").AddComponent<RectTransform>();
        var layout = empty.gameObject.AddComponent<LayoutElement>();
        layout.preferredWidth = sourceItem.rect.width;
        layout.preferredHeight = sourceItem.rect.height;
        empty.SetParent(targetParent, false);

        yield return null;

        MainCanvas.main.Add(sourceItem);

        while (Vector2.Distance(sourceItem.position, empty.position) > 1f) {
            sourceItem.position = Vector2.MoveTowards(sourceItem.position, empty.position, UIMoveSpeed * Time.deltaTime);

            yield return null;
        }

        sourceItem.SetParent(targetParent.transform, false);
        sourceItem.SetSiblingIndex(empty.GetSiblingIndex());
        GameObject.Destroy(empty.gameObject);
    }

}
