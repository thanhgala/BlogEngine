namespace FrameworkCore.Infrastructure.Entity.Audit
{
    public interface IModifiedTracking
    {
        string CreatedBy { set; get; }

        string ModifiedBy { set; get; }
    }
}
