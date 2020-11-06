using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using ApplicationCore.Interfaces.Repositories;
using ApplicationCore.Interfaces.Repositories.Identity;

namespace ApplicationCore.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        DbContext Context { get; }
        Task CreateTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
        Task SaveChangesAsync();

        #region Repositories
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IAnnotationRepository AnnotationRepository { get; }
        IAnswerRepository AnswerRepository { get; }
        IEssayExerciseRepository EssayExerciseRepository { get; }
        ILogRepository LogRepository { get; }
        IMultipleChoicesExerciseRepository MultipleChoicesExerciseRepository { get; }
        IEssayAnswerRepository EssayAnswerRepository { get; }
        IMultipleChoicesAnswerRepository MultipleChoicesAnswerRepository { get; }
        IStudentRepository StudentRepository { get; }
        ITestRepository TestRepository { get; }
        #endregion
    }
}
