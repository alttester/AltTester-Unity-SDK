using System.Collections.Generic;

public class AltUnityInputsVisualiser : UnityEngine.MonoBehaviour
{
    public float VisibleTime = 1;
    [UnityEngine.Space]
    [UnityEngine.SerializeField] private AltUnityInputMark Template;

    private readonly List<AltUnityInputMark> _pool = new List<AltUnityInputMark>();
    private readonly Dictionary<int, AltUnityInputMark> _continuously = new Dictionary<int, AltUnityInputMark>();
    private UnityEngine.Transform _transform;
  
    private void Awake()
    {
        _transform = GetComponent<UnityEngine.Transform>();
    }

    public void ShowClick(UnityEngine.Vector2 pos)
    {
        GetMark().Show(pos);
    }
    
    public int ShowContinuousInput(UnityEngine.Vector2 pos, int id)
    {
        var currentId = id;

        AltUnityInputMark mark; 
        if (_continuously.ContainsKey(currentId))
            mark = _continuously[currentId];
        else
        {
            mark = GetMark();
            currentId = mark.Id;
            _continuously[currentId] = mark;
        }
        
        mark.Show(pos);

        return currentId;
    }

    private AltUnityInputMark GetMark()
    {
        AltUnityInputMark inputMark;

        if (_pool.Count > 0)
        {
            inputMark = _pool[0];
            inputMark.gameObject.SetActive(true);
            _pool.Remove(inputMark);
        }
        else
        {
            inputMark = Instantiate(Template, _transform);
            inputMark.Init(VisibleTime, PutMark);
        }

        return inputMark;
    }

    private void PutMark(AltUnityInputMark mark)
    {
        if (_continuously.ContainsKey(mark.Id))
            _continuously.Remove(mark.Id);
        
        mark.gameObject.SetActive(false);
        _pool.Add(mark);
    }
}