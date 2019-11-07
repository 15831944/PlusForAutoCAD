using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace DotNetARX
{
    /// <summary>
    /// 添加三点创建圆弧
    /// </summary>
    public static class ArcTools
    {
        /// <summary>
        /// 根据圆弧上三点创建圆弧
        /// </summary>
        /// <param name="arc">圆弧实体</param>
        /// <param name="startPoint">圆弧的起始点</param>
        /// <param name="pointOnArc">圆弧上的点</param>
        /// <param name="endPoint">圆弧的终止点</param>
        public static void CreatArc(this Arc arc, Point3d startPoint, Point3d pointOnArc, Point3d endPoint)
        {
            // 创建一个几何类的圆弧对象
            CircularArc3d getArc = new CircularArc3d(startPoint, pointOnArc, endPoint);
            // 将几何类圆弧对象的圆心和半径赋值给圆弧
            Point3d centerPoint = getArc.Center;
            arc.Center = centerPoint;
            arc.Radius = getArc.Radius;
            // 计算起始和终止的角度
            arc.StartAngle = startPoint.AngleFromXAxis(centerPoint);
            arc.EndAngle = endPoint.AngleFromXAxis(centerPoint);
        }
    }


}
