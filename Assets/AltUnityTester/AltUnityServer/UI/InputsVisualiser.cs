using System.Collections.Generic;

public class InputsVisualiser : UnityEngine.MonoBehaviour
{
    public float VisibleTime = 1;
    [UnityEngine.Space]
    [UnityEngine.SerializeField] private InputMark Template;

    private readonly List<InputMark> _pool = new List<InputMark>();
    private readonly Dictionary<int, InputMark> _continuously = new Dictionary<int, InputMark>();
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

        InputMark mark; 
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

    private InputMark GetMark()
    {
        InputMark inputMark;

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

    private void PutMark(InputMark mark)
    {
        if (_continuously.ContainsKey(mark.Id))
            _continuously.Remove(mark.Id);
        
        mark.gameObject.SetActive(false);
        _pool.Add(mark);
    }
}