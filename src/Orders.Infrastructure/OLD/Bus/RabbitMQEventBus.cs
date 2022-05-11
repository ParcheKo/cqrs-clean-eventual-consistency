// using System;
// using System.Collections.Generic;
// using System.Net.Sockets;
// using System.Reflection;
// using System.Text;
// using System.Threading.Tasks;
// using CqrsEssentials;
// using Microsoft.Extensions.Logging;
// using Newtonsoft.Json;
// using Newtonsoft.Json.Serialization;
// using Orders.Core.Shared;
// using Polly;
// using RabbitMQ.Client;
// using RabbitMQ.Client.Events;
// using RabbitMQ.Client.Exceptions;
//
// namespace Orders.Infrastructure.Bus;
//
// public class RabbitMqEventBus : IEventBus, IDisposable
// {
//     private static readonly Dictionary<string, Type> SubsManager = new();
//     private readonly string _brokerName = "orders_events";
//
//     private readonly IEventDispatcher _eventDispatcher;
//     private readonly ILogger<RabbitMqEventBus> _logger;
//     private readonly IPersistentConnection<IModel> _persistentConnection;
//     private readonly string _queueName = "orders_events";
//     private readonly int _retryCount;
//
//     private IModel _consumerChannel;
//
//     private bool _disposedValue; // To detect redundant calls
//
//     public RabbitMqEventBus(
//         IEventDispatcher eventDispatcher,
//         IPersistentConnection<IModel> persistentConnection,
//         ILogger<RabbitMqEventBus> logger,
//         int retryCount = 5
//     )
//     {
//         _eventDispatcher = eventDispatcher ?? throw new ArgumentNullException(nameof(eventDispatcher));
//         _persistentConnection = persistentConnection ?? throw new ArgumentNullException(nameof(persistentConnection));
//         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
//         _retryCount = retryCount;
//         _consumerChannel = CreateConsumerChannel();
//
//         JsonConvert.DefaultSettings = () => new JsonSerializerSettings
//         {
//             ContractResolver = new NonPublicPropertiesResolver()
//         };
//     }
//
//     void IDisposable.Dispose()
//     {
//         Dispose(true);
//     }
//
//     public void Publish(IEvent @event)
//     {
//         if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();
//
//         var policy = Policy.Handle<BrokerUnreachableException>()
//             .Or<SocketException>()
//             .WaitAndRetry(
//                 _retryCount,
//                 retryAttempt => TimeSpan.FromSeconds(
//                     Math.Pow(
//                         2,
//                         retryAttempt
//                     )
//                 ), (
//                     ex,
//                     time
//                 ) =>
//                 {
//                     _logger.LogWarning(
//                         "Retrying message because of error {0}",
//                         ex.ToString()
//                     );
//                 }
//             );
//
//         using var channel = _persistentConnection.CreateModel();
//         var eventName = @event.GetType().Name;
//
//         channel.ExchangeDeclare(
//             _brokerName,
//             "direct"
//         );
//
//         var message = JsonConvert.SerializeObject(@event);
//         var body = Encoding.UTF8.GetBytes(message);
//
//         policy.Execute(
//             () =>
//             {
//                 var properties = channel.CreateBasicProperties();
//                 properties.DeliveryMode = 2; // persistent
//
//                 channel.BasicPublish(
//                     _brokerName,
//                     eventName,
//                     true,
//                     properties,
//                     body
//                 );
//             }
//         );
//     }
//
//     public void Subscribe<T>() where T : IEvent
//     {
//         var eventName = typeof(T).Name;
//         var containsKey = SubsManager.ContainsKey(eventName);
//         if (!containsKey)
//             SubsManager.Add(
//                 eventName,
//                 typeof(T)
//             );
//
//         if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();
//
//         using var channel = _persistentConnection.CreateModel();
//         channel.QueueBind(
//             _queueName,
//             _brokerName,
//             eventName
//         );
//     }
//
//     private IModel CreateConsumerChannel()
//     {
//         if (!_persistentConnection.IsConnected) _persistentConnection.TryConnect();
//
//         var channel = _persistentConnection.CreateModel();
//
//         channel.ExchangeDeclare(
//             _brokerName,
//             "direct"
//         );
//
//         channel.QueueDeclare(
//             _queueName,
//             true,
//             false,
//             false,
//             null
//         );
//
//         var consumer = new EventingBasicConsumer(channel);
//         consumer.Received += async (
//             model,
//             ea
//         ) =>
//         {
//             var eventName = ea.RoutingKey;
//             var message = Encoding.UTF8.GetString(ea.Body.ToArray());
//
//             await ProcessEvent(
//                 eventName,
//                 message
//             );
//
//             channel.BasicAck(
//                 ea.DeliveryTag,
//                 false
//             );
//         };
//
//         channel.BasicConsume(
//             _queueName,
//             false,
//             consumer
//         );
//
//         channel.CallbackException += (
//             sender,
//             ea
//         ) =>
//         {
//             _consumerChannel.Dispose();
//             _consumerChannel = CreateConsumerChannel();
//         };
//
//         return channel;
//     }
//
//     private async Task ProcessEvent(
//         string eventName,
//         string message
//     )
//     {
//         if (SubsManager.ContainsKey(eventName))
//         {
//             var type = SubsManager[eventName];
//             var @event = JsonConvert.DeserializeObject(
//                 message,
//                 type
//             ) as IEvent;
//
//             await _eventDispatcher.DispatchAsync(@event);
//         }
//     }
//
//     protected virtual void Dispose(bool disposing)
//     {
//         if (!_disposedValue)
//         {
//             if (disposing)
//             {
//                 if (_consumerChannel != null) _consumerChannel.Dispose();
//
//                 SubsManager.Clear();
//             }
//
//             _disposedValue = true;
//         }
//     }
//
//     public class NonPublicPropertiesResolver : DefaultContractResolver
//     {
//         protected override JsonProperty CreateProperty(
//             MemberInfo member,
//             MemberSerialization memberSerialization
//         )
//         {
//             var prop = base.CreateProperty(
//                 member,
//                 memberSerialization
//             );
//             if (member is PropertyInfo pi)
//             {
//                 prop.Readable = pi.GetMethod != null;
//                 prop.Writable = pi.SetMethod != null;
//             }
//
//             return prop;
//         }
//     }
// }

