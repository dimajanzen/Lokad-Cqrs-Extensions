#region Copyright (c) 2011, EventDay Inc.

// Copyright (c) 2011, EventDay Inc.
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of the EventDay Inc. nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL EventDay Inc. BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.

#endregion

using System;
using System.Configuration;
using System.Data;
using System.Data.Common;

using EventStore.Persistence.SqlPersistence;

namespace Lokad.Cqrs.Extensions.EventStore.Build
{
    internal class DirectConnectionFactory : ConfigurationConnectionFactory
    {
        private readonly string connectionName;
        private readonly string connectionString;

        public DirectConnectionFactory(string connectionName, string connectionString)
            : base(connectionName)
        {
            this.connectionName = connectionName;
            this.connectionString = connectionString;
        }

        protected override IDbConnection Open(Guid streamId, string connectionName)
        {
            var setting = Settings;
            var factory = DbProviderFactories.GetFactory(setting.ProviderName);
            var connection = factory.CreateConnection();
            connection.ConnectionString = BuildConnectionString(streamId, setting);
            connection.Open();
            return connection;
        }

        public override ConnectionStringSettings Settings
        {
            get
            {
                var settings = new ConnectionStringSettings
                {
                    ProviderName = "System.Data.SqlClient",
                    Name = connectionName,
                    ConnectionString = connectionString
                };

                return settings;
            }
        }
    }
}