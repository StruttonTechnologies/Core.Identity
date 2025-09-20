namespace ST.Core.Identity.Fakes.Data
{
    public static class FakeRoles
    {
        public static IEnumerable<object[]> Valid =>
            new List<object[]>
            {
                new object[] { "Admin" },
                new object[] { "user" },
                new object[] { "SUPPORT" }
            };

        public static IEnumerable<object[]> Invalid =>
            new List<object[]>
            {
                new object[] { null },
                new object[] { "" },
                new object[] { "UnknownRole" }
            };
    }
}