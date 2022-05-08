using System.Threading.Tasks;
using Autofac;
using Orders.Command.Abstractions;

namespace Orders.Infrastructure.Dispatchers
{
    public class CommandDispatcher : ICommandDispatcher
    {
        private readonly IComponentContext _componentContext;

        public CommandDispatcher(IComponentContext componentContext)
        {
            this._componentContext = componentContext;
        }

        public Task<TResult> Dispatch<TResult>(ICommand<TResult> command) where TResult : ICommandResult
        {
            if (command == null)
            {
                throw new System.ArgumentNullException(nameof(command));
            }

            var commandHandlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResult));

            dynamic handler = _componentContext.Resolve(commandHandlerType);

            return (Task<TResult>)commandHandlerType
                .GetMethod("Handle")
                .Invoke(handler, new object[] { command });
        }

        public Task DispatchNonResult(ICommand command)
        {
            var commandHandlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());

            dynamic handler = _componentContext.Resolve(commandHandlerType);

            return (Task)commandHandlerType
                .GetMethod("HandleNonResult")
                .Invoke(handler, new object[] { command });
        }
    }
}