using System;

namespace Distance.CustomDeathMessages
{
    [Flags]
    public enum MessageType : uint
    {
        None,
        KillGrid,
        SelfTermination,
        LaserOverheated,
        Impact,
        Overheated,
        AntiTunnelSquish,
        StuntCollect,
        KickNoLevel,
        Finished,
        NotReady,
        Spectate,
        TagPointsLead
    }
}
