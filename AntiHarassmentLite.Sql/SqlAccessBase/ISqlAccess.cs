namespace AntiHarassmentLite.Sql
{
    public interface ISqlAccess
    {
        ISqlCommandWrapper CreateStoredProcedure(string sql);
        ISqlCommandWrapper CreateQuery(string sql);
    }
}
