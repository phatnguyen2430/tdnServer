﻿using ApplicationCore.Interfaces.AnswerService;
using ApplicationCore.Interfaces.AzureBlobService;
using ApplicationCore.Interfaces.Email;
using ApplicationCore.Interfaces.EssayExerciseService;
using ApplicationCore.Interfaces.Identity;
using ApplicationCore.Interfaces.LogService;
using ApplicationCore.Interfaces.MultipleChoicesExerciseService;
using ApplicationCore.Interfaces.NotificationsService;
using ApplicationCore.Interfaces.RabbitMQ;
using ApplicationCore.Interfaces.TestService;

namespace ApplicationCore.Interfaces
{
    public interface IService
    {
        IEmailService EmailService { get; }
        IIdentityService IdentityService { get; }
        IRabbitMQService RabbitMQService { get; }
        IAzureBlobService AzureBlobService { get; }
        IAnswerService AnswerService { get; }
        IEssayExerciseService EssayExerciseService { get; }
        ILogService LogService { get; }
        IMultipleChoicesExerciseService MultipleChoicesExerciseService { get; }
        ITestService TestService { get; }
        IMultipleChoicesAnswerService MultipleChoicesAnswerService { get; }
        IEssayAnswerService EssayAnswerService { get; }
        INotificationService NotificationService { get; }
    }
}
