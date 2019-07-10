using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecalageCartes : MonoBehaviour
{
    Animation anim;
    public AnimationCurve curve;
    public AnimationCurve curve2;
    RectTransform rt;
    public float wantedX;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        anim = GetComponent<Animation>();
    }

    public void EcarterCarte(float x, float y)
    {
        curve = AnimationCurve.Linear(0.0F, rt.anchoredPosition.x, 0.2F, x);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        curve = AnimationCurve.Linear(0.0F, rt.anchoredPosition.y, 0.2F, y);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        anim.AddClip(clip, "test");
        anim.Play("test");
    }

    public void RetourNormale(float x, float y)
    {
        curve = AnimationCurve.Linear(0.0F, rt.anchoredPosition.x, 0.2F, x);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve);
        curve = AnimationCurve.Linear(0.0F, rt.anchoredPosition.y, 0.2F, y);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve);
        anim.AddClip(clip, "test");
        anim.Play("test");
    }

    public void Highlight(float xGiven)
    {
        curve2 = AnimationCurve.Linear(0.0F, rt.anchoredPosition.y, 0.2F, -350);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve2);
        curve2 = AnimationCurve.Linear(0.0F, rt.anchoredPosition.x, 0.2F, xGiven);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve2);
        anim.AddClip(clip, "test");
        anim.Play("test");
    }

    public void RetourHighlight(float x, float y)
    {
        curve2 = AnimationCurve.Linear(0.0F, rt.anchoredPosition.y, 0.2F, y);
        AnimationClip clip = new AnimationClip();
        clip.legacy = true;
        clip.SetCurve("", typeof(Transform), "localPosition.y", curve2);
        curve2 = AnimationCurve.Linear(0.0F, rt.anchoredPosition.x, 0.2F, x);
        clip.SetCurve("", typeof(Transform), "localPosition.x", curve2);
        anim.AddClip(clip, "test");
        anim.Play("test");
    }
}
