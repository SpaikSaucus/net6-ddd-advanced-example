using System.ComponentModel;

namespace AuthorizationOperation.Application.Shared.ViewModels
{
    public enum EnumStatusRequest
    {
        [Description("DEFAULT")]
        DEFAULT,
        [Description("WAITING_FOR_SIGNERS")]
        WAITING_FOR_SIGNERS,
        [Description("AUTHORIZED")]
        AUTHORIZED,
        [Description("EXPIRED")]
        EXPIRED,
        [Description("CANCELLED")]
        CANCELLED
    }
}
