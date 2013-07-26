using System;
using System.Collections.Generic;
using System.Linq;
using Givn;
using NUnit.Framework;

namespace DesignByContract.Tests
{
    [TestFixture]
    public class NotNullOrEmpty
    {
        [Test]
        public void WhenAValueIsNull()
        {
            Assert.Throws<DbcException>(() => Dbc.NotNullOrEmpty((string)null));
        }
        
        [Test]
        public void WhenAValueIsEmptyString()
        {
            Assert.Throws<DbcException>(() => Dbc.NotNullOrEmpty(""));
        }
        
        [Test]
        public void WhenAValueIsEmptyCollection()
        {
            Assert.Throws<DbcException>(() => Dbc.NotNullOrEmpty(new List<int>()));
            Assert.Throws<DbcException>(() => Dbc.NotNullOrEmpty(new int[]{}));
        }
        
        [Test]
        public void WhenAValueIsNotEmpty()
        {
            Assert.DoesNotThrow(() => Dbc.NotNullOrEmpty("foo", "some message"));
        }

        [Test]
        public void WhenACollectionIsNotEmpty()
        {
            Assert.DoesNotThrow(() => Dbc.NotNullOrEmpty(new int[] {1}, "some message"));
        }

        [Test]
        public void WhenSomeValuesAreNullOrEmpty()
        {
            Assert.Throws<DbcException>(() => Dbc.NotNullOrEmpty(new string[] {"","foo"}));
            Assert.Throws<DbcException>(() =>Dbc.NotNullOrEmpty(new string[] {"foo", null}));
        }
    }

    [TestFixture]
    public class NotNull
    {
        private object _value;
        private object[] _vls;

        [Test]
        public void WhenAValueIsNull()
        {
            Giv.n(() => AValue(null));
            Th.n(() => Assert.Throws<DbcException>(() => Dbc.NotNull(_value)));
        }
        
        [Test]
        public void WhenOneOfMultipleValuesIsNull()
        {
            Giv.n(() => Values("", 1, null));
            Th.n(() => Assert.Throws<DbcException>(() => Dbc.NotNull(_vls, "message")));
        }

        private void Values(params object[] vls)
        {
            _vls = vls;
        }

        private void AValue(object o)
        {
            _value = o;
        }
    }

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
