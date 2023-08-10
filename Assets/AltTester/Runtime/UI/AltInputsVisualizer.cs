/*
    Copyright(C) 2023 Altom Consulting

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program. If not, see <https://www.gnu.org/licenses/>.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltInputsVisualizer : UnityEngine.MonoBehaviour
{
    public float VisibleTime = 1;
    [UnityEngine.Space]
    [UnityEngine.SerializeField] public AltInputMark Template = null;

    private readonly List<AltInputMark> _pool = new List<AltInputMark>();
    private readonly Dictionary<int, AltInputMark> _continuously = new Dictionary<int, AltInputMark>();
    private UnityEngine.Transform _transform;
    private float currentRatio;
    private float initialRatio = 1;
    public float growthBound = 2f;
    public float approachSpeed = 0.02f;

    private void Awake()
    {
        _transform = GetComponent<UnityEngine.Transform>();
    }

    private IEnumerator VisualizerPulse(AltInputMark mark)
    {
        currentRatio = initialRatio;
        while (this.currentRatio != this.growthBound)
        {
            currentRatio = Mathf.MoveTowards(currentRatio, growthBound, approachSpeed);
            mark.transform.localScale = Vector2.one * currentRatio;
            yield return new WaitForEndOfFrame();
        }
        currentRatio = initialRatio;
        mark.transform.localScale = Vector2.one * currentRatio;
    }

    public void ShowClick(UnityEngine.Vector2 pos, UnityEngine.Color color)
    {
        AltInputMark mark = GetMark(color);
        StartCoroutine(VisualizerPulse(mark));
        mark.Show(pos);
    }

    public int ShowContinuousInput(UnityEngine.Vector2 pos, int id, UnityEngine.Color color)
    {
        var currentId = id;

        AltInputMark mark;
        if (_continuously.ContainsKey(currentId))
            mark = _continuously[currentId];
        else
        {
            mark = GetMark(color);
            currentId = mark.Id;
            _continuously[currentId] = mark;
        }

        mark.Show(pos);

        return currentId;
    }

    private AltInputMark GetMark(UnityEngine.Color color)
    {
        AltInputMark inputMark;

        if (_pool.Count > 0)
        {
            inputMark = _pool[0];
            inputMark.gameObject.SetActive(true);
            _pool.Remove(inputMark);
        }
        else
        {
            inputMark = Instantiate(Template, _transform);
            inputMark.Init(VisibleTime, PutMark, color);
        }

        return inputMark;
    }

    private void PutMark(AltInputMark mark)
    {
        if (_continuously.ContainsKey(mark.Id))
            _continuously.Remove(mark.Id);

        mark.gameObject.SetActive(false);
        _pool.Add(mark);
    }
}
