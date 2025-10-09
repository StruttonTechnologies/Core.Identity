using Microsoft.AspNetCore.Identity;
using Moq;
using ST.Core.Identity.Stub.Data;
using ST.Core.Identity.Stub.Entities;
using ST.Core.Identity.Test.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST.Core.Identity.Mocks.Factories
{
    /// <summary>
    /// Factory for creating mocked <see cref="UserManager{TUser}"/> instances
    /// preconfigured with <see cref="StubUsers"/>.
    /// </summary>
    public static class MockUserManagerFactory
    {
        public static Mock<UserManager<StubUser>> Create()
        {
            var store = new Mock<IUserStore<StubUser>>();
            var mgr = new Mock<UserManager<StubUser>>(
                store.Object,
                null!, null!, null!, null!, null!, null!, null!, null!);

            mgr.Setup(m => m.FindByIdAsync(StubUserData.Default.Id.ToString()))
                .ReturnsAsync(StubUserData.Default);


            mgr.Setup(m => m.FindByNameAsync("admin"))
               .ReturnsAsync(StubUserData.Admin);

            return mgr;
        }
    }
}