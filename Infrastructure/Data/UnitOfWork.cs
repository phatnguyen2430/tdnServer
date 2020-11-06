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
            IAnnotationRepository annotationRepository,
            IAnswerRepository answerRepository,
            IEssayAnswerRepository essayAnswerRepository,
            IEssayExerciseRepository essayExerciseRepository,
            ILogRepository logRepository,
            IMultipleChoicesAnswerRepository multipleChoicesAnswerRepository,
            IMultipleChoicesExerciseRepository multipleChoicesExerciseRepository,
            IStudentRepository studentRepository,
            ITestRepository testRepository
            )
        {
            Context = context;
            RefreshTokenRepository = refreshTokenRepository;
            AnnotationRepository = annotationRepository;
            AnswerRepository = answerRepository;
            EssayExerciseRepository = essayExerciseRepository;
            LogRepository = logRepository;
            MultipleChoicesExerciseRepository = multipleChoicesExerciseRepository;
            StudentRepository = studentRepository;
            TestRepository = testRepository;
            EssayAnswerRepository = essayAnswerRepository;
            MultipleChoicesAnswerRepository = multipleChoicesAnswerRepository;
        }
        public DbContext Context { get; }
        #region Repositories
        public IRefreshTokenRepository RefreshTokenRepository { get; }
        public IAnnotationRepository AnnotationRepository { get; }
        public IAnswerRepository AnswerRepository { get; }
        public IEssayExerciseRepository EssayExerciseRepository { get; }
        public ILogRepository LogRepository { get; }
        public IMultipleChoicesExerciseRepository MultipleChoicesExerciseRepository { get; }
        public IStudentRepository StudentRepository { get; }
        public ITestRepository TestRepository { get; }
        public IEssayAnswerRepository EssayAnswerRepository { get; }
        public IMultipleChoicesAnswerRepository MultipleChoicesAnswerRepository { get; }

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
