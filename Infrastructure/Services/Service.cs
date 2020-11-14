using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.AnswerService;
using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.EssayExerciseService;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Interfaces.MultipleChoicesExerciseService;
using ApplicationCore.Interfaces.NotificationsService;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Interfaces.TestService;

namespace Infrastructure.Services
{
    public class Service : IService
    {
        public Service(IEmailService emailService,
                       IIdentityService identityService,
                       IRabbitMQService rabbitMQService,
                       IAzureBlobService azureBlobService,
                       IAnswerService answerService,
                       IEssayExerciseService essayExerciseService,
                       ILogService logService,
                       IMultipleChoicesExerciseService multipleChoicesExerciseService,
                       ITestService testService,
                       IMultipleChoicesAnswerService multipleChoicesAnswerService,
                       IEssayAnswerService essayAnswerService,
                       INotificationService notificationService)
        {
            EmailService = emailService;
            IdentityService = identityService;
            RabbitMQService = rabbitMQService;
            AzureBlobService = azureBlobService;
            AnswerService = answerService;
            EssayExerciseService = essayExerciseService;
            LogService = logService;
            MultipleChoicesExerciseService = multipleChoicesExerciseService;
            TestService = testService;
            EssayAnswerService = essayAnswerService;
            MultipleChoicesAnswerService = multipleChoicesAnswerService;
            NotificationService = notificationService;
        }

        public IEmailService EmailService { get; }
        public IIdentityService IdentityService { get; }
        public IRabbitMQService RabbitMQService { get; }
        public IAzureBlobService AzureBlobService { get; }
        public IAnswerService AnswerService { get; }
        public IEssayExerciseService EssayExerciseService { get; }
        public ILogService LogService { get; }
        public IMultipleChoicesExerciseService MultipleChoicesExerciseService { get; }
        public ITestService TestService { get; }
        public IEssayAnswerService EssayAnswerService { get; }
        public IMultipleChoicesAnswerService MultipleChoicesAnswerService { get; }
        public INotificationService NotificationService { get; }
    }
}
