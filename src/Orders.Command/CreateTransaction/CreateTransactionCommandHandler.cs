﻿using System;
using System.Threading.Tasks;
using Orders.Command.Abstractions;
using Orders.Core;
using Orders.Core.Shared;
using Orders.Core.Transactions;

namespace Orders.Command.CreateTransaction
{
    public class CreateTransactionCommandHandler : ICommandHandler<CreateTransactionCommand, CreateTransactionCommandResult>
    {
        private readonly IEventBus eventBus;
        private readonly ITransactionWriteOnlyRepository transactionRepository;

        public CreateTransactionCommandHandler(IEventBus eventBus, ITransactionWriteOnlyRepository transactionRepository)
        {
            this.eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            this.transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));
        }

        public async Task<CreateTransactionCommandResult> Handle(CreateTransactionCommand command)
        {
            var charge = new Money(command.Amount, command.CurrencyCode);
            var newTransaction = Transaction.CreateTransactionForCard(command.CardId, command.UniqueId, command.ChargeDate, charge);

            var success = await transactionRepository.Add(newTransaction);

            if (success)
            {
                var transactionCreatedEvent = new TransactionCreatedEvent(newTransaction);

                eventBus.Publish(transactionCreatedEvent);
            }

            return new CreateTransactionCommandResult(
                newTransaction.Id,
                newTransaction.CardId,
                newTransaction.ChargeDate,
                newTransaction.UniqueId,
                newTransaction.Charge.Amount,
                newTransaction.Charge.CurrencyCode,
                success);
        }
    }
}