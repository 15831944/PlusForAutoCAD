using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;

namespace DotNetARX
{
    /// <summary>
    /// 根据圆周上任意三点绘制圆形
    /// </summary>
    public static class CircleTools
    {

        /// <summary>
        /// 使用Geometry命名空间中的几何类CircularArc3d类来依据圆周上任意三点创建园
        /// </summary>
        /// <param name="circle">圆形实体</param>
        /// <param name="pt1">圆上第一个点</param>
        /// <param name="pt2">圆上第二个点</param>
        /// <param name="pt3">圆上第三个点</param>
        /// <returns></returns>
        public static bool CreateCircle(this Circle circle, Point3d pt1, Point3d pt2, Point3d pt3)
        {
            //先判断三点是否共线，得到pt1指向pt2、pt3点的矢量
            Vector3d va = pt1.GetVectorTo(pt2);
            Vector3d vb = pt1.GetVectorTo(pt3);
            //如果两矢量夹角为0或180度（π弧度），则三点共线
            if(va.GetAngleTo(vb) == 0 | va.GetAngleTo(vb) == Math.PI)
            {
                return false;
            }
            else
            {
                //创建一个几何类的圆弧对象，但CircularArc3d类能够创建一个几何类对象，但该对象只能用来计算，不能在图形窗口中显示。
                CircularArc3d getArc = new CircularArc3d(pt1, pt2, pt3);
                //将圆弧对象的圆心和半径赋值给园
                circle.Center = getArc.Center;
                circle.Radius = getArc.Radius;
                return true;
            }
        }
    }
}
