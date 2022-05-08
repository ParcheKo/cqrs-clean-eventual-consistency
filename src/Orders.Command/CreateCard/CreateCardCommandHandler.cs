using System;
using System.Threading.Tasks;
using Orders.Command.Abstractions;
using Orders.Core;
using Orders.Core.Cards;
using Orders.Core.Shared;

namespace Orders.Command.CreateCard
{
    public class CreateCardCommandHandler : ICommandHandler<CreateCardCommand, CreateCardCommandResult>
    {
        private readonly IEventBus _eventBus;
        private readonly ICardWriteOnlyRepository _cardRepository;
        private readonly ValidationNotificationHandler _notificationHandler;

        public CreateCardCommandHandler(IEventBus eventBus, ICardWriteOnlyRepository cardRepository, ValidationNotificationHandler notificationHandler)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _cardRepository = cardRepository ?? throw new ArgumentNullException(nameof(cardRepository));
            _notificationHandler = notificationHandler ?? throw new ArgumentNullException(nameof(notificationHandler)); ;
        }

        public async Task<CreateCardCommandResult> Handle(CreateCardCommand command)
        {
            if (_cardRepository.IsDuplicatedCardNumber(command.Number))
            {
                _notificationHandler.AddNotification(nameof(CreateCardCommand.Number), $"Card number already exists {command.Number}");
            }

            var newCard = Card.CreateNewCard(command.Number, command.CardHolder, command.ExpirationDate);
            newCard.Validate(_notificationHandler);

            // todo : temporary
            if (true) //newCard.Valid)
            {
                var success = await _cardRepository.Add(newCard);

                if (success)
                {
                    var cardCreatedEvent = new CardCreatedEvent(newCard);

                    _eventBus.Publish(cardCreatedEvent);
                }

                return new CreateCardCommandResult(newCard.Id, newCard.Number, newCard.CardHolder, newCard.ExpirationDate, success);
            }

            return CommandResult.CreateFailResult<CreateCardCommandResult>();
        }
    }
}