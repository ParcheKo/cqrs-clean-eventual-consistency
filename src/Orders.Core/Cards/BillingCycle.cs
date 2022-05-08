using System;

namespace Orders.Core.Cards
{
    public sealed class BillingCycle
    {
        public int DueDay { get; private set; }
        public int Range { get; private set; }
    }
}
