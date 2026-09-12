using QDR_Server.Models;

namespace QDR_Server.Services
{
    public static class OrganizationPermissions
    {
        public const string CreateEvent = "CreateEvent";
        public const string DeleteEvent = "DeleteEvent";
        public const string EditEvent = "EditEvent";
        public const string DeleteMember = "DeleteMember";
        public const string InviteMember = "InviteMember";
        public const string EditOrg = "EditOrg";
        public const string DeleteOrg = "DeleteOrg";
    }

    public static class OrganizationPositionService
    {
        private static readonly HashSet<string> EventPermissions =
            [OrganizationPermissions.CreateEvent, OrganizationPermissions.DeleteEvent, OrganizationPermissions.EditEvent];

        private static readonly HashSet<string> MemberPermissions =
            [OrganizationPermissions.DeleteMember, OrganizationPermissions.InviteMember];

        private static readonly HashSet<string> OrgPermissions =
            [OrganizationPermissions.EditOrg, OrganizationPermissions.DeleteOrg];

        private static readonly Dictionary<OrganizationPosition, HashSet<string>> Permissions = new()
        {
            [OrganizationPosition.Owner] = [.. EventPermissions, .. MemberPermissions, .. OrgPermissions],
            [OrganizationPosition.Admin] = [.. EventPermissions, .. MemberPermissions],
            [OrganizationPosition.Organizer] = [.. EventPermissions],
            [OrganizationPosition.Member] = []
        };

        public static bool Has(OrganizationPosition position, string permission) =>
            Permissions.TryGetValue(position, out var perms) && perms.Contains(permission);

        public static IReadOnlyCollection<string> GetPermissions(OrganizationPosition position) =>
            Permissions.TryGetValue(position, out var perms) ? perms : [];
    }
}
