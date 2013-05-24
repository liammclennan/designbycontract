using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Givn;
using NUnit.Framework;

namespace DesignByContract.Tests
{
    [TestFixture]
    public class DbcTests
    {
        [Test]
        public void WhenAssertingSomethingTrue()
        {
            Giv.n(ATruthyCondition);
            Wh.n(() => IExpressionThePreconditionWithMessage("A message"));
            Th.n(AnExceptionIsNotThrown);
        }

        [Test]
        public void WhenAssertingSomethingFalse()
        {
            Giv.n(AFalseyCondition);
            Wh.n(() => IExpressionThePreconditionWithMessage("A message"));
            Th.n(AnExceptionIsThrown)
                .And(() => TheExceptionIncludesTheMessage("A message"));
        }

        [Test]
        public void WhenAssertingSomethingWithMultipleConditions()
        {
            Giv.n(ATruthyCondition);
            Wh.n(() => IExpressionThePreconditionWithMessage("A message"));
            Th.n(ARequiresWithMultipleConditionsThrowsAnExceptionIfAnyConditionIsFalsey);
        }

        private void ARequiresWithMultipleConditionsThrowsAnExceptionIfAnyConditionIsFalsey()
        {
            _ex = Assert.Throws<DbcException>(() => Dbc.Requires(true, _message).And(false));
        }

        private void ATruthyCondition()
        {
            _condition = true;
        }

        private void AFalseyCondition()
        {
            _condition = false;
        }

        private void IExpressionThePreconditionWithMessage(string message)
        {
            _message = message;
        }

        private void AnExceptionIsThrown()
        {
            _ex = Assert.Throws<DbcException>(() => Dbc.Requires(_condition, _message));
        }

        private void AnExceptionIsNotThrown()
        {
            Assert.DoesNotThrow(() => Dbc.Requires(_condition, _message));
        }

        private void TheExceptionIncludesTheMessage(string aMessage)
        {
            Assert.IsTrue(_ex.Message.Contains(aMessage));
        }

        private bool _condition;
        private string _message;
        private DbcException _ex;
    }
}
