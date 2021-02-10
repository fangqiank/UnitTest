using System;

namespace UnitTest
{
    public class TransferPolicyEvaluator
    {
        public virtual bool IsInTransferPeriod()
        {
            throw new NotImplementedException();
        }

        protected virtual bool IsBannedFromTranserring()
        {
            throw new NotImplementedException();
        }
    }
}
