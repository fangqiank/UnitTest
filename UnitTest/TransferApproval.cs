using System;

namespace UnitTest
{
    public class TransferApproval
    {
        private readonly IPhysicalExamination _physicalExamination;
        private readonly TransferPolicyEvaluator _transferPolicyEvaluator;
        private const int RemainingBudget = 300;
        public bool PlayerHealthChecked { get; private set; }

        public TransferApproval(IPhysicalExamination physicalExamination,TransferPolicyEvaluator transferPolicyEvaluator)
        {
            _physicalExamination = physicalExamination ?? throw new ArgumentNullException(nameof(physicalExamination));
            _transferPolicyEvaluator = transferPolicyEvaluator;
            _physicalExamination.HealthChecked += PhysicalExaminationHealthChecked;
        }

        private void PhysicalExaminationHealthChecked(object sender, EventArgs e)
        {
            PlayerHealthChecked = true;
        }

        public TransferResult Evaluate(TransferApplication transfer)
        {
            //var isHealthy =
            //    _physicalExamination.IsHealthy(transfer.PlayerAge, transfer.PlayerStrength, transfer.PlayerSpeed);


            //if (!_physicalExamination.IsMedicalRoomAvailable)
            //{
            //    return TransferResult.Rejected;
            //}

            if (!_transferPolicyEvaluator.IsInTransferPeriod())
            {
                return TransferResult.Postponed;
            }

            if (_physicalExamination.MedicalRoom.Status.IsAvailable == "Disabled")
            {
                return TransferResult.Postponed;
            }

            //_physicalExamination.IsHealthy(transfer.PlayerAge, transfer.PlayerStrength, transfer.PlayerSpeed,
            //    out var isHealthy);
            //var isHealthy = _physicalExamination
            //    .IsHealthy(transfer.PlayerAge, transfer.PlayerStrength, transfer.PlayerSpeed);

            //if (!isHealthy)
            //{
            //    _physicalExamination.PhysicalGrade = PhysicalGrade.Failed;
            //    return TransferResult.Rejected;
            //}
            //else
            //{
            //    if (transfer.PlayerAge < 25)
            //    {
            //        _physicalExamination.PhysicalGrade = PhysicalGrade.Superb;
            //    }
            //    else
            //    {
            //        _physicalExamination.PhysicalGrade = PhysicalGrade.Passed;
            //    }
            //}

            bool isHealthy;
            try
            {
                isHealthy = _physicalExamination.IsHealthy(transfer.PlayerAge, transfer.PlayerStrength,
                    transfer.PlayerSpeed);
            }
            catch (Exception)
            {
                return TransferResult.Postponed;
            }

            if(!isHealthy)
            {
                _physicalExamination.PhysicalGrade = PhysicalGrade.Failed;
                return TransferResult.Rejected;
            }
            else
            {
                if (transfer.PlayerAge < 25)
                {
                    _physicalExamination.PhysicalGrade = PhysicalGrade.Superb;
                }
                else
                {
                    _physicalExamination.PhysicalGrade = PhysicalGrade.Passed;
                }
            }


            var totalTransfer = transfer.TransferFee + transfer.ContractYears * transfer.AnnualSalary;
            if (RemainingBudget < totalTransfer)
            {
                return TransferResult.Rejected;
            }

            if (transfer.PlayerAge < 30)
            {
                return TransferResult.Approved;
            }

            if (transfer.IsSuperStar)
            {
                return TransferResult.ReferredToBoss;
            }

            return TransferResult.Rejected;
        }
    }
}
