using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;

namespace DotNetARX
{
    /// <summary>
    /// 对实体对象进行移动、复制、旋转、缩放、镜像等操作
    /// </summary>
    public static class EntTools
    {
        /// <summary>移动实体对象</summary>
        /// <param name="id">实体的ObjectId</param>
        /// <param name="sourcePt">移动的源点</param>
        /// <param name="targetPt">移动的目标点</param>
        public static void Move(this ObjectId id, Point3d sourcePt, Point3d targetPt)
        {
            // 构建用于移动实体的矩阵
            Vector3d vector = targetPt.GetVectorTo(sourcePt); // Point3d结构的GetVectorTo函数用于返回两个点之间的方向矢量，注意矢量的方向
            Matrix3d mt = Matrix3d.Displacement(vector);      // Matrix3d结构的静态函数Displacement用于移动对象
            // 以写的方式打开id表示的实体对象
            Entity ent = (Entity)id.GetObject(OpenMode.ForWrite);
            ent.TransformBy(mt); // 对实体实施移动
            ent.DowngradeOpen(); // 为防止错误，切换实体为读的状态
        }

        /// <summary>移动实体对象（利用Entity类的IsNewObject属性实现判断实体是否被添加到数据库中）</summary>
        /// <param name="ent">需要移动的实体</param>
        /// <param name="sourcePt">移动的源点</param>
        /// <param name="targetPt">移动的目标点</param>
        public static void Move(this Entity ent, Point3d sourcePt, Point3d targetPt)
        {
            if (ent.IsNewObject) //如果是还未被添加到数据库中的新实体
            {
                //构建用于移动实体的矩阵
                Vector3d vector = targetPt.GetVectorTo(sourcePt);
                Matrix3d mt = Matrix3d.Displacement(vector);
                ent.TransformBy(mt); //对实体实施移动
            }
            else //如果是已经添加到数据库中的实体
            {
                ent.ObjectId.Move(sourcePt, targetPt);
            }
        }

        /// <summary>用于复制实体对象</summary>
        /// <param name="id">实体的ObjectId</param>
        /// <param name="sourcePt">移动的源点</param>
        /// <param name="targetPt">移动的目标点</param>
        /// <returns>返回添加到模型空间中的实体</returns>
        public static ObjectId Copy(this ObjectId id, Point3d sourcePt, Point3d targetPt)
        {
            //构建用于复制实体的矩阵
            Vector3d vector = targetPt.GetVectorTo(sourcePt);
            Matrix3d mt = Matrix3d.Displacement(vector);
            //获取id表示的实体对象
            Entity ent = (Entity)id.GetObject(OpenMode.ForRead);
            //获取实体的复制件
            Entity entCopy = ent.GetTransformedCopy(mt);
            //将复制的实体对象添加到模型空间
            ObjectId copyId = id.Database.AddToModelSpace(entCopy);  // 这里出现database是因为AddToModelSpace使用了this关键字
            //返回复制实体的ObjectId
            return copyId;
         }

        ///<summary>用于旋转实体对象</summary>
        ///<param name="id">实体的ObjectId</param>
        ///<param name="basePt">旋转基点</param>
        ///<param name="angle">旋转角度</param>
        public static void Rotate(this ObjectId id, Point3d basePt, double angle)
        {
            Matrix3d mt = Matrix3d.Rotation(angle, Vector3d.ZAxis, basePt);
            Entity ent = (Entity)id.GetObject(OpenMode.ForWrite);
            ent.TransformBy(mt);
            ent.DowngradeOpen();
        }

        ///<summary>用于缩放实体对象</summary>
        ///<param name="id">实体的ObjectId</param>
        ///<param name="basePt">缩放基点</param>
        ///<param name="scaleFactor">缩放比例</param>
        public static void Scale(this ObjectId id, Point3d basePt, double scaleFactor)
        {
            Matrix3d mt = Matrix3d.Scaling(scaleFactor, basePt);
            Entity ent = (Entity)id.GetObject(OpenMode.ForWrite);
            ent.TransformBy(mt);
            ent.DowngradeOpen();
        }

