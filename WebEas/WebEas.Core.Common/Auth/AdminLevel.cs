using ServiceStack.DataAnnotations;

namespace WebEas
{
    public enum AdminLevel : short
    {
        [Description("MEMBER")]
        User = 0,

        [Description("ADMIN")]
        CfeAdmin = 1,

        [Description("SYS_ADMIN")]
        SysAdmin = 2
    }
}
