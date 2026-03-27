using Liteon_TestProgram.Base;
using Liteon_TestProgram.Forms;
using Liteon_TestProgram.Utilities;
using Liteon_TestProgram.Utilities.IOHelpers.Extensions;
using static Liteon_TestProgram.Base.Class_Variable;
using Liteon_TestProgram.TestFunc.ICT.LogDataCollection_V1;


namespace Liteon_TestProgram.CaseProject
{
    internal class WCBN816A_BA_ICT : Base_ICT
    {

        protected override string str_ICTFlowMark { get; set; } = "008076";


        public WCBN816A_BA_ICT(MainForm mf, TestForm tf, ProjectChangeForm pc, string str_CaseProject)
          : base(mf, tf, pc, str_CaseProject)
        {
            struct_TestVariable.bNeedCreateSFCFile = true;
            _CaseProjectName = str_CaseProjectName = str_CaseProject == GetType().Name ? str_CaseProject : GetType().Name;
            _testForm = tf;

            Type sfcFileType = typeof(SfcFile); // 如果你已经有了SfcFile的实例，可以用instance.GetType()        
            struct_TestVariable.str_LogDataCollectionType = sfcFileType.FullName; // 使用反射获取类型的完整名称（包括命名空间）

            List<string> tipslist_str = new List<string>();
            tipslist_str = CreateTestStringList();
            string[] array_tipsstr = tipslist_str.ToArray();
            ShowTipsMessage(str_CaseProjectName + "测试提要", array_tipsstr);

            List<string> changelist_str = new List<string>();
            changelist_str = CreateChangeStringList();
            string[] array_changestr = changelist_str.ToArray();
            ShowChangeListMessage($"  <{str_CaseProjectName} 变更记录>", array_changestr);
        }


        /// <summary>
        /// 机种变更记录,会显示到UI标签页上
        /// </summary>
        /// <returns></returns>
        public override List<string> CreateChangeStringList()
        {
            List<string> list_str = new List<string>();

            #region V0.0.0.1

            list_str.Add("V0.0.0.1");
            list_str.Add("1.初版");
            list_str.Add("2.添加了自动导入Flow的功能");

            #endregion

            return list_str;
        }


        public override void Func_TestMac()
        {
            macBD_Relation = MacBD_Relation.PlusOne;
            if (struct_NormalINI.stru_b_DebugMode)
            {
                struct_TestVariable.iMAC_OriginalLength = 12;
                struct_TestVariable.iMAC_ExtractStartPosition = 0;
                struct_TestVariable.iMACLength = 12;

                struct_TestVariable.iBD_OriginalLength = 12;
                struct_TestVariable.iBD_ExtractStartPosition = 0;
                struct_TestVariable.iBDLength = 12;
            }
            else
            {
                struct_TestVariable.iMACLength = 12;
                struct_TestVariable.iBDLength = 12;
            }
        }



    }
}
