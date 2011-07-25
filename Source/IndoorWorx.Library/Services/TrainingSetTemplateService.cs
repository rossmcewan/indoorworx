using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Services;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using System.ServiceModel.Activation;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;
using System.Transactions;
using IndoorWorx.Infrastructure.Models;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class TrainingSetTemplateService : ITrainingSetTemplateService
    {
        private readonly IServiceLocator serviceLocator;
        public TrainingSetTemplateService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        public ITrainingSetTemplateRepository Repository
        {
            get { return serviceLocator.GetInstance<ITrainingSetTemplateRepository>(); }
        }

        public IApplicationUserRepository ApplicationUserRepository
        {
            get { return serviceLocator.GetInstance<IApplicationUserRepository>(); }
        }

        public ICollection<Infrastructure.Models.TrainingSetTemplate> FindAll()
        {
            return Repository.FindAll(x => x.IsPublic);
        }

        public SaveTemplateResponse Save(SaveTemplateRequest request)
        {
            var response = new SaveTemplateResponse();
            try
            {
                //using (var scope = new TransactionScope())
                //{
                    var user = ApplicationUserRepository.FindOne(u => u.Username == request.User);
                    var template = Repository.Save(request.Template);
                    if (!user.Templates.Contains(template))
                    {
                        user.Templates.Add(template);
                        user = ApplicationUserRepository.Save(user);
                    }
                    response.Credits = user.Credits;
                    response.SavedTemplate = template;
                    response.Status = SaveTemplateStatus.Success;
                    //scope.Complete();
                //}
            }
            catch (Exception ex)
            {
                response.Status = SaveTemplateStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }

        public RemoveTemplateResponse Remove(RemoveTemplateRequest request)
        {
            var response = new RemoveTemplateResponse();
            try
            {
                var user = ApplicationUserRepository.FindOne(u => u.Username == request.User);
                if (user.Templates.Contains(request.Template))
                {
                    user.Templates.Remove(request.Template);
                    user = ApplicationUserRepository.Save(user);
                    if (!request.Template.IsPublic)
                    {
                        Repository.Delete(request.Template);
                    }
                }
                response.Credits = user.Credits;
                response.Status = RemoveTemplateStatus.Success;                
            }
            catch (Exception ex)
            {
                response.Status = RemoveTemplateStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }        
    }
}
