using System.Data.SqlClient;
using System.Runtime.CompilerServices;

namespace BigLamp.Extensions.Exception
{

    public static class SqlExceptionExtensions
    {
        private enum SqlErrorCodes
        {
            DuplicateKey = 2601
        }
        public static SqlException GetSqlException(this System.Exception ex)
        {
            SqlException se = null;
            var next = ex;

            if (next.InnerException != null)
            {
                while (next.InnerException != null)
                {
                    se = next.InnerException as SqlException;
                    next = next.InnerException;
                }
            }
            else
            {
                se = next as SqlException;
            }
     

            return se;

        }

        public static bool IsDuplicateKeyException(this SqlException ex)
        {
            return ex != null && ex.Number == (int) SqlErrorCodes.DuplicateKey;
        }
        public static bool IsDuplicateKeyException(this SqlException ex, string indexName)
        {
            return ex != null && ex.Number == (int)SqlErrorCodes.DuplicateKey && ex.Message.Contains(indexName);
        }
    }
}
