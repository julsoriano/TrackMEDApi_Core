using Autofac;
using Autofac.Core;
using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrackMEDApi
{
    // To register IEntityRepository<> in a generic manner you can use a RegistrationSource : http://stackoverflow.com/questions/28551332/autofac-register-generic-on-non-generic-abstract-class
    public class DataContextRegistrationSource : IRegistrationSource
    {

        public Boolean IsAdapterForIndividualComponents
        {
            get { return true; }
        }

        public IEnumerable<IComponentRegistration> RegistrationsFor(Service service, Func<Service, IEnumerable<IComponentRegistration>> registrationAccessor)
        {
            if (service == null)
            {
                throw new ArgumentNullException("service");
            }
            if (registrationAccessor == null)
            {
                throw new ArgumentNullException("registrationAccessor");
            }

            // service must be typed
            IServiceWithType ts = service as IServiceWithType;

            // if service not typed or not(ts is generic and generic type definition is of type IEntityRepository
            if (ts == null || !(ts.ServiceType.IsGenericType && ts.ServiceType.GetGenericTypeDefinition() == typeof(IEntityRepository<>)))
            //if (ts == null || !(ts.ServiceType.IsGenericType && ts.ServiceType.GetGenericTypeDefinition() == typeof(IEntityRepository)))
                {
                yield break;
            }

            yield return RegistrationBuilder.ForType(ts.ServiceType)
                                            .AsSelf()
                                            .WithParameter(new NamedParameter("databaseName", "test"))
                                            .WithParameter(new NamedParameter("serverName", "test2"))
                                            .CreateRegistration();

        }
    }
}
