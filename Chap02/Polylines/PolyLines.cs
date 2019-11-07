using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using DotNetARX;

namespace Polylines
{
    public class PolyLines
    {
        [CommandMethod("AddPolyline")]
        public void AddPolyline()
        {
            Database db = HostApplicationServices.WorkingDatabase;
            using(Transaction trans = db.TransactionManager.StartTransaction())
            {
                Point2d startPoint = Point2d.Origin;
                Point2d endPoint = new Point2d(100, 100);
                Point2d pt = new Point2d(60, 70);
                Point2d center = new Point2d(50, 50);
                // 创建直线
                Polyline pline = new Polyline();
                pline.CreatPolyline(startPoint, endPoint);
                // 创建矩形
                Polyline rectangle = new Polyline();
                rectangle.CreatRectangle(pt, endPoint);
                // 创建正六边形
                Polyline polygon = new Polyline();
                polygon.CreatePolygon(Point2d.Origin, 6, 30);
                // 创建半径为30的园
                Polyline circle = new Polyline();
                circle.CreatePolyCircle(center, 30);
                // 创建圆弧，起点角度为45度，终点角度为225度
                Polyline arc = new Polyline();
                double startAngle = 45;
                double endAngle = 225;
                arc.CreatePolyArc(center, 50, startAngle.DegreeToRadian(), endAngle.DegreeToRadian());
                // 添加对象到模型空间
                db.AddToModelSpace(pline);
                db.AddToModelSpace(rectangle);
                db.AddToModelSpace(polygon);
                db.AddToModelSpace(circle);
                db.AddToModelSpace(arc);
                trans.Commit();
            }
        }
    }
}
