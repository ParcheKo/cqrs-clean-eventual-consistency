﻿using Orders.Core.Shared;

namespace Orders.Core.Cards
{
    public interface ICardWriteOnlyRepository : IWriteOnlyRepository<Card>
    {
        bool IsDuplicatedCardNumber(string cardNamber);
    }
}