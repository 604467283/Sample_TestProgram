using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.TestFunc.Litepoint.LogDataCollection_V1;
using static Liteon_TestProgram.Base.Class_Variable;


namespace Liteon_TestProgram.CaseProject
{
    internal class ZLogDataCollection_Litepoint_V1 : CaseCodeBase
    {



        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================
        //========================↑↑↑自定义变量↑↑↑================================



        public string _CaseProjectName { get; set; }
        public TestForm _testForm;


        public ZLogDataCollection_Litepoint_V1(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
          : base(mf, tf, pc, str_CaseProject)
        {
            struct_TestVariable.bNeedCreateSFCFile = true;
            _CaseProjectName = str_CaseProjectName = str_CaseProject == GetType().Name ? str_CaseProject : GetType().Name;
            _testForm = tf;

            Type sfcFileType = typeof(SfcFile); // 如果你已经有了SfcFile的实例，可以用instance.GetType()        
            struct_TestVariable.str_LogDataCollectionType = sfcFileType.FullName; // 使用反射获取类型的完整名称（包括命名空间）

            List<string> list_str = new List<string>();
            list_str = CreateTestStringList();
            string[] array_str = list_str.ToArray();
            ShowTipsMessage(str_CaseProjectName + "测试提要", array_str);
        }


        /// <summary>
        /// 测试须知填写处,会显示到测试UI上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateTestStringList()
        {
            List<string> list_str = new List<string>();
            list_str.Add("1.此机种不用于测试，只用于作小工具收集Log数据\r\n");
            return list_str;
        }


        public override void Func_TestMac()
        {
        }


        public override bool Func_TestPre()
        {
            bool bResult = true;

            return bResult;
        }

        public override bool Func_TestInit()
        {
            bool bInitResult = true;

            return bInitResult;
        }

        public override bool Func_TestFlow()
        {

            bool bTestResult = false;
           

            return bTestResult;
        }

        public override bool Func_TestEnd(bool bTestResult)
        {
            bool bEndResult = false;

           


            return bEndResult;
        }


        public override void Func_ResourceRelease()
        {

        }




        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================
        //========================↓↓↓自定义方法↓↓↓================================







    }
}
