using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Repositories.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {

        private IDbContextTransaction _transaction;
        public UnitOfWork(NoisContext context,
            IRefreshTokenRepository refreshTokenRepository,
            IAnswerRepository answerRepository,
            IEssayAnswerRepository essayAnswerRepository,
            IEssayExerciseRepository essayExerciseRepository,
            ILogRepository logRepository,
            IMultipleChoicesAnswerRepository multipleChoicesAnswerRepository,
            IMultipleChoicesExerciseRepository multipleChoicesExerciseRepository,
            ITestRepository testRepository,
            INotificationRepository notificationRepository
            )
        {
            Context = context;
            RefreshTokenRepository = refreshTokenRepository;
            AnswerRepository = answerRepository;
            EssayExerciseRepository = essayExerciseRepository;
            LogRepository = logRepository;
            MultipleChoicesExerciseRepository = multipleChoicesExerciseRepository;
            TestRepository = testRepository;
            EssayAnswerRepository = essayAnswerRepository;
            MultipleChoicesAnswerRepository = multipleChoicesAnswerRepository;
            NotificationRepository = notificationRepository;
        }
        public DbContext Context { get; }
        #region Repositories
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IAnswerRepository AnswerRepository { get; }
        public IEssayExerciseRepository EssayExerciseRepository { get; }
        public ILogRepository LogRepository { get; }
        public IMultipleChoicesExerciseRepository MultipleChoicesExerciseRepository { get; }
        public ITestRepository TestRepository { get; }
        public IEssayAnswerRepository EssayAnswerRepository { get; }
        public IMultipleChoicesAnswerRepository MultipleChoicesAnswerRepository { get; }
        public INotificationRepository NotificationRepository { get; }

        #endregion


        public async Task SaveChangesAsync()
        {
            await Context.SaveChangesAsync();
        }

        public async Task CreateTransactionAsync()
        {
            _transaction = await Context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            await _transaction.CommitAsync();
        }

        public async Task RollbackAsync()
        {
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
        }


        public void Dispose()
        {
            Context?.Dispose();
        }
    }
}
