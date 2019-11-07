using Autodesk.AutoCAD.DatabaseServices;

namespace DotNetARX
{
    /// <summary>
    /// 将实体添加到模型空间
    /// </summary>
    public static class Tools
    {
        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ent">要添加的实体</param>
        /// <returns>返回添加到模型空间中的实体</returns>
        public static ObjectId AddToModelSpace(this Database db, Entity ent)
        {
            ObjectId entId; //用于返回添加到模型空间中的实体ObjectId
            
            //定义一个指向当前数据库的事务处理，以添加直线
            using(OpenCloseTransaction trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);  //以只读方式打开块表
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);  //以写方式打开模型空间块表记录
                entId = btr.AppendEntity(ent);  //将图形对象的信息添加到块表记录中
                trans.AddNewlyCreatedDBObject(ent, true);  //把对象添加到事务处理中
                trans.Commit();  //提交事务处理
            }
            return entId;
        }


        /*
        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到模型空间中的实体</returns>
        public static ObjectIdCollection AddToModelSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection(); //用于返回添加到模型空间中的实体ObjectId

            //定义一个指向当前数据库的事务处理，以添加直线
            using (OpenCloseTransaction trans = db.TransactionManager.StartOpenCloseTransaction())
            {
                BlockTable bt = (BlockTable)trans.GetObject(db.BlockTableId, OpenMode.ForRead);  //以只读方式打开块表
                BlockTableRecord btr = (BlockTableRecord)trans.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite);  //以写方式打开模型空间块表记录
                foreach (var ent in ents)
                {
                    ids.Add(btr.AppendEntity(ent));
                    trans.AddNewlyCreatedDBObject(ent, true);
                }
                trans.Commit();  //提交事务处理
            }
            return ids;
        }
        */
        

          
        /// <summary>
        /// 将实体添加到模型空间
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="ents">要添加的多个实体</param>
        /// <returns>返回添加到模型空间中的实体</returns>
        public static ObjectIdCollection AddModelSpace(this Database db, params Entity[] ents)
        {
            ObjectIdCollection ids = new ObjectIdCollection();
            var trans = db.TransactionManager;
            BlockTableRecord btr = (BlockTableRecord)trans.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite);
            foreach (var ent in ents)
            {
                ids.Add(btr.AppendEntity(ent));
                trans.AddNewlyCreatedDBObject(ent, true);
            }
            btr.DowngradeOpen();
            return ids;
        }
      

    }
}
