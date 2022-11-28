using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;


public interface IMyCounterAppModel : IModel
{
    BindableProperty<int> Count { get; }
}

public class MyCounterAppModel: AbstractModel,IMyCounterAppModel
{
    public BindableProperty<int> Count { get; } = new BindableProperty<int>();

    protected override void OnInit()
    {
        var storage = this.GetUtility<IMyStorage>();
        Count.SetValueWithoutEvent(storage.LoadInt(nameof(Count)));
        MyCounterApp.Interface.RegisterEvent<MyCountChangeEvent>(e =>
        {
            storage.SaveInt(nameof(Count), Count);
        });
    }
}

public interface IMyStorage : IUtility
{
    void SaveInt(string key, int value);
    int LoadInt(string key, int defaultValue = 0);
}

public class MyStorage : IMyStorage
{
    public void SaveInt(string key, int value)
    {
        PlayerPrefs.SetInt(key,value);
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        return PlayerPrefs.GetInt(key, defaultValue);
    }
    
}

public class EasySaveStorage : IMyStorage
{
    public void SaveInt(string key, int value)
    {
        //todo
    }

    public int LoadInt(string key, int defaultValue = 0)
    {
        //todo
        throw new NotImplementedException();
    }
}

public interface IMyAchievementSystem : ISystem
{
    
}

public class MyAchievementSystem: AbstractSystem,IMyAchievementSystem
{
    protected override void OnInit()
    {
        var Count = this.GetModel<IMyCounterAppModel>().Count;
        Count.Register(count =>
        {
            if (count == 10)
            {
                Debug.LogError("点击达人");
            }else if (count == 20)
            {
                Debug.LogError("点击达人");
            }else if (count == -10)
            {
                Debug.LogError("菜鸡达人");
            }
        });
    }
}


public class MyCounterApp : Architecture<MyCounterApp>
{
    protected override void Init()
    {
        this.RegisterSystem<IMyAchievementSystem>(new MyAchievementSystem());
        this.RegisterModel<IMyCounterAppModel>(new MyCounterAppModel());
        this.RegisterUtility<IMyStorage>(new MyStorage());
    }
}

public class MyIncreaseCountCommand:AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<IMyCounterAppModel>().Count.Value++;
        this.SendEvent<MyCountChangeEvent>();
    }
}

public class MyDecreaseCountCommand:AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<IMyCounterAppModel>().Count.Value--;
        this.SendEvent<MyCountChangeEvent>();
    }
}

public struct MyCountChangeEvent
{
    
}


public class MyCounterAppController : MonoBehaviour,IController
{
    private Button addButton;

    private Button subButton;

    private Text countText;

    private IMyCounterAppModel model;
    // Start is called before the first frame update
    void Start()
    {
        model = this.GetModel<IMyCounterAppModel>();
        addButton = transform.Find("BtnAdd").GetComponent<Button>();
        subButton = transform.Find("BtnSub").GetComponent<Button>();
        countText = transform.Find("CountText").GetComponent<Text>();
        
        addButton.onClick.AddListener(() =>
        {
            this.SendCommand<MyIncreaseCountCommand>();
        });
        
        subButton.onClick.AddListener((() =>
        {
            this.SendCommand<MyDecreaseCountCommand>();
        }));
        
        UpdateView();
        this.RegisterEvent<MyCountChangeEvent>((e) => { UpdateView(); }).UnRegisterWhenGameObjectDestroyed(gameObject);
    }

    void UpdateView()
    {
        countText.text = model.Count.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public IArchitecture GetArchitecture()
    {
        return MyCounterApp.Interface;
    }

    private void OnDestroy()
    {
        model = null;
    }
}
