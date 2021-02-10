using Moq;
using Moq.Protected;
using UnitTest;
using Xunit;

namespace TestMain
{
    public class TransferApprovalShould
    {
        private Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();
        private Mock<TransferPolicyEvaluator> mockTransferPolicy = new Mock<TransferPolicyEvaluator>();
        private TransferApproval approval;

        public TransferApprovalShould()
        {
            mockExamination.SetupAllProperties();
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("Available");
            mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
                .Returns(true);

            mockTransferPolicy.Setup(x => x.IsInTransferPeriod()).Returns(true);
            mockTransferPolicy.Protected()
                .Setup<bool>("IsBannedFromTranserring")
                .Returns(true);
            approval = new TransferApproval(mockExamination.Object, mockTransferPolicy.Object);
        }

        [Fact]
        public void ApprovalYoungCheapPlayerTransfer()
        {

            //Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();
            //Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>(MockBehavior.Strict);

            //mockExamination.Setup(x => x.IsHealthy(It.IsAny<int>(),
            //    It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //mockExamination.Setup(x => x.IsHealthy(It.Is<int>(age => age < 30),
            //    It.IsIn<int>(80, 85, 90), 
            //    It.IsInRange<int>(75, 99, Range.Inclusive)))
            //    .Returns(true);

            //var mockExamination = new Mock<IPhysicalExamination>();
            //mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("Available");

            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true); 
            
            //var approval = new TransferApproval(mockExamination.Object);

            var emreTransfer = new TransferApplication
            {
                PlayerName = "Emre Can",
                PlayerAge = 24,
                TransferFee = 0,
                AnnualSalary = 4.52m,
                ContractYears = 4,
                IsSuperStar = false,

                PlayerStrength = 80,
                PlayerSpeed = 75
            };

            var result = approval.Evaluate(emreTransfer);

            Assert.Equal(TransferResult.Approved,result);
        }

