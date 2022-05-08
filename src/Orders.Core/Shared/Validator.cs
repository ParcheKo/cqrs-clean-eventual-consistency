using System;

namespace Orders.Core.Shared
{
    public abstract class Validator<TEntity>
    {
        private readonly ValidationNotificationHandler _notificationHandler;

        protected Validator(ValidationNotificationHandler notificationHandler)
        {
            this._notificationHandler = notificationHandler ?? throw new ArgumentNullException(nameof(notificationHandler));
        }

        public abstract void Validate(TEntity entity);

        protected void CheckRule(TEntity entity, ISpecification<TEntity> specification, string code, string message)
        {
            var isSatisfied = specification.IsSatisfiedBy(entity);

            if (!isSatisfied)
            {
                _notificationHandler.AddNotification(code, message);
            }
        }

        protected void CheckRule<TSpecification>(TEntity entity, string code, string message) where TSpecification : CompositeSpecification<TEntity>, new()
        {
            var spec = new TSpecification();

            CheckRule(entity, spec, code, message);
        }
    }
}
