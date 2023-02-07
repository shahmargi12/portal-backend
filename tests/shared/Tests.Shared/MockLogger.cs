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

using Microsoft.Extensions.Logging;

namespace Org.Eclipse.TractusX.Portal.Backend.Tests.Shared;

public interface IMockLogger<T>
{
    void Log(LogLevel logLevel, Exception? exception, string logMessage);
}

public class MockLogger<T> : ILogger<T>
{
    private readonly IMockLogger<T> _logger;

    public MockLogger(IMockLogger<T> logger)
    {
        _logger = logger;
    }

    public IDisposable BeginScope<TState>(TState state) => new TestDisposable();

    public bool IsEnabled(LogLevel logLevel) => true;

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState,Exception?,string> formatter) =>
        _logger.Log(logLevel,exception,formatter(state,exception));
    
    public class TestDisposable : IDisposable
    {
        public void Dispose() {
            GC.SuppressFinalize(this);
        }
    }
}
