using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;

namespace Lines
{
    public class Lines
    {
        [CommandMethod("FirstLine")]
        public static void FirstLine()
        {
            // 获取当前活动图形数据库
            Database db = HostApplicationServices.WorkingDatabase;
            // 直线起点
            Point3d startPoint = new Point3d(0, 100, 0);
            // 直线终点
            Point3d endPoint = new Point3d(100, 100, 0);
            // 新建一个直线对象
            Line line = new Line(startPoint, endPoint);
            // 定义一个指向当前数据库的事务处理，以添加直线
            using (Transaction trans = db.TransactionManager.StartTransaction())
            {
                #region 这里分别使用了 as 和 is 进行类型转换，注意两者的区别和使用
                // 以读方式打开块表
                DBObject obj1 = trans.GetObject(db.BlockTableId, OpenMode.ForRead);
                BlockTable bt = obj1 as BlockTable;
                if (bt != null)
                {
                    DBObject obj2 = trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);
                    BlockTableRecord btr;
                    if (obj2 is BlockTableRecord)
                    {
                        btr = (BlockTableRecord)obj2;
                        // 将图形对象的信息添加到块表记录中，并返回ObjectId对象
                        btr.AppendEntity(line);
                        // 把对象添加到事务处理中
                        trans.AddNewlyCreatedDBObject(line, true);
                        // 提交事务处理
                        trans.Commit();
                    }
                }
                #endregion
            }
        }

            [CommandMethod("SecondLine")]
            public static void SecondLine()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            Point3d startPoint = new Point3d(0, 100, 0);
            Point3d entPoint = new Point3d(0, 200, 0);
            Line line = new Line(startPoint, entPoint);
            db.AddToModelSpace(line);
        }
    }
}
