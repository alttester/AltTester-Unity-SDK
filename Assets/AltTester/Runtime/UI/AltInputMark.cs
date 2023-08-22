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

using UnityEngine.UI;

public class AltInputMark : UnityEngine.MonoBehaviour
{
    public UnityEngine.CanvasGroup CanvasGroup;
    public UnityEngine.AnimationCurve VisibilityCurve;

    public int Id { get; private set; }
    public UnityEngine.Transform Transform
    {
        get
        {
            if (_transform == null)
                _transform = GetComponent<UnityEngine.Transform>();
            return _transform;
        }
    }
    private UnityEngine.Transform _transform;

    private System.Action<AltInputMark> _onFinished;
    private float _time;
    private float _currentTime;

    private void Awake()
    {
        Id = GetInstanceID();

        if (CanvasGroup == null)
            CanvasGroup = GetComponentInChildren<UnityEngine.CanvasGroup>();

        CanvasGroup.alpha = 0;
    }

    public void Init(float time, System.Action<AltInputMark> onFinished, UnityEngine.Color color = default)
    {
        _time = time;
        _onFinished = onFinished;
        GetComponentInChildren<Image>().color = color == default ? UnityEngine.Color.red : color;
    }

    public void Show(UnityEngine.Vector2 pos)
    {
        Transform.localPosition = pos;
        CanvasGroup.alpha = 1;
        _currentTime = 0;
    }

    private void Update()
    {
        CanvasGroup.alpha = VisibilityCurve.Evaluate(_currentTime);

        _currentTime += UnityEngine.Time.unscaledDeltaTime / _time;
        if (_currentTime < _time)
            return;

        Finish();
    }

    private void Finish()
    {
        if (_onFinished != null)
            _onFinished.Invoke(this);
        else
            gameObject.SetActive(false);
    }
}
