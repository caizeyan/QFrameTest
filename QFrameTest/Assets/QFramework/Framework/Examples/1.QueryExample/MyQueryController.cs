using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QFramework.MyExample
{
    public class MyQueryController : MonoBehaviour,IController
    {
        public class StudentModel:AbstractModel
        {
            public List<string> StudentNames = new List<string>()
            {
                "张三",
                "李四",
            };
            protected override void OnInit()
            {
                
            }
        }
        
        public class TeacherModel:AbstractModel
        {
            public List<string> TeacherNames = new List<string>()
            {
                "王五",
                "赵六"
            };
            protected override void OnInit()
            {
                
            }
        }

        public class QueryApp : Architecture<QueryApp>
        {
            protected override void Init()
            {
                this.RegisterModel(new StudentModel());
                this.RegisterModel(new TeacherModel());
            }
        }
        
        public class SchoolAllPersonCountQuery : AbstractQuery<int>
        {
            protected override int OnDo()
            {
                return this.GetModel<StudentModel>().StudentNames.Count +
                       this.GetModel<TeacherModel>().TeacherNames.Count;
            }
        }

        private int allPersonCount = 0;

        private void OnGUI()
        {
            GUILayout.Label(allPersonCount.ToString());
            if (GUILayout.Button("查询学校总人数"))
            {
                allPersonCount = this.SendQuery(new SchoolAllPersonCountQuery());
            }
        }

        public IArchitecture GetArchitecture()
        {
            return QueryApp.Interface;
        }
    }    
}

