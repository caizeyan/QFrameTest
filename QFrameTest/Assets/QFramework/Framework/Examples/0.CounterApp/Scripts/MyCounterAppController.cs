using System;
using System.Collections;
using System.Collections.Generic;
using QFramework;
using UnityEngine;
using UnityEngine.UI;


public class MyCounterAppModel: AbstractModel
{
    public BindableProperty<int> Count = new BindableProperty<int>();


    protected override void OnInit()
    {
        var storage = this.GetUtility<MyStorage>();
        Count.SetValueWithoutEvent(storage.LoadInt(nameof(Count)));
        MyCounterApp.Interface.RegisterEvent<MyCountChangeEvent>(e =>
        {
            storage.SaveInt(nameof(Count), Count);
        });
    }
}

public class MyStorage : IUtility
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

public class MyAchievementSystem: AbstractSystem
{
    protected override void OnInit()
    {
        var Count = this.GetModel<MyCounterAppModel>().Count;
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
        this.RegisterSystem(new MyAchievementSystem());
        this.RegisterModel(new MyCounterAppModel());
        this.RegisterUtility(new MyStorage());
    }
}

public class MyIncreaseCountCommand:AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MyCounterAppModel>().Count.Value++;
        this.SendEvent<MyCountChangeEvent>();
    }
}

public class MyDecreaseCountCommand:AbstractCommand
{
    protected override void OnExecute()
    {
        this.GetModel<MyCounterAppModel>().Count.Value--;
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

    private MyCounterAppModel model;
    // Start is called before the first frame update
    void Start()
    {
        model = this.GetModel<MyCounterAppModel>();
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
