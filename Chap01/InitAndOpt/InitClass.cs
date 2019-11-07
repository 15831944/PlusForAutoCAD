using System;


using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

// 用于定义InitClass类为程序的入口点，而让AutoCAD只会执行OptimizeClass类中定义的命令
[assembly: ExtensionApplication(typeof(InitAndOpt.InitClass))]
[assembly: CommandClass(typeof(InitAndOpt.OptimizeClass))]

namespace InitAndOpt
{
    
    public class InitClass : IExtensionApplication
    {
        public void Initialize()
        {
            // 获取当前的命令行对象
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            // 在AutoCAD命令行上显示一些信息
            ed.WriteMessage("程序开始初始化...");
        }

        public void Terminate()
        {
            // 在Visual Studio 的输出窗口上显示程序结束的信息
            System.Diagnostics.Debug.WriteLine("程序结束，你可以做一些清理工作，如关闭AutoCAD文档");
        }

        [CommandMethod("InitCommand")]
        public void InitCommand()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            ed.WriteMessage("Test");
        }
    }
}