        ///<summary>用于镜像实体对象</summary>
        ///<param name="id">实体的ObjectId</param>
        ///<param name="mirrorPt1">镜像轴的第一点</param>
        ///<param name="mirrorPt2">镜像轴的第二点</param>
        ///<param name="eraseSourceObject">是否删除源对象</param>
        /// <returns>返回添加到模型空间中的实体</returns>
        public static ObjectId Mirror(this ObjectId id, Point3d mirrorPt1, Point3d mirrorPt2, bool eraseSourceObject)
        {
            Line3d miLine = new Line3d(mirrorPt1, mirrorPt2); //镜像线
            Matrix3d mt = Matrix3d.Mirroring(miLine); //镜像矩阵
            ObjectId mirrorId = id;
            Entity ent = (Entity)id.GetObject(OpenMode.ForWrite);
            //如果删除源对象，则直接对源对象实行镜像变换
            if (eraseSourceObject == true)
            {
                ent.TransformBy(mt);
            }
            //如果不删除源对象，则镜像复制源对象
            else
            {
                Entity entCopy = ent.GetTransformedCopy(mt);
                mirrorId = id.Database.AddToModelSpace(entCopy);
            }
            return mirrorId;
        }

        /// <summary>
        /// 用于偏移命令
        /// </summary>
        /// <param name="Id">实体的Object</param>
        /// <param name="dis">偏移的距离</param>
        /// <returns></returns>
        public static ObjectIdCollection Offset(this ObjectId Id, double dis)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            Curve cur = Id.GetObject(OpenMode.ForWrite) as Curve;
            if(cur != null)
            {
                try
                {
                    // 获取偏移命令
                    DBObjectCollection offsetCurves = cur.GetOffsetCurves(dis);
                    // 讲对象集合类型转换为实体类的数组，以方便加入实体的操作
                    Entity[] offsetEnts = new Entity[offsetCurves.Count];
                    offsetCurves.CopyTo(offsetEnts, 0);
                    // 将偏移的对象加入到数据库
                    ids = Id.Database.AddModelSpace(offsetEnts);
                }
                catch // 如果偏移出现异常
                {
                    Application.ShowAlertDialog("无法偏移！");
                }
            }
            else
            {
                // 如果不是去曲线
                Application.ShowAlertDialog("无法偏移！");
            }
            return ids; // 返回偏移后的实体Id集合
        }


        ///<summary>用于矩阵阵列命令</summary>
        ///<param name="Id">实体ObjectId</param>
        ///<param name="numRows">矩阵阵列的行数</param>
        ///<param name="numCols">矩阵阵列的列数</param>
        ///<param name="disRows">矩阵行间的距离</param>
        ///<param name="disCols">矩阵列间的距离</param>
        public static ObjectIdCollection Arrayrectang(this ObjectId Id, int numRows, int numCols, double disRows, double disCols)
        {
            // 用于返回阵列后的实体集合的ObjectId
            ObjectIdCollection ids = new ObjectIdCollection();
            Entity ent = (Entity)Id.GetObject(OpenMode.ForRead);
            for (int m = 0; m < numRows; m++)
            {
                for (int n = 0; n < numCols; n++)
                {
                    // 获取平移矩阵
                    Matrix3d mt = Matrix3d.Displacement(new Vector3d(n * disCols, m * disRows, 0));
                    Entity entCopy = ent.GetTransformedCopy(mt); // 复制实体
                    // 将复制的实体添加到模型空间
                    ObjectId entCopyId = Id.Database.AddToModelSpace(entCopy);
                    ids.Add(entCopyId); // 将复制实体的ObjectId添加到集合中
                }
            }
            ent.UpgradeOpen(); // 切换实体为写的状态
            ent.Erase(); // 删除实体
            return ids; // 返回阵列后的实体集合的ObjectId
        }


        ///<summary>用环形矩阵命令</summary>
        ///<param name="Id">实体的ObjectId</param>
        ///<param name="cenPt">环形阵列的中心点</param>
        ///<param name="numObj">环形阵列中所要创建的对象数量</param>
        ///<param name="angle">以弧度表示的填充角度，正值表示逆时针方向旋转，负值表示顺时针方向旋转，如果角度为0会报错</param>
        public static ObjectIdCollection ArrayPolar(this ObjectId Id, Point3d cenPt, int numObj, double angle)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            Entity ent = (Entity)Id.GetObject(OpenMode.ForRead);
            for (int i = 0; i < numObj; i++)
            {
                Matrix3d mt = Matrix3d.Rotation(angle * (i + 1) / numObj, Vector3d.ZAxis, cenPt);
                Entity entCopy = ent.GetTransformedCopy(mt);
                ObjectId entCopyId = Id.Database.AddToModelSpace(entCopy);
                ids.Add(entCopyId);
            }
            return ids;
        }
            
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="Id">实体的ObjectId</param>
        public static void Erase(this ObjectId Id)
        {
            DBObject ent = Id.GetObject(OpenMode.ForWrite);
            ent.Erase();
        }
    }
}
