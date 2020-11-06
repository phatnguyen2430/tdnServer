using ApplicationCore.Interfaces;
using ApplicationCore.Interfaces.AnnotationService;
using ApplicationCore.Interfaces.AnswerService;
using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.EssayExerciseService;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Interfaces.MultipleChoicesExerciseService;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Interfaces.StudentService;
using ApplicationCore.Interfaces.TestService;

namespace Infrastructure.Services
{
    public class Service : IService
    {
        public Service(IEmailService emailService,
                       IIdentityService identityService,
                       IRabbitMQService rabbitMQService,
                       IAzureBlobService azureBlobService,
                       IAnnotationService annotationService,
                       IAnswerService answerService,
                       IEssayExerciseService essayExerciseService,
                       ILogService logService,
                       IMultipleChoicesExerciseService multipleChoicesExerciseService,
                       IStudentService studentService,
                       ITestService testService,
                       IMultipleChoicesAnswerService multipleChoicesAnswerService,
                       IEssayAnswerService essayAnswerService)
        {
            EmailService = emailService;
            IdentityService = identityService;
            RabbitMQService = rabbitMQService;
            AzureBlobService = azureBlobService;
            AnnotationService = annotationService;
            AnswerService = answerService;
            EssayExerciseService = essayExerciseService;
            LogService = logService;
            MultipleChoicesExerciseService = multipleChoicesExerciseService;
            StudentService = studentService;
            TestService = testService;
            EssayAnswerService = essayAnswerService;
            MultipleChoicesAnswerService = multipleChoicesAnswerService;
        }

        public IEmailService EmailService { get; }
        public IIdentityService IdentityService { get; }
        public IRabbitMQService RabbitMQService { get; }
        public IAzureBlobService AzureBlobService { get; }
        public IAnnotationService AnnotationService { get; }
        public IAnswerService AnswerService { get; }
        public IEssayExerciseService EssayExerciseService { get; }
        public ILogService LogService { get; }
        public IMultipleChoicesExerciseService MultipleChoicesExerciseService { get; }
        public IStudentService StudentService { get; }
        public ITestService TestService { get; }
        public IEssayAnswerService EssayAnswerService { get; }
        public IMultipleChoicesAnswerService MultipleChoicesAnswerService { get; }
    }
}
