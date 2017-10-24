using System;
using System.Collections.Generic;
using System.Text;

namespace RSRepository
{
    public interface IDbTransaction
    {
        void Commit();
        void Roolback();
    }
}
