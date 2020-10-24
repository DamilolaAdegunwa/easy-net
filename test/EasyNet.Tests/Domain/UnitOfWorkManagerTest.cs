using System;
using System.Threading.Tasks;
using System.Transactions;
using EasyNet.Domain.Uow;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace EasyNet.Tests.Domain
{
    public class UnitOfWorkManagerTest
    {
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWorkManagerTest()
        {
            var services = new ServiceCollection();
            services.AddTransient<IUnitOfWork, NullUnitOfWork>();
            services.AddScoped<ICurrentUnitOfWorkProvider, AsyncLocalCurrentUnitOfWorkProvider>();
            services.AddScoped<IUnitOfWorkManager, UnitOfWorkManager>();
            services.Configure<UnitOfWorkDefaultOptions>(o =>
            {
                o.Timeout = TimeSpan.Zero;
            });

            _serviceProvider = services.BuildServiceProvider();
        }

        [Fact]
        public void TestInit()
        {
            // Arrange
            var currentUnitOfWorkProvider = _serviceProvider.GetService<ICurrentUnitOfWorkProvider>();
            var unitOfWorkManager = _serviceProvider.GetService<IUnitOfWorkManager>();

            // Assert
            Assert.Null(currentUnitOfWorkProvider.Current);
            Assert.Null(unitOfWorkManager.Current);
        }

        [Fact]
        public void TestStartNewUnitOfWork()
        {
            // Arrange
            var currentUnitOfWorkProvider = _serviceProvider.GetService<ICurrentUnitOfWorkProvider>();
            var unitOfWorkManager = _serviceProvider.GetService<IUnitOfWorkManager>();

            #region Test Begin

            // Act
            var unitOfWorkOptions = new UnitOfWorkOptions
            {
                IsTransactional = false,
                IsolationLevel = IsolationLevel.Snapshot
            };
            var unitOfWork = unitOfWorkManager.Begin(unitOfWorkOptions);

            // Assert
            Assert.True(unitOfWork is IUnitOfWork);
            Assert.Same(unitOfWork, unitOfWorkManager.Current);
            Assert.Same(unitOfWork, currentUnitOfWorkProvider.Current);
            Assert.Same(unitOfWorkManager.Current, currentUnitOfWorkProvider.Current);
            Assert.Equal(false, ((NullUnitOfWork)unitOfWork).Options.IsTransactional);
            Assert.Equal(IsolationLevel.Snapshot, ((NullUnitOfWork)unitOfWork).Options.IsolationLevel);
            Assert.Equal(TimeSpan.Zero, ((NullUnitOfWork)unitOfWork).Options.Timeout);

            #endregion

            #region Test Complete

            // Act
            unitOfWork.Complete();

            // Assert
            Assert.Null(currentUnitOfWorkProvider.Current);
            Assert.Null(unitOfWorkManager.Current);

            #endregion

            #region Test Dispose

            // Act
            unitOfWork.Dispose();

            // Assert
            Assert.True(((IUnitOfWork)unitOfWork).IsDisposed);

            #endregion
        }

        [Fact]
        public async Task TestNestedUnitOfWork()
        {
            // Arrange
            var currentUnitOfWorkProvider = _serviceProvider.GetService<ICurrentUnitOfWorkProvider>();
            var unitOfWorkManager = _serviceProvider.GetService<IUnitOfWorkManager>();

            // Act & Assert

            // Start a uow as top uow, the TransactionScope is Required.
            using (var unitOfWork1 = unitOfWorkManager.Begin(TransactionScopeOption.Required))
            {
                Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current);

                #region Start a new child uow as unitOfWork2

                // Start a child uow, the TransactionScope is Required, too.
                // The current uow is not changed. The current uow is unitOfWork1, too.
                using (var unitOfWork2 = unitOfWorkManager.Begin(TransactionScopeOption.Required))
                {
                    Assert.True(unitOfWork2 is InnerUnitOfWorkCompleteHandle);
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current);

                    // Must call method Complete before dispose.
                    Assert.Throws<EasyNetException>(() => unitOfWork2.Dispose());

                    await unitOfWork2.CompleteAsync();
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current);
                }

                #endregion

                #region Start a new child uow as unitOfWork3

                // Start a child uow, the TransactionScope is RequiresNew.
                // It will change the current uow to this uow.
                using (var unitOfWork3 = unitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
                {
                    Assert.Same(unitOfWork3, currentUnitOfWorkProvider.Current);
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current.Outer);

                    // The current uow will be changed to unitOfWork1 after execute method Complete if there is a parent uow. 
                    await unitOfWork3.CompleteAsync();
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current);
                }

                #endregion

                #region Start a new child uow as unitOfWork4

                // Start a child uow, the TransactionScope is Suppress.
                // It will change the current uow to this uow.
                using (var unitOfWork4 = unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
                {
                    Assert.Same(unitOfWork4, currentUnitOfWorkProvider.Current);
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current.Outer);

                    #region Start a new child uow as unitOfWork5

                    // Start a new child now, the TransactionScope is Required.
                    // The current uow is not changed. The current uow is unitOfWork4, too.
                    using (var unitOfWork5 = unitOfWorkManager.Begin(TransactionScopeOption.Required))
                    {
                        Assert.True(unitOfWork5 is InnerSuppressUnitOfWorkCompleteHandle);
                        Assert.Same(unitOfWork4, currentUnitOfWorkProvider.Current);

                        // Must call method Complete before dispose.
                        Assert.Throws<EasyNetException>(() => unitOfWork5.Dispose());

                        await unitOfWork5.CompleteAsync();
                        Assert.Same(unitOfWork4, currentUnitOfWorkProvider.Current);
                    }

                    #endregion

                    // The current uow will be changed to unitOfWork1 after execute method Complete if there is a parent uow. 
                    await unitOfWork4.CompleteAsync();
                    Assert.Same(unitOfWork1, currentUnitOfWorkProvider.Current);
                }

                #endregion
            }
        }
    }
}
