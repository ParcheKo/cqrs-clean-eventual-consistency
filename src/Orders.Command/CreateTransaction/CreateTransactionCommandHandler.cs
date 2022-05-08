using System;
using System.Threading.Tasks;
using Orders.Command.Abstractions;
using Orders.Core.Shared;
using Orders.Core.Transactions;

namespace Orders.Command.CreateTransaction;

public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, CreateTransactionCommandResult>
{
    private readonly IEventBus _eventBus;
    private readonly ITransactionRepository _transactionRepository;

    public CreateTransactionCommandHandler(
        IEventBus eventBus,
        ITransactionRepository transactionRepository
    )
    {
        _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        _transactionRepository =
            transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
    }

    public async Task<CreateTransactionCommandResult> Handle(CreateTransactionCommand command)
    {
        var charge = new Money(
            command.Amount,
            command.CurrencyCode
        );
        var newTransaction = Transaction.CreateTransactionForCard(
            command.CardId,
            command.UniqueId,
            command.ChargeDate,
            charge
        );

        var success = await _transactionRepository.Add(newTransaction);

        if (success)
        {
            var transactionCreatedEvent = new TransactionCreatedEvent(newTransaction);

            _eventBus.Publish(transactionCreatedEvent);
        }

        return new CreateTransactionCommandResult(
            newTransaction.Id,
            newTransaction.CardId,
            newTransaction.ChargeDate,
            newTransaction.UniqueId,
            newTransaction.Charge.Amount,
            newTransaction.Charge.CurrencyCode,
            success
        );
    }
}