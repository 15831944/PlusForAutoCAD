using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;
using System;

namespace Lines
{
    public class EntEdit
    {
        [CommandMethod("EditLine")]

        public static void EditLine()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            //事务处理器管理
            Autodesk.AutoCAD.DatabaseServices.TransactionManager tm = db.TransactionManager;
            //开始第一个事务处理tr1
            using (Transaction tr1 = tm.StartTransaction())
            {
                Point3d ptStart = Point3d.Origin;
                Point3d ptEnd = new Point3d(100, 0, 0);
                Line line1 = new Line(ptStart, ptEnd);
                ObjectId Id1 = db.AddToModelSpace(line1);

                //开始第二个事务处理tr2
                using (Transaction tr2 = tm.StartTransaction())
                {
                    line1.UpgradeOpen();  //切换line1的状态为可写
                    line1.ColorIndex = 1;  //设置直线的颜色为红色
                    ObjectId Id2 = Id1.Copy(ptStart, ptEnd);  //复制直线
                    Id2.Rotate(ptEnd, Math.PI / 2);  //以直线的起点旋转90度

                    //开始第三个事务处理tr3
                    using (Transaction tr3 = tm.StartTransaction())
                    {
                        Line line2 = (Line)tr3.GetObject(Id2, OpenMode.ForWrite);
                        line2.ColorIndex = 3; // 设置直线的颜色为绿色
                        tr3.Abort(); //撤销第三个事务处理tr3，line2的颜色不变
                    }
                    // 提交第二个事务处理tr2，line1颜色变为红色，复制line1并旋转90度
                    tr2.Commit();
                }
                //提交第一个事务处理tr1，添加line1到数据库中
                tr1.Commit(); 
            }
        }
    }
}
