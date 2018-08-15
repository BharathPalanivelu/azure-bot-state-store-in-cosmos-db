using Autofac;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Microsoft.Bot.Builder.Azure;
using System.Reflection;

namespace Bot_State_Application
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //GlobalConfiguration.Configure(WebApiConfig.Register);

            Uri docDbServiceEndpoint = new Uri(ConfigurationManager.AppSettings["DocumentDbServiceEndpoint"]);
            string docDbEmulatorKey = ConfigurationManager.AppSettings["DocumentDbAuthKey"];
            var builder = new ContainerBuilder();
            builder.RegisterModule(new AzureModule(Assembly.GetExecutingAssembly()));
            var store = new DocumentDbBotDataStore(docDbServiceEndpoint, docDbEmulatorKey);
            builder.Register(c => store).Keyed<IBotDataStore<BotData>>(AzureModule.Key_DataStore)
                 .AsSelf()
                 .SingleInstance();
            builder.Update(Conversation.Container);
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
