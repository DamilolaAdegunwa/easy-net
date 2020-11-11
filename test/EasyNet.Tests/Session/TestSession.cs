using EasyNet.Runtime.Session;

namespace EasyNet.Tests.Session
{
    public class TestSession : EasyNetSessionBase
    {
        public override string UserId => "1";
        public override string TenantId => "1";
        public override string UserName => "Test";
        public override string Role => "Admin";
    }
}
