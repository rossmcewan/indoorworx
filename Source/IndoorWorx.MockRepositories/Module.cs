using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Composite.Modularity;
using Microsoft.Practices.Unity;
using IndoorWorx.Infrastructure.Repositories;

namespace IndoorWorx.MockRepositories
{
    public class Module : IModule
    {
        [Dependency]
        public IUnityContainer Container { get; set; }

        #region IModule Members

        public void Initialize()
        {
            Container.RegisterInstance<ICatalogRepository>(Container.Resolve<CatalogRepository>());
            Container.RegisterInstance<ICategoryRepository>(Container.Resolve<CategoryRepository>());
            Container.RegisterInstance<IVideoRepository>(Container.Resolve<VideoRepository>());
            Container.RegisterInstance<IApplicationUserRepository>(Container.Resolve<ApplicationUserRepository>());
            Container.RegisterInstance<IOccupationRepository>(Container.Resolve<OccupationRepository>());
            Container.RegisterInstance<ITrainingVolumeRepository>(Container.Resolve<TrainingVolumeRepository>());
            Container.RegisterInstance<IIndoorTrainingFrequencyRepository>(Container.Resolve<IndoorTrainingFrequencyRepository>());
            Container.RegisterInstance<IReferralSourcesRepository>(Container.Resolve<ReferralSourcesRepository>());
            Container.RegisterInstance<ICompetitiveLevelRepository>(Container.Resolve<CompetitiveLevelRepository>());
        }

        #endregion
    }

    class CatalogRepository : ICatalogRepository
    {
        public Infrastructure.Models.Catalog Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.Catalog> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Catalog, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Catalog FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Catalog, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Catalog FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Catalog, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Catalog Save(Infrastructure.Models.Catalog entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.Catalog entity)
        {
            throw new NotImplementedException();
        }
    }

    class CategoryRepository : ICategoryRepository
    {
        public Infrastructure.Models.Category Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.Category> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Category, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Category FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Category, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Category FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Category, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Category Save(Infrastructure.Models.Category entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.Category entity)
        {
            throw new NotImplementedException();
        }
    }

    class VideoRepository : IVideoRepository
    {
        public Infrastructure.Models.Video Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.Video> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Video, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Video FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Video, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Video FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Video, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Video Save(Infrastructure.Models.Video entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.Video entity)
        {
            throw new NotImplementedException();
        }
    }

    class ApplicationUserRepository : IApplicationUserRepository
    {
        public Infrastructure.Models.ApplicationUser Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.ApplicationUser> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ApplicationUser, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ApplicationUser FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ApplicationUser, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ApplicationUser FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ApplicationUser, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ApplicationUser Save(Infrastructure.Models.ApplicationUser entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.ApplicationUser entity)
        {
            throw new NotImplementedException();
        }
    }

    class OccupationRepository : IOccupationRepository
    {
        public Infrastructure.Models.Occupation Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.Occupation> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Occupation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Occupation FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Occupation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Occupation FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.Occupation, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.Occupation Save(Infrastructure.Models.Occupation entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.Occupation entity)
        {
            throw new NotImplementedException();
        }
    }

    class TrainingVolumeRepository : ITrainingVolumeRepository
    {
        public Infrastructure.Models.TrainingVolume Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.TrainingVolume> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.TrainingVolume, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.TrainingVolume FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.TrainingVolume, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.TrainingVolume FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.TrainingVolume, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.TrainingVolume Save(Infrastructure.Models.TrainingVolume entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.TrainingVolume entity)
        {
            throw new NotImplementedException();
        }
    }

    class IndoorTrainingFrequencyRepository : IIndoorTrainingFrequencyRepository
    {
        public Infrastructure.Models.IndoorTrainingFrequency Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.IndoorTrainingFrequency> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.IndoorTrainingFrequency, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.IndoorTrainingFrequency FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.IndoorTrainingFrequency, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.IndoorTrainingFrequency FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.IndoorTrainingFrequency, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.IndoorTrainingFrequency Save(Infrastructure.Models.IndoorTrainingFrequency entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.IndoorTrainingFrequency entity)
        {
            throw new NotImplementedException();
        }
    }

    class ReferralSourcesRepository : IReferralSourcesRepository
    {
        public Infrastructure.Models.ReferralSource Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.ReferralSource> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ReferralSource, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ReferralSource FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ReferralSource, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ReferralSource FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.ReferralSource, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.ReferralSource Save(Infrastructure.Models.ReferralSource entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.ReferralSource entity)
        {
            throw new NotImplementedException();
        }
    }

    class CompetitiveLevelRepository : ICompetitiveLevelRepository
    {
        public Infrastructure.Models.CompetitiveLevel Get(object id)
        {
            throw new NotImplementedException();
        }

        public ICollection<Infrastructure.Models.CompetitiveLevel> FindAll(System.Linq.Expressions.Expression<Func<Infrastructure.Models.CompetitiveLevel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.CompetitiveLevel FindOne(System.Linq.Expressions.Expression<Func<Infrastructure.Models.CompetitiveLevel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.CompetitiveLevel FindFirst(System.Linq.Expressions.Expression<Func<Infrastructure.Models.CompetitiveLevel, bool>> where)
        {
            throw new NotImplementedException();
        }

        public Infrastructure.Models.CompetitiveLevel Save(Infrastructure.Models.CompetitiveLevel entity)
        {
            throw new NotImplementedException();
        }


        public void Delete(Infrastructure.Models.CompetitiveLevel entity)
        {
            throw new NotImplementedException();
        }
    }

}