        [Fact]
        public void RejectedWhenNonSuperstarOldPlayer()
        {
           var mockExamination = new Mock<IPhysicalExamination>
           {
               DefaultValue = DefaultValue.Mock
           };

            //Mock<IPhysicalExamination> mockExamination = new Mock<IPhysicalExamination>();

            //mockExamination.Setup(x => x.IsMedicalRoomAvailable).Returns(true);


            //bool isHealthy = true;
            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>(), out isHealthy));
            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //var approval = new TransferApproval(mockExamination.Object);

            var carlosTransfer = new TransferApplication
            {
                PlayerName = "Carlos Bacca",
                PlayerAge = 32,
                TransferFee = 15m,
                AnnualSalary = 3.5m,
                ContractYears = 4,
                IsSuperStar = false,
                PlayerStrength = 80,
                PlayerSpeed = 70
            };

            var result = approval.Evaluate(carlosTransfer);

            Assert.Equal(TransferResult.Rejected,result);
        }

        [Fact]
        public void ReferToBossWhenTransferringSuperStar()
        {
            //var mockExamination = new Mock<IPhysicalExamination>
            //{
            //    DefaultValue = DefaultValue.Mock
            //};

            //mockExamination.Setup(x=>x.MedicalRoom.Status.IsAvailable);
            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //var approval = new TransferApproval(mockExamination.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.ReferredToBoss,result);
        }

        [Fact]
        public void PhysicalGradeShouldPassWhenTransferringSuperStar()
        {
            mockExamination.DefaultValue = DefaultValue.Mock;
            // 开始追踪PhysicalGrade属性
            mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Failed);

            //mockExamination.SetupAllProperties();

            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Failed);

            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //var approval = new TransferApproval(mockExamination.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(PhysicalGrade.Passed,mockExamination.Object.PhysicalGrade);

        }

        [Fact]
        public void ShouldPhysicalExamineWhenTransferringSuperStar()
        {
            //var mockExamination = new Mock<IPhysicalExamination>();

            //mockExamination.Setup(c => c.MedicalRoom.Status.IsAvailable).Returns("Available");
            ////mockExamination.SetupAllProperties();
            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>())).Returns(true);

            //var approval = new TransferApproval(mockExamination.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            //确认方法被调用
            //mockExamination.Verify(x=>
            //    x.IsHealthy(33,95,88),Times.Exactly(12),"Arguments not match");

            //确认属性被访问
            mockExamination.VerifyGet(x=>x.MedicalRoom.Status.IsAvailable);
            //mockExamination.VerifyGet(x=>x.MedicalRoom.Name);
        }

        [Fact]
        public void ShouldSetPhysicalGradeWhenTransferringSuperStar()
        {
            //var mockExamination = new Mock<IPhysicalExamination>();

            //mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("Avail");
            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            ////mockExamination.Setup(x =>
            ////    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            ////    .Returns(true);

            //mockExamination.SetupSequence(x =>
            //        x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            //    .Returns(true)
            //    .Returns(false);

            //var approval = new TransferApproval(mockExamination.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.ReferredToBoss,result);

            var result2 = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.Rejected,result2);

            //确认set
            mockExamination.VerifySet(c=>c.PhysicalGrade=PhysicalGrade.Passed);
            //mockExamination.VerifySet(c=>c.PhysicalGrade=PhysicalGrade.Superb);
        }

        [Fact]
        public void PostponedWhenTransferringChildPlayer()
        {
            //var mockExamination = new Mock<IPhysicalExamination>();
            //mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("xxx");
            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            //mockExamination.Setup(x =>
            //        x.IsHealthy(It.Is<int>(age => age < 16), It.IsAny<int>(), It.IsAny<int>()))
            //    //.Throws<Exception>();
            //    .Throws(new Exception("The player is still a child"));
            //var approval = new TransferApproval(mockExamination.Object);

            var childTransfer = new TransferApplication
            {
                PlayerName = "Some Child Player",
                PlayerAge = 13,
                TransferFee = 0,
                AnnualSalary = 0.01m,
                ContractYears = 3,
                IsSuperStar = false,
                PlayerStrength = 40,
                PlayerSpeed = 50
            };

            var result = approval.Evaluate(childTransfer);

            Assert.Equal(TransferResult.Postponed,result);
        }

        [Fact]
        public void ShouldPlayerHealthCheckedWhenTransferringSuperstar()
        {
            //var mockExamination = new Mock<IPhysicalExamination>();
            //mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("xxx");
            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            //mockExamination.Setup(x =>
            //    x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            //    .Returns(true)
            //    .Raises(x=>x.HealthChecked += null,EventArgs.Empty);

            //var approval = new TransferApproval(mockExamination.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };

            var result = approval.Evaluate(cr7Transfer);

            //mockExamination.Raise(x=>x.HealthChecked += null,EventArgs.Empty);

            Assert.True(approval.PlayerHealthChecked);

        }

        [Fact]
        public void ShouldPostponedWhenNotInTransferperiod()
        {
            //var mockExamination = new Mock<IPhysicalExamination>();
            //mockExamination.Setup(x => x.MedicalRoom.Status.IsAvailable).Returns("xxx");
            //mockExamination.SetupProperty(x => x.PhysicalGrade, PhysicalGrade.Passed);
            //mockExamination.Setup(x =>
            //        x.IsHealthy(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int>()))
            //    .Returns(true);

            //var mockTransferPlicy = new Mock<TransferPolicyEvaluator>();
            //mockTransferPlicy.Setup(x => x.IsInTransferPeriod()).Returns(false);

            

            // approval = new TransferApproval(mockExamination.Object,mockTransferPlicy.Object);

            var cr7Transfer = new TransferApplication
            {
                PlayerName = "Cristiano Ronaldo",
                PlayerAge = 33,
                TransferFee = 112m,
                AnnualSalary = 30m,
                ContractYears = 4,
                IsSuperStar = true,
                PlayerStrength = 90,
                PlayerSpeed = 90
            };
            
            var result = approval.Evaluate(cr7Transfer);
            Assert.Equal(TransferResult.Postponed,result);

        }

    }
}
