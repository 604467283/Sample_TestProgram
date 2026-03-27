using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;
using static Liteon_TestProgram.Base.Class_Variable;


namespace Liteon_TestProgram.CaseProject
{
    internal class ZLogDataCollection_ICT_V1: ZLogDataCollection_Litepoint_V1
    {
        public ZLogDataCollection_ICT_V1(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
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
    }
}
