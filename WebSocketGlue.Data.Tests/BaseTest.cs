﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace WebSocketGlue.Data.Tests {
  public class BaseTest {
    protected BaseTest() {
      Fixture = new Fixture();
    }

    protected Fixture Fixture {get;}
    protected CancellationToken Token => new CancellationTokenSource().Token;

    [TestInitialize]
    public void BaseTestInitialize() {
      mMocks.Clear();
    }

    [TestCleanup]
    public void BaseTestCleanup() {
      Array.ForEach(mMocks.ToArray(), m => m.VerifyAll());
    }

    public void IsGreaterThan(int actual, int expected) {
      Assert.IsTrue(actual > expected, $"{expected} is not greater than {actual}");
    }

    public T One<T>() {
      return Fixture.Create<T>();
    }

    public T[] Many<T>() {
      return Many<T>(3);
    }

    public T[] Many<T>(int count) {
      return Enumerable.Range(0, count)
                       .Select(i => One<T>())
                       .ToArray();
    }

    protected Mock<T> Mock<T>() where T : class {
      var mock = new Mock<T>(MockBehavior.Strict);
      mMocks.Add(mock);
      return mock;
    }

    protected static T[] BA<T>(params T[] values) {
      return values;
    }

    protected static Task Tsk() {
      return Tsk(true);
    }

    protected static Task<T> Tsk<T>(T obj) {
      return Task.FromResult(obj);
    }

    protected async Task AssertThrowsWithMessageAsync<T>(Func<Task> func, string message) where T : Exception {
      try {
        await func();
      } catch (T ex) {
        Assert.AreEqual(typeof(T), ex.GetType());
        Assert.AreEqual(ex.Message, message);
      }
    }

    private readonly IList<Mock> mMocks = new List<Mock>();
  }
}