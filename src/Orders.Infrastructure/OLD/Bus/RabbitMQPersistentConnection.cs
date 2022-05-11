﻿// using System;
// using System.IO;
// using System.Net.Sockets;
// using Microsoft.Extensions.Logging;
// using Orders.Core;
// using Orders.Core.Shared;
// using Polly;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using RabbitMQ.Client.Exceptions;
//
// namespace Orders.Infrastructure.Bus;
//
// public class RabbitMqPersistentConnection : IPersistentConnection<IModel>
// {
//     // todo: if another micro was in the picture 
//
//     private readonly IConnectionFactory _connectionFactory;
//     private readonly ILogger<RabbitMqPersistentConnection> _logger;
//     private readonly int _retryCount;
//     private readonly object _syncRoot = new();
//     private IConnection _connection;
//     private bool _disposed;
//
//     public RabbitMqPersistentConnection(
//         AppConfiguration configuration,
//         ILogger<RabbitMqPersistentConnection> logger
//     )
//     {
//         _connectionFactory = CreateConnectionFactory(configuration);
//         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//         _retryCount = configuration.RetryCount ?? 5;
//     }
//
//     public bool IsConnected => _connection != null && _connection.IsOpen && !_disposed;
//
//     public IModel CreateModel()
//     {
//         if (!IsConnected)
//             throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
//
//         return _connection.CreateModel();
//     }
//
//     public void Dispose()
//     {
//         if (_disposed) return;
//
//         _disposed = true;
//
//         try
//         {
//             _connection.Dispose();
//         }
//         catch (IOException ex)
//         {
//             _logger.LogCritical(ex.ToString());
//         }
//     }
//
//     public bool TryConnect()
//     {
//         _logger.LogInformation("RabbitMQ Client is trying to connect");
//
//         lock (_syncRoot)
//         {
//             var policy = Policy.Handle<SocketException>()
//                 .Or<BrokerUnreachableException>()
//                 .WaitAndRetry(
//                     _retryCount,
//                     retryAttempt => TimeSpan.FromSeconds(
//                         Math.Pow(
//                             2,
//                             retryAttempt
//                         )
//                     ), (
//                         ex,
//                         time
//                     ) =>
//                     {
//                         _logger.LogWarning(ex.ToString());
//                     }
//                 );
//
//             policy.Execute(
//                 () =>
//                 {
//                     _connection = _connectionFactory
//                         .CreateConnection();
//                 }
//             );
//
//             if (IsConnected)
//             {
//                 _connection.ConnectionShutdown += OnConnectionShutdown;
//                 _connection.CallbackException += OnCallbackException;
//                 _connection.ConnectionBlocked += OnConnectionBlocked;
//
//                 _logger.LogInformation(
//                     $"RabbitMQ persistent connection acquired a connection {_connection.Endpoint.HostName} and is subscribed to failure events"
//                 );
//
//                 return true;
//             }
//
//             _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
//
//             return false;
//         }
//     }
//
//     private IConnectionFactory CreateConnectionFactory(AppConfiguration configuration)
//     {
//         var factory = new ConnectionFactory
//         {
//             HostName = configuration.ConnectionStrings.EventBusHostname
//         };
//
//         if (!string.IsNullOrEmpty(configuration.ConnectionStrings.EventBusUsername))
//             factory.UserName = configuration.ConnectionStrings.EventBusUsername;
//
//         if (!string.IsNullOrEmpty(configuration.ConnectionStrings.EventBusPassword))
//             factory.Password = configuration.ConnectionStrings.EventBusPassword;
//
//         return factory;
//     }
//
//     private void OnCallbackException(
//         object sender,
//         CallbackExceptionEventArgs e
//     )
//     {
//         if (_disposed) return;
//
//         _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");
//
//         TryConnect();
//     }
//
//     private void OnConnectionBlocked(
//         object sender,
//         ConnectionBlockedEventArgs e
//     )
//     {
//         if (_disposed) return;
//
//         _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");
//
//         TryConnect();
//     }
//
//     private void OnConnectionShutdown(
//         object sender,
//         ShutdownEventArgs reason
//     )
//     {
//         if (_disposed) return;
//
//         _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");
//
//         TryConnect();
//     }
// }

