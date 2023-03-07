/********************************************************************************
 * Copyright (c) 2021, 2023 BMW Group AG
 * Copyright (c) 2021, 2023 Contributors to the Eclipse Foundation
 *
 * See the NOTICE file(s) distributed with this work for additional
 * information regarding copyright ownership.
 *
 * This program and the accompanying materials are made available under the
 * terms of the Apache License, Version 2.0 which is available at
 * https://www.apache.org/licenses/LICENSE-2.0.
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS, WITHOUT
 * WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. See the
 * License for the specific language governing permissions and limitations
 * under the License.
 *
 * SPDX-License-Identifier: Apache-2.0
 ********************************************************************************/

using AutoFixture;
using FakeItEasy;
using Org.Eclipse.TractusX.Portal.Backend.Apps.Service.BusinessLogic;
using Org.Eclipse.TractusX.Portal.Backend.Apps.Service.ViewModels;
using Org.Eclipse.TractusX.Portal.Backend.Tests.Shared.Extensions;
using Xunit;

namespace Org.Eclipse.TractusX.Portal.Backend.Apps.Service.Controllers.Tests;

public class AppChangeControllerTest
{
    private const string IamUserId = "4C1A6851-D4E7-4E10-A011-3732CD045E8A";
    private readonly IFixture _fixture;
    private readonly AppChangeController _controller;
    private readonly IAppChangeBusinessLogic _logic;

    public AppChangeControllerTest()
    {
        _fixture = new Fixture();
        _logic = A.Fake<IAppChangeBusinessLogic>();
        this._controller = new AppChangeController(_logic);
        _controller.AddControllerContextWithClaim(IamUserId);
    }

    [Fact]
    public async Task AddActiveAppUserRole_ReturnsExpectedCount()
    {
        var appId = _fixture.Create<Guid>();
        var iamUserId = _fixture.Create<string>();

        var appUserRoles = _fixture.CreateMany<AppUserRole>(3);
        var appRoleData = _fixture.CreateMany<AppRoleData>(3);
        A.CallTo(() => _logic.AddActiveAppUserRoleAsync(appId, appUserRoles, iamUserId))
            .Returns(appRoleData);

        //Act
        var result = await this._controller.AddActiveAppUserRole(appId, appUserRoles).ConfigureAwait(false);
        foreach (var item in result)
        {
            //Assert
            A.CallTo(() => _logic.AddActiveAppUserRoleAsync(appId, appUserRoles, iamUserId)).MustHaveHappenedOnceExactly();
            Assert.NotNull(item);
            Assert.IsType<AppRoleData>(item);
        }
    }
}