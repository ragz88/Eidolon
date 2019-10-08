using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSyncValue : AudioSyncer
{

    public float beatValue = 1;
    public float restValue = 0;
    /*[HideInInspector]*/ public float currentVal = 0;

    private IEnumerator MoveToScale(float _target)
    {
        float _curr = currentVal;
        float _initial = _curr;
        float _timer = 0;

        while (_curr < _target)
        {
            _curr = Mathf.Lerp(_initial, _target, _timer / timeToBeat);
            _timer += Time.deltaTime;

            currentVal = _curr;

            yield return null;
        }

        m_isBeat = false;
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (m_isBeat) return;

        currentVal = Mathf.Lerp(currentVal, restValue, restSmoothTime * Time.deltaTime);
    }

    public override void OnBeat()
    {
        base.OnBeat();

        StopCoroutine("MoveToScale");
        StartCoroutine("MoveToScale", beatValue);
    }

}