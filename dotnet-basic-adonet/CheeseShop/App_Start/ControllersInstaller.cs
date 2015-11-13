using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Castle.Windsor.Mvc;
using CheeseShop.Common;
using CheeseShop.Domain.Bus;
using CheeseShop.Domain.Events;
using CheeseShop.Domain.Members;
using CheeseShop.Domain.Passwords;

namespace CheeseShop
{
    public class ControllersInstaller : IWindsorInstaller
    {
        public static IWindsorContainer Register()
        {
            var container = new WindsorContainer()
                .Install(FromAssembly.This());
            var controllerFactory = new WindsorControllerFactory(container.Kernel);
            ControllerBuilder.Current.SetControllerFactory(controllerFactory);
            return container;
        }

        public void Install(IWindsorContainer container,
                            IConfigurationStore store)
        {
            var cs = ConfigurationManager.ConnectionStrings["CheeseShop"].ConnectionString; 

            container.Register(Classes.FromThisAssembly()
                                      .BasedOn<IController>()
                                      .LifestyleTransient(),
                               Component.For<IWindsorContainer>()
                                        .Instance(container)
                                        .LifestyleSingleton(),
                               Component.For<IDbConnection>()
                                        .UsingFactoryMethod(
                                            x =>
                                            {
                                                var c = new SqlConnection(cs);
                                                c.Open();
                                                return c;
                                            })
                                        .LifestylePerWebRequest(),
                               Component.For<IBus>()
                                        .ImplementedBy<Bus>()
                                        .LifestylePerWebRequest(),
                               Component.For<IPasswordFactory>()
                                        .ImplementedBy<PasswordFactory>()
                                        .LifestylePerWebRequest(),
                               Component.For<IMemberRepository>()
                                        .ImplementedBy<MemberRepository>()
                                        .LifestylePerWebRequest(),
                               Component.For<IMemberRegistrationService>()
                                        .ImplementedBy<MemberRegistrationService>()
                                        .LifestylePerWebRequest(),
                               Component.For<IFluentEmailClient>()
                                        .ImplementedBy<FluentEmailClient>()
                                        .LifestylePerWebRequest(),
                               Classes.FromAssemblyContaining<Member>()
                                      .BasedOn(typeof (IDomainEventHandler<>))
                                      .WithServiceBase()
                                      .LifestyleTransient()
                );
        }
    }
}