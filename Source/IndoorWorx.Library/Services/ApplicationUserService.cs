using IndoorWorx.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IndoorWorx.Infrastructure.Models;
using IndoorWorx.Infrastructure.Criteria;
using Microsoft.Practices.ServiceLocation;
using IndoorWorx.Infrastructure.Repositories;
using System.ServiceModel.Activation;
using IndoorWorx.Infrastructure.Responses;
using IndoorWorx.Infrastructure.Requests;

namespace IndoorWorx.Library.Services
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly IServiceLocator serviceLocator;
        public ApplicationUserService(IServiceLocator serviceLocator)
        {
            this.serviceLocator = serviceLocator;
        }

        #region Repositories

        private IApplicationUserRepository ApplicationUserRepository
        {
            get { return serviceLocator.GetInstance<IApplicationUserRepository>(); }
        }

        private IReferralSourcesRepository ReferralSourcesRepository
        {
            get { return serviceLocator.GetInstance<IReferralSourcesRepository>(); }
        }

        private IOccupationRepository OccupationRepository
        {
            get { return serviceLocator.GetInstance<IOccupationRepository>(); }
        }

        private IVideoRepository VideoRepository
        {
            get { return serviceLocator.GetInstance<IVideoRepository>(); }
        }

        private ITrainingSetTemplateRepository TemplateRepository
        {
            get { return serviceLocator.GetInstance<ITrainingSetTemplateRepository>(); }
        }

        #endregion

        #region Methods

        public ApplicationUser SaveApplicationUser(ApplicationUser user)
        {
            var result = ApplicationUserRepository.Save(user);
            return result;
        }

        public ApplicationUser RetrieveApplicationUser(ApplicationUserFindCriteria criteria)
        {
            var result = ApplicationUserRepository.FindOne(u=> u.Username == criteria.Username);
            return result;
        }

        public ICollection<Occupation> RetrieveAllOccupations()
        {
            var result = OccupationRepository.FindAll(a => a.IsActive == true).ToList();
            return result;
        }

        public ICollection<ReferralSource> RetrieveAllReferralSources()
        {
            var result = ReferralSourcesRepository.FindAll(a => a.IsActive == true);
            return result;
        }

        public AddVideoResponse AddVideoToLibrary(AddVideoRequest request)
        {
            var response = new AddVideoResponse();
            try
            {
                var user = ApplicationUserRepository.FindOne(u => u.Username == request.User);
                if (user.Videos.Any(x => x.Id == request.VideoId))
                {
                    response.AddVideoStatus = AddVideoStatus.VideoAlreadyAdded;
                    return response;
                }
                var video = VideoRepository.FindOne(v => v.Id == request.VideoId);
                if (user.Credits < video.Credits)
                {
                    response.AddVideoStatus = AddVideoStatus.InsufficientCredits;
                    return response;
                }
                user.Videos.Add(video);
                ApplicationUserRepository.Save(user);
                response.AddVideoStatus = AddVideoStatus.Success;
            }
            catch (Exception ex)
            {
                response.AddVideoStatus = AddVideoStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }        

        public AddTemplateResponse AddTemplateToLibrary(AddTemplateRequest request)
        {
            var response = new AddTemplateResponse();
            try
            {
                var user = ApplicationUserRepository.FindOne(u => u.Username == request.User);
                if (user.Templates.Any(x => x.Id == request.TemplateId))
                {
                    response.AddTemplateStatus = AddTemplateStatus.TemplateAlreadyAdded;
                    return response;
                }
                var template = TemplateRepository.FindOne(v => v.Id == request.TemplateId);
                if (user.Credits < template.Credits)
                {
                    response.AddTemplateStatus = AddTemplateStatus.InsufficientCredits;
                    return response;
                }
                user.Templates.Add(template);
                ApplicationUserRepository.Save(user);
                response.AddTemplateStatus = AddTemplateStatus.Success;
            }
            catch (Exception ex)
            {
                response.AddTemplateStatus = AddTemplateStatus.Error;
                response.Message = ex.Message;
            }
            return response;
        }
        #endregion
    }
}
