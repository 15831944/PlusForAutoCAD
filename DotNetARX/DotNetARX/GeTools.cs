using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;

namespace DotNetARX
{
    /// <summary>
    /// 用于计算从第一点到第二点所确定的矢量与x轴正方向的夹角
    /// </summary>
    public static class GeTools
    {
        /// <summary>
        /// 用于计算从第一点到第二点所确定的矢量与x轴正方向的夹角
        /// </summary>
        /// <param name="pt1">起始点</param>
        /// <param name="pt2">终止点</param>
        /// <returns></returns>
        public static double AngleFromXAxis(this Point3d pt1, Point3d pt2)
        {
            // 构建一个从第一点到第二点所确定的矢量
            Vector2d vector = new Vector2d(pt1.X - pt2.X, pt1.Y - pt2.Y);
            // 返回该矢量和x轴正半轴的角度（弧度）
            return vector.Angle;
        }

        /// <summary>
        /// 用于将角度值转换为弧度值
        /// </summary>
        /// <param name="angle">角度值</param>
        /// <returns></returns>
        public static double DegreeToRadian(this double angle) => angle* (Math.PI / 180.0);
    }

    
    
}
