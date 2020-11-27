using System;

namespace PlaystationGamesImporterWebJob
{
    public class ServiceLocator
    {
        public static IServiceProvider Instance { get; internal set; }
    }
}