using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;

namespace InitAndOpt
{
    class OptimizeClass
    {
        [CommandMethod("OptCommand")]
        public void OptCommand()
        {
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            string fileName = "C:\\Users\\Administrator\\Desktop\\Chap01\\Hello\\bin\\Debug\\Hello.dll";
            try
            {
                ExtensionLoader.Load(fileName);
                // 在命令行上显示信息，用于提示用户Hello.dll程序集已经被载入
                ed.WriteMessage("\n" + fileName + "被载入，请输入Hello进行测试！ ");
            }
            catch (System.Exception ex) // 捕捉程序异常
            {
                ed.WriteMessage(ex.Message); // 显示异常信息
            }
            finally
            {
                ed.WriteMessage("\nfinally 语句：程序执行完毕！ ");
            }
        }

        [CommandMethod("ChangeColor")]
        public void ChangeColor()
        {
            // 获取当前图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            // 获取命令行对象
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            try
            {
                // 提示用户选择对象
                ObjectId id = ed.GetEntity("\n 请选择要改变颜色的对象").ObjectId;
                // 开启事务处理
                using (Transaction trans = db.TransactionManager.StartTransaction())
                {
                    // 以写的方式打开对象
                    Entity ent = (Entity) trans.GetObject(id, OpenMode.ForWrite);
                    ent.ColorIndex = 300; // 为测试异常，设置对象为不合法的颜色
                    trans.Commit();  // 提交事务处理，颜色更改完成
                }
            }
            catch (Autodesk.AutoCAD.Runtime.Exception ex)  // 捕获异常
            {
                // 对不同的异常分别进行处理，ErrorStatus用来表示与当前异常相关的错误状态代码
                switch (ex.ErrorStatus)
                {
                    case ErrorStatus.InvalidIndex:  // 输入错误的颜色值
                        ed.WriteMessage("\n输入的颜色值有误！");
                        break;
                    case ErrorStatus.InvalidObjectId:  // 未选择对象
                        ed.WriteMessage("\n 未选择对象！");
                        break;
                    default: // 其他异常
                        ed.WriteMessage(ex.ErrorStatus.ToString());
                        break;
                }
            }
        }
    }
}
