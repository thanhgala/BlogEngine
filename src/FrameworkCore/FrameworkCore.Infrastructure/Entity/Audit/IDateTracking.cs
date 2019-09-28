using System;

namespace FrameworkCore.Infrastructure.Entity.Audit
{
    public interface IDateTracking
    {
        DateTime DateCreated { set; get; }

        DateTime DateModified { set; get; }
    }
}
